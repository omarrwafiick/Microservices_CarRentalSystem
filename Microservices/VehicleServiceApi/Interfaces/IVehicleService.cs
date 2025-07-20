using Common.Dtos;
using System.Linq.Expressions;
using VehicleServiceApi.Dtos; 
using VehicleServiceApi.Models;

namespace VehicleServiceApi.Interfaces
{
    public interface IVehicleService
    { 
        Task<ServiceResult<List<Vehicle>>> GetVehiclesAsync(); 
        Task<ServiceResult<List<Vehicle>>> GetVehiclesByFilterAsync(string fuelType, string vehicleType, string transmissionType);
        Task<ServiceResult<List<Vehicle>>> GetVehiclesByConditionAsync(Expression<Func<Vehicle, bool>> condition);
        Task<ServiceResult<bool>> RegisterVehicleAsync(CreateVehicleDto dto); 
        Task<ServiceResult<bool>> UpdateVehicleStatusAsync(Guid id, string status);
        Task<ServiceResult<bool>> ChangeVehicleStatusAsync(Guid id, bool activate);
        Task<ServiceResult<List<Vehicle>>> RecommendRelevantVehiclesAsync(RecommendationDto data); 
    }
}
