using AuthenticationApi.Models;
using Common.Interfaces;
using System.Linq.Expressions;

namespace AuthenticationApi.Repositories
{
    public class GetRepository : IGetRepository<User>
    {
        public Task<User> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<User> Get(Expression<Func<User, bool>> condition)
        {
            throw new NotImplementedException();
        }

        public Task<User> Get(Guid id, Expression<Func<User, object>> include)
        {
            throw new NotImplementedException();
        }

        public Task<User> Get(Guid id, Expression<Func<User, object>> include1, Expression<Func<User, object>> include2)
        {
            throw new NotImplementedException();
        }
    }
}
