using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;

namespace WebApp.Test
{
    public class LoginControllerTest : MyTestBase
    {
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
    }
}
