using VehicleServiceApi.Dtos;
using VehicleServiceApi.Models;

namespace VehicleServiceApi.Interfaces
{
    public interface ILocationService
    {
        Task<Location> GetLocationAsync(Guid id);
        Task<bool> CreateLocationAsync(LocationDto dto);
        Task<bool> DeleteLocationAsync(Guid id);
    }
}
