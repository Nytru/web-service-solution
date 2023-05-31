using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace WebApp
{
    [Route("api")]
    public class LoginController : Controller
    {
        private readonly IAccountDatabase _db;

        public LoginController(IAccountDatabase db)
        {
            _db = db;
        }
        
        // I changed signature of method to match "Example Test" logic
        [HttpPost("sign-in/{userName}")]
        public async Task<ActionResult<string>> Login([FromRoute]string userName)
        {
            var account = await _db.FindByUserNameAsync(userName);
            if (account != null)
            {
                //TODO 1: Generate auth cookie for user 'userName' with external id
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.UserName),
                    new Claim(ClaimTypes.Role, account.Role),
                    new Claim(ClaimTypesExtension.ExternalId, account.ExternalId),
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return Ok();
            }

            //TODO 2: return 404 if user not found
            return NotFound();
        }
    }
}
