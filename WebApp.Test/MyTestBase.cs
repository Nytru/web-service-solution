using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
using NUnit.Framework;

namespace WebApp.Test
{
    // This project designed to simplify the process of solving code problems for you.
    // I recommend you to write tests to verify your code. But you can go by your own way and it's not a bad choice.
    // Remember that it's just a recommendation and presence or absence of tests will not have a large affect on
    // evaluation of result. 90% of the assessment will consist of quantity and quality of solved TODOs.
    // Good luck.:)
    [TestFixture]
    public class MyTestBase
    {
        protected WebAppTestEnvironment Env { get; set; }
        protected HttpClient Client { get; set; }
        protected HttpClient AliceClient { get; set; }
        protected HttpClient BobClient { get; set; }

        [OneTimeSetUp]
        public void Init()
        {
            Env = new WebAppTestEnvironment();
            Env.Start();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Env.Dispose();
            Client.Dispose();
        }

        [SetUp]
        public void Prepare()
        {
            Env.Prepare();
            Client = Env.WebAppHost.GetClient();
            AliceClient = CreateAuthorizedClientAsync("alice@mailinator.com").GetAwaiter().GetResult();
            BobClient = CreateAuthorizedClientAsync("bob@mailinator.com").GetAwaiter().GetResult();
        }

        protected async Task<HttpClient> CreateAuthorizedClientAsync(string login)
        {
            var client = Env.WebAppHost.GetClient();
            var res = await client.SignInAsync(login);
            client.DefaultRequestHeaders.Add(HeaderNames.Cookie, res.Headers.GetValues(HeaderNames.SetCookie));
            return client;
        }
    }
}