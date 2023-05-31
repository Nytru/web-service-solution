using System.Threading.Tasks;

namespace WebApp
{
    public interface IAccountService
    {
        /// <summary>
        /// Get account from cache or return null of account is not in the cache.
        /// </summary>
        Account GetFromCache(long id);
        
        /// <summary>
        /// Get account from cache or load it from db. 
        /// </summary>
        ValueTask<Account> LoadOrCreateAsync(string id);
        
        ValueTask<Account> LoadOrCreateAsync(long id);

        void Update(Account account);
    }
}