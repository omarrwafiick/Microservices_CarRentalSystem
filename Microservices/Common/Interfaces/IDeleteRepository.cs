

namespace Common.Interfaces
{
    public interface IDeleteRepository<T> where T : class, IBaseEntity
    {
        Task<bool> DeleteAsync<TID>(TID id) ;
    }
}
