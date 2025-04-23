using Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Common.Repositories
{
    public class CreateRepository<TDbContext, T> : ICreateRepository<T>
    where TDbContext : DbContext
    where T : class, IBaseEntity
    {
        private readonly TDbContext _context;

        public CreateRepository(TDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            var result = await _context.SaveChangesAsync();
            if (result > 0) return true;
            return false;
        }

    }
}
