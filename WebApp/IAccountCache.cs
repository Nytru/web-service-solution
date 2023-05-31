namespace WebApp
{
    public interface IAccountCache
    {
        bool TryGetValue(long accountId, out Account item);
        bool TryGetValue(string externalId, out Account item);
        void AddOrUpdate(Account account);
        bool TryRemove(long key, out Account account);
        void Clear();
    }
}