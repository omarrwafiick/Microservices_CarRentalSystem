using Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Common.Repositories
{
    public class DeleteRepository<TDbContext, T> : IDeleteRepository<T>
    where TDbContext : DbContext
    where T : class, IBaseEntity
    {
        private readonly TDbContext _context;

        public DeleteRepository(TDbContext context)
        {
            _context = context;
        }
        public async Task<bool> DeleteAsync<TID>(TID id) 
        {
            var entity = await _context.Set<T>().SingleOrDefaultAsync(e => EF.Property<TID>(e, "Id").Equals(e));
            await Task.Run(() => _context.Set<T>().Remove(entity));
            var result = await _context.SaveChangesAsync();
            if (result > 0) return true;
            return false;
        }
    }
}
