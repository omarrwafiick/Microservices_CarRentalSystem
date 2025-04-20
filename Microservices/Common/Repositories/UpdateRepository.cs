
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace Common.Repositories
{
    public class UpdateRepository<T> : IUpdateRepository<T> where T : class
    {
        private readonly DbContext _dbContext;
        public UpdateRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> UpdateAsync(T entity)
        {
            await Task.Run(() => _dbContext.Set<T>().Update(entity));
            var result = await _dbContext.SaveChangesAsync();
            if (result > 0) return true;
            return false;
        }

    }
}
