## TODO 0:

There was an issue with ConfigureServices. One service was missing and using 'UseMvc' to configure MVC is not supported while using Endpoint Routing. To fix that i:

**Startup.cs**

Specified mvc service options
``` csharp
services.AddMvc(options => options.EnableEndpointRouting = false);
```

added missing service

``` csharp
services.AddSingleton<IAccountService, AccountService>();
```

## TODO 1:

I used cookie authentication without ASP.NET Core Identity.
Cookie was generated with `HttpContext.SignInAsync(scheme, principal)` method.

More about authentication and authorization can be found in **TODO4** description

**LoginController.cs**

``` csharp
var claims = new List<Claim>
{
    new Claim(ClaimTypes.Name, account.UserName),
    new Claim(ClaimTypes.Role, account.Role),
    new Claim(ClaimTypesExtension.ExternalId, account.ExternalId),
};

var claimsIdentity = new ClaimsIdentity(
    claims, CookieAuthenticationDefaults.AuthenticationScheme);
await HttpContext.SignInAsync(
    CookieAuthenticationDefaults.AuthenticationScheme,
    new ClaimsPrincipal(claimsIdentity));

return Ok();
```

also:
changed the signature of the method to match the logic of "Example Test"

``` csharp
[HttpPost("sign-in/{userName}")]
public async Task<ActionResult<string>> Login([FromRoute]string userName)
```

## TODO 2:

**LoginController.cs**

if  user was not found then return 404

``` csharp
var account = await _db.FindByUserNameAsync(userName);
if (account != null)
{
...
}
return NotFound();
```

## TODO 3:

**AccountController.cs**

Getting ExternalId

``` csharp
var externalId = User.FindFirst(ClaimTypesExtension.ExternalId).Value;
return _accountService.LoadOrCreateAsync(externalId);
```


## TODO 4:

**Startup.cs**

In order to use Authorization and Authentication we should configure services.  To make unauthorized users receive 401 status code we should specify redirect events (because of behavior of authentication middleware, which tries to redirect to login page).

``` csharp
services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        return Task.CompletedTask;
    };
    options.Events.OnRedirectToAccessDenied = options.Events.OnRedirectToLogin
});
```
and add proper middlewares to make it work
``` csharp
app.UseAuthentication();
app.UseAuthorization();
```

## TODO 5:

**AccountController.cs**

To make api/account/{id} endpoint only for admins we can specify property "Role". That will access only users with "Admin" role
``` csharp
[Authorize(Roles = "Admin")]
```

## TODO 6:

There was an issue with cache update logic. To prevent this, update cache and database value on every counter increment.

**AccountController.cs**

``` csharp
account.Counter++;
_accountService.Update(account);
```

**IAccountService.cs**

``` csharp
void Update(Account account);
```

**AccountService.cs**

``` csharp
public void Update(Account account)
{
    _db.Update(account);
    _cache.AddOrUpdate(account);
}
```

Also add new method to AccountDatabase interface and implementation

**IAccountDatabase.cs**

``` csharp
void Update(Account account);
```

**AccountDatabaseStub.cs**
``` csharp
public void Update(Account account)
{
    lock (this)
    {
        _accounts[account.ExternalId] = account;
    }
}
```

## Other changes
New support class to represent ExternalId in claims

**ClaimTypesExtension.cs**

``` csharp
public static class ClaimTypesExtension
{
    public const string ExternalId = "ExternalId";
}
```

## Tests
Four test were made for controllers. Two on each controller.

**LoginControllerTest.cs**

``` csharp
[Test]
public async Task TryLoginEmptyLogin_NotFoundReturned()
{
    var response = await Env.WebAppHost.GetClient().SignInAsync(string.Empty);
    response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
}

[Test]
public async Task TryLoginAlice_ReturnedSuccess()
{
    (await CreateAuthorizedClientAsync("alice@mailinator.com")).Should().BeOfType<HttpClient>();
}
```

These tests ensure that client both can and cannot be made

**AccountControllerTest.cs**

``` csharp
[Test]
public async Task TryGetAccountById_Unauthorized_401Returned()
{
    var userClient = await CreateAuthorizedClientAsync("bob@mailinator.com");
    var response = await userClient.GetAccountByIdAsync(0);
    response.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
}

[Test]
public async Task TryGetAccountById_Success()
{
    var userClient = await CreateAuthorizedClientAsync("alice@mailinator.com");
    _ = await userClient.GetAccountAsync();
    var response = await userClient.GetAccountByIdAsync(1);
    response.StatusCode.Should().Be((int)HttpStatusCode.OK);
}
```

These tests confirms proper status codes on different roles access tries.
