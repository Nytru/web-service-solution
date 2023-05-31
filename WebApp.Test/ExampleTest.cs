using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace WebApp.Test
{
    public class ExampleTest : MyTestBase
    {
        // This is example test for TODO6. It should works correctly if you've solved other TODOs in the right way.
        // The test structure also is an solution hint.
        [Test(Description = "TODO 6"), Order(1)]
        [Repeat(30)]
        public async Task Todo6()
        {
            var tasks = new List<Task>();
            for (var i = 0; i < 200; i++)
            {
                tasks.Add(new Task(async () => await AliceClient.GetAccountAsync()));
            }

            tasks.AsParallel().ForAll(x => x.Start());
            await Task.WhenAll(tasks);
            await AliceClient.CountAsync();
            await AliceClient.CountAsync();
            await AliceClient.CountAsync();
            await AliceClient.CountAsync();
            await AliceClient.CountAsync();
            var account = await (await AliceClient.GetAccountByIdAsync(1)).Response<Account>();
            if (account.Counter != 5)
                throw new Exception($"counter is {account.Counter}");
        }
    }
}
