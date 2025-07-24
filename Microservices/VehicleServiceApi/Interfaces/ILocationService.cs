

using Common.Dtos;
using System.Linq.Expressions;
using VehicleServiceApi.Dtos;
using VehicleServiceApi.Models;

namespace VehicleServiceApi.Interfaces
{
    public interface ILocationService
    {
        Task<ServiceResult<List<Location>>> GetLocationsAsync(); 
        Task<ServiceResult<Location>> GetLocationAsync(int id); 
        Task<ServiceResult<Location>> GetLocationAsync(int id, Expression<Func<Location, object>> include);
        Task<ServiceResult<int>> AddLocationAsync(CreateLocationDto dto);
        Task<ServiceResult<bool>> UpdateLocationAsync(int id, UpdateLocationDto dto);
        Task<ServiceResult<bool>> ChangeLocationStatusAsync(int id, bool activate);   
    }
}
