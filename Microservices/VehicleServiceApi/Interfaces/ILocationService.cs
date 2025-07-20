

using Common.Dtos;
using System.Linq.Expressions;
using VehicleServiceApi.Dtos;
using VehicleServiceApi.Models;

namespace VehicleServiceApi.Interfaces
{
    public interface ILocationService
    {
        Task<ServiceResult<List<Location>>> GetLocationsAsync(); 
        Task<ServiceResult<Location>> GetLocationAsync(Guid id); 
        Task<ServiceResult<Location>> GetLocationAsync(Guid id, Expression<Func<Location, object>> include);
        Task<ServiceResult<bool>> AddLocationAsync(CreateLocationDto dto);
        Task<ServiceResult<bool>> UpdateLocationAsync(Guid id, UpdateLocationDto dto);
        Task<ServiceResult<bool>> ChangeLocationStatusAsync(Guid id, bool activate);   
    }
}
