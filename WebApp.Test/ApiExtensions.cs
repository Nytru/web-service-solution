using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApp.Test
{
    public static class ApiExtensions
    {
        public static async Task<HttpResponseMessage> SignInAsync(this HttpClient client, string userName) =>
            await client.PostAsync($"api/sign-in/{userName}", null);

        public static async Task<HttpResponseMessage> GetAccountAsync(this HttpClient client) => await client.GetAsync("api/account");

        public static async Task<HttpResponseMessage> GetAccountByIdAsync(this HttpClient client, int id) =>
            await client.GetAsync($"api/account/{id}");

        public static async Task<HttpResponseMessage> CountAsync(this HttpClient client) =>
            await client.PostAsync("api/account/counter", null);

        public static async Task<T> Response<T>(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                throw new Exception(await response.Content.ReadAsStringAsync());

            return await response.Content.ReadAsAsync<T>();
        }
    }
}