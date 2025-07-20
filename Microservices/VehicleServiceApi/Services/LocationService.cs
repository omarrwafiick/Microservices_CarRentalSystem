using VehicleServiceApi.Interfaces;
using VehicleServiceApi.Interfaces.UnitOfWork; 

namespace VehicleServiceApi.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationUnitOfWork _locationUnitOfWork;
        public LocationService(IVehicleUnitOfWork vehicleUnitOfWork, ILocationUnitOfWork locationUnitOfWork)
        { 
            _locationUnitOfWork = locationUnitOfWork;
        }

    }
}
