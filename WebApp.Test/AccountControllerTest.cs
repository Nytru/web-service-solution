using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace WebApp.Test
{
    public class AccountControllerTest : MyTestBase
    {
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
    }
}
