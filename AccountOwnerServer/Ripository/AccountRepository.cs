using Contracts;
using Entities;
using Entities.Models;

namespace Ripository
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {   
        }
    }
}
