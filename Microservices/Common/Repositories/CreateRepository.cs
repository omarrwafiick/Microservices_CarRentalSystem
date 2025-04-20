using Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Common.Repositories
{
    public class CreateRepository<T> : ICreateRepository<T> where T : class 
    {
        private readonly DbContext _dbContext;
        public CreateRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        } 

        public async Task<bool> CreateAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            var result = await _dbContext.SaveChangesAsync();
            if (result > 0) return true;
            return false;
        }

    }
}
