using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Common.Repositories
{
    public class GetAllRepository<TDbContext, T> : IGetAllRepository<T>
    where TDbContext : DbContext
    where T : class, IBaseEntity
    {
        private readonly TDbContext _context;

        public GetAllRepository(TDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<T>> GetAll() => await _context.Set<T>().AsNoTracking().ToListAsync();
        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> condition)
            => await _context.Set<T>().AsNoTracking().Where(condition).ToListAsync();
        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, object>> include)
            => await _context.Set<T>().AsNoTracking().Include(include).ToListAsync();
        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, object>> include1, Expression<Func<T, object>> include2)
            => await _context.Set<T>().AsNoTracking().Include(include1).Include(include2).ToListAsync();

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, object>> include1, Expression<Func<T, object>> include2, Expression<Func<T, object>> include3)
            => await _context.Set<T>().AsNoTracking().Include(include1).Include(include2).Include(include3).ToListAsync();

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> condition, Expression<Func<T, object>> include)
            => await _context.Set<T>().AsNoTracking().Include(include).Where(condition).ToListAsync();
    }
}
