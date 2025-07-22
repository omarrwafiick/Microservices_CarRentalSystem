
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions; 

namespace Common.Repositories
{
    public class GetRepository<TDbContext, T> : IGetRepository<T>
    where TDbContext : DbContext
    where T : class, IBaseEntity
    {
        private readonly TDbContext _context;

        public GetRepository(TDbContext context)
        {
            _context = context;
        }
        public async Task<T> Get<TID>(TID id) 
            => await _context.Set<T>().AsNoTracking().SingleOrDefaultAsync(e => EF.Property<TID>(e, "Id").Equals(e));

        public async Task<T> Get(Expression<Func<T, bool>> condition) 
            => await _context.Set<T>().AsNoTracking().Where(condition).SingleOrDefaultAsync();
        public async Task<T> Get<TID>(TID id,Expression<Func<T, object>> include)
            => await _context.Set<T>().AsNoTracking().Include(include).SingleOrDefaultAsync(e => EF.Property<TID>(e, "Id").Equals(e));
        public async Task<T> Get<TID>(TID id, Expression<Func<T, object>> include1, Expression<Func<T, object>> include2)
           => await _context.Set<T>().AsNoTracking().Include(include1).Include(include2).SingleOrDefaultAsync(e => EF.Property<TID>(e, "Id").Equals(e));

        public async Task<T> GetWithTracking<TID>(TID id)
            => await _context.Set<T>().SingleOrDefaultAsync(e => EF.Property<TID>(e, "Id").Equals(e));

        public async Task<T> GetWithTracking(Expression<Func<T, bool>> condition)
            => await _context.Set<T>().Where(condition).SingleOrDefaultAsync();
    }
}
