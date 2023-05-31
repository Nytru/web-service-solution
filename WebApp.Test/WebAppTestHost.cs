using System;
using System.Net.Http;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace WebApp.Test
{
    public class WebAppTestHost : IDisposable
    {
        private TestServer _testServer;
        public IServiceProvider Services => _testServer.Host.Services;

        public void Start()
        {
            var builder = WebHost.CreateDefaultBuilder();
            builder.UseStartup<Startup>();
            _testServer = new TestServer(builder);
        }

        public HttpClient GetClient() => _testServer.CreateClient();

        public void Dispose()
        {
            _testServer?.Dispose();
        }
    }
}