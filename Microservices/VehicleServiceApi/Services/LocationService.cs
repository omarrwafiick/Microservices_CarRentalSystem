using Common.Interfaces; 
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
  
    }
}
