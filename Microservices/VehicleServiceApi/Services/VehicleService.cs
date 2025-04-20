using Common.Interfaces;
using VehicleServiceApi.Interfaces;
using VehicleServiceApi.Models;

namespace VehicleServiceApi.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IGetAllRepository<Vehicle> _getAllRepository;
        private readonly IGetRepository<Vehicle> _getRepository;
        private readonly ICreateRepository<Vehicle> _createRepository;
        private readonly IUpdateRepository<Vehicle> _updateRepository;
        private readonly IDeleteRepository<Vehicle> _deleteRepository;

        public VehicleService(
            IGetAllRepository<Vehicle> getAllRepository, 
            IGetRepository<Vehicle> getRepository, 
            ICreateRepository<Vehicle> createRepository, 
            IUpdateRepository<Vehicle> updateRepository, 
            IDeleteRepository<Vehicle> deleteRepository)
        {
            _getAllRepository = getAllRepository;
            _getRepository = getRepository;
            _createRepository = createRepository;
            _updateRepository = updateRepository;
            _deleteRepository = deleteRepository;
        }

        public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync() => await _getAllRepository.GetAll();

        public async Task<IEnumerable<Vehicle>> GetAvailableVehiclesAsync() => await _getAllRepository.GetAll(x => x.IsAvailable);

        public async Task<Vehicle> GetVehicleByIdAsync(Guid id) => await _getRepository.Get(id);

        public async Task<bool> CreateVehicleAsync(Vehicle domain)
        {
            var exists = await _getRepository.Get(x => x.LicensePlate == x.LicensePlate);
            if(exists is not null) return false;
            var createResult = await _createRepository.CreateAsync(domain);
            return createResult;
        }

        public async Task<bool> UpdateVehicleAsync(Vehicle domain)
        {  
            var updateResult = await _updateRepository.UpdateAsync(domain);
            return updateResult;
        }

        public async Task<bool> DeleteVehicleAsync(Guid id)
        {
            var deleteResult = await _deleteRepository.DeleteAsync(id);
            return deleteResult;
        }
    }
}
