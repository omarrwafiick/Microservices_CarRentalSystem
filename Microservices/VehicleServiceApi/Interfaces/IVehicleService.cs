using Common.Dtos;
using System.Linq.Expressions;
using VehicleServiceApi.Dtos; 
using VehicleServiceApi.Models;

namespace VehicleServiceApi.Interfaces
{
    public interface IVehicleService
    { 
        Task<ServiceResult<List<Vehicle>>> GetVehiclesAsync(Expression<Func<Vehicle, object>> include1, Expression<Func<Vehicle, object>> include2, Expression<Func<Vehicle, object>> include3); 
        Task<ServiceResult<List<Vehicle>>> GetVehiclesByFilterAsync(Expression<Func<Vehicle, object>> include1, Expression<Func<Vehicle, object>> include2, Expression<Func<Vehicle, object>> include3, string fuelType, string vehicleType, string transmissionType);
        Task<ServiceResult<List<Vehicle>>> GetVehiclesByConditionAsync(Expression<Func<Vehicle, object>> include1, Expression<Func<Vehicle, object>> include2, Expression<Func<Vehicle, object>> include3, Func<Vehicle, bool> condition);
        Task<ServiceResult<int>> RegisterVehicleAsync(CreateVehicleDto dto); 
        Task<ServiceResult<bool>> UpdateVehicleStatusAsync(int id, string status);
        Task<ServiceResult<bool>> ChangeVehicleStatusAsync(int id, bool activate);
        Task<ServiceResult<List<Vehicle>>> RecommendRelevantVehiclesAsync(RecommendationDto data); 
    }
}
