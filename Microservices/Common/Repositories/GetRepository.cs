
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions; 

namespace Common.Repositories
{
    public class GetRepository<T> : IGetRepository<T> where T : class, IBaseEntity
    {
        private readonly DbContext _context;
        public GetRepository(DbContext context)
        {
            _context = context;
        }
        public async Task<T> Get(Guid id) => await _context.Set<T>().AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);

        public async Task<T> Get(Expression<Func<T, bool>> condition) 
            => await _context.Set<T>().AsNoTracking().Where(condition).SingleOrDefaultAsync();
        public async Task<T> Get(Guid id,Expression<Func<T, object>> include)
            => await _context.Set<T>().AsNoTracking().Include(include).SingleOrDefaultAsync(x => x.Id == id);
        public async Task<T> Get(Guid id, Expression<Func<T, object>> include1, Expression<Func<T, object>> include2)
           => await _context.Set<T>().AsNoTracking().Include(include1).Include(include2).SingleOrDefaultAsync(x => x.Id == id);
    }
}
