using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp
{
    class AccountDatabaseStub : IAccountDatabase
    {
        private readonly Dictionary<string, Account> _accounts = new Dictionary<string, Account>();
        private int _internalIdCounter;

        public AccountDatabaseStub()
        {
            _accounts["alice"] = new Account
            {
                ExternalId = "alice",
                InternalId = ++_internalIdCounter,
                UserName = "alice@mailinator.com",
                Role = "Admin"
            };
            _accounts["bob"] = new Account
            {
                ExternalId = "bob",
                InternalId = ++_internalIdCounter,
                UserName = "bob@mailinator.com",
                Role = "user"
            };
        }

        public Task<Account> GetOrCreateAccountAsync(string id)
        {
            lock (this)
            {
                if (!_accounts.TryGetValue(id, out var account))
                {
                    account = new Account()
                    {
                        InternalId = ++_internalIdCounter,
                        ExternalId = id
                    };
                    _accounts[id] = account;
                }
                return Task.FromResult(account.Clone());
            }
        }

        public Task<Account> GetOrCreateAccountAsync(long id)
        {
            lock (this)
            {
                var account = _accounts.FirstOrDefault(x => x.Value.InternalId == id).Value;
                if (account == null)
                {
                    account = new Account()
                    {
                        InternalId = id,
                        ExternalId = Guid.NewGuid().ToString()
                    };
                    _accounts[account.ExternalId] = account;
                }
                return Task.FromResult(account.Clone());
            }
        }

        public Task<Account> FindByUserNameAsync(string userName)
        {
            lock (this)
            {
                var account = _accounts.Values.FirstOrDefault(x => x.UserName == userName);
                return Task.FromResult(account?.Clone());
            }
        }

        public void Update(Account account)
        {
            lock (this)
            {
                _accounts[account.ExternalId] = account;
            }
        }

        public Task ResetAsync()
        {
            lock (this)
            {
                _accounts.Clear();
                _internalIdCounter = 0;
                _accounts["alice"] = new Account
                {
                    ExternalId = "alice",
                    InternalId = ++_internalIdCounter,
                    UserName = "alice@mailinator.com",
                    Role = "Admin"
                };
                _accounts["bob"] = new Account
                {
                    ExternalId = "bob",
                    InternalId = ++_internalIdCounter,
                    UserName = "bob@mailinator.com",
                    Role = "user"
                };
                return Task.CompletedTask;
            }
        }
    }}