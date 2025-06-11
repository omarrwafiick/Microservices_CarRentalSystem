using Common.Interfaces;
using VehicleServiceApi.Dtos;
using VehicleServiceApi.Interfaces;
using VehicleServiceApi.Models;

namespace VehicleServiceApi.Services
{
    public class LocationService : ILocationService
    { 
        private readonly IGetRepository<Location> _getLocationRepository; 
        private readonly ICreateRepository<Location> _createLocationRepository; 
        private readonly IDeleteRepository<Location> _deleteLocationRepository;

        public LocationService(
            IGetRepository<Location> getLocationRepository,
            ICreateRepository<Location> createLocationRepository,
            IDeleteRepository<Location> deleteLocationRepository
            )
        {
            _getLocationRepository = getLocationRepository;
            _createLocationRepository = createLocationRepository;
            _deleteLocationRepository = deleteLocationRepository;
        }

        public async Task<Location> GetLocationAsync(Guid id)
            => await _getLocationRepository.Get(id);

        public async Task<bool> CreateLocationAsync(LocationDto dto)
        {
            var exists = await _getLocationRepository.Get(l => l.Latitude == dto.Latitude && l.Longitude == dto.Longitude);

            if (exists is not null) return false;

            return await _createLocationRepository.CreateAsync(new Location
            {
                City = dto.City,
                District = dto.District,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude
            }
            );
        }

        public async Task<bool> DeleteLocationAsync(Guid id)
        {
            var exists = await _getLocationRepository.Get(id);

            if (exists is null) return false;

            return await _deleteLocationRepository.DeleteAsync(id);
        }
    }
}
