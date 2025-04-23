using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace Common.Repositories
{
    public class UpdateRepository<TDbContext, T> : IUpdateRepository<T>
    where TDbContext : DbContext
    where T : class, IBaseEntity
    {
        private readonly TDbContext _context;

        public UpdateRepository(TDbContext context)
        {
            _context = context;
        }
        public async Task<bool> UpdateAsync(T entity)
        {
            await Task.Run(() => _context.Set<T>().Update(entity));
            var result = await _context.SaveChangesAsync();
            if (result > 0) return true;
            return false;
        }

    }
}
