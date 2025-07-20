using Common.Dtos; 
using System.Linq.Expressions;
using VehicleServiceApi.Dtos;
using VehicleServiceApi.Interfaces;
using VehicleServiceApi.Interfaces.UnitOfWork;
using VehicleServiceApi.Models;

namespace VehicleServiceApi.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationUnitOfWork _locationUnitOfWork;
        public LocationService(IVehicleUnitOfWork vehicleUnitOfWork, ILocationUnitOfWork locationUnitOfWork)
        { 
            _locationUnitOfWork = locationUnitOfWork;
        }

        public async Task<ServiceResult<List<Location>>> GetLocationsAsync()
        {
            var result = await _locationUnitOfWork.GetAllLocationRepository.GetAll();

            return result.Any() ?
                ServiceResult<List<Location>>.Success("Location was found!", result.ToList()) :
                ServiceResult<List<Location>>.Failure("Location was not found");
        } 

        public async Task<ServiceResult<Location>> GetLocationAsync(Guid id)
        {
            var result = await _locationUnitOfWork.GetLocationRepository.Get(id);

            return result is not null ?
                ServiceResult<Location>.Success("Location was found!", result) :
                ServiceResult<Location>.Failure("Location was not found");
        }
         
        public async Task<ServiceResult<Location>> GetLocationAsync(Guid id, Expression<Func<Location, object>> include)
        {
            var result = await _locationUnitOfWork.GetLocationRepository.Get(id, include);

            return result is not null ?
                ServiceResult<Location>.Success("Location was found!", result) :
                ServiceResult<Location>.Failure("Location was not found");
        }

        public async Task<ServiceResult<bool>> AddLocationAsync(CreateLocationDto dto)
        {
            #region variable
            var nameMatches = dto.Name.ToLower();
            var city = dto.City;
            var country = dto.Country;
            var district = dto.District;
            var latitude = dto.Latitude;
            var longitude = dto.Longitude;
            #endregion

            var locationExists = await _locationUnitOfWork.GetLocationRepository.Get(location =>
                location.Name.ToLower() == nameMatches ||
                (
                    location.City == city &&
                    location.Country == country &&
                    location.District == district &&
                    location.Latitude == latitude &&
                    location.Longitude == longitude
                )
            );

            if (locationExists is not null)
            {
                return ServiceResult<bool>.Failure("Location already exists with same data");
            }

            var newLocation = Location.Factory(dto.Name, dto.District, dto.City, dto.Country, dto.Longitude, dto.Latitude);

            var result = await _locationUnitOfWork.CreateLocationRepository.CreateAsync(newLocation);

            return result ?
               ServiceResult<bool>.Success("Location was created successfully") :
               ServiceResult<bool>.Failure("Failed to create a new location");
        }

        public async Task<ServiceResult<bool>> UpdateLocationAsync(Guid id, UpdateLocationDto dto)
        {
            var location = await _locationUnitOfWork.GetLocationRepository.Get(id);

            if (location is null)
            {
                return ServiceResult<bool>.Failure("Location was not found");
            }

            location.UpdateAddress(dto.District, dto.City, dto.Country);

            var result = await _locationUnitOfWork.UpdateLocationRepository.UpdateAsync(location);

            return result ?
                ServiceResult<bool>.Success("Location was updated successfully") :
                ServiceResult<bool>.Failure("Failed to update location");
        }

        public async Task<ServiceResult<bool>> ChangeLocationStatusAsync(Guid id, bool activate)
        {
            var location = await _locationUnitOfWork.GetLocationRepository.Get(id);

            if(location is null)
            {
                return ServiceResult<bool>.Failure("Location was not found");
            }

            if (activate)
            { 
                location.ActivateLocation();
            }
            else
            {
                location.DeactivateLocation();
            }

            var result = await _locationUnitOfWork.UpdateLocationRepository.UpdateAsync(location);

            return result ?
                ServiceResult<bool>.Success("Location status was updated successfully"):
                ServiceResult<bool>.Failure("Failed to update location status");
        }
          
    }
}
