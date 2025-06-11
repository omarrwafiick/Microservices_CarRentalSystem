using Common.Interfaces;
using System.Text.Json;
using System.Text;
using VehicleServiceApi.Dtos;
using VehicleServiceApi.Interfaces;
using VehicleServiceApi.Models;

namespace VehicleServiceApi.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IGetAllRepository<Vehicle> _getAllRepository;
        private readonly IGetRepository<Vehicle> _getRepository;
        private readonly IGetRepository<Location> _getLocationRepository;
        private readonly ICreateRepository<Vehicle> _createRepository; 
        private readonly IUpdateRepository<Vehicle> _updateRepository;
        private readonly IDeleteRepository<Vehicle> _deleteRepository; 
        private readonly HttpClient _client;

        public VehicleService(
            IGetAllRepository<Vehicle> getAllRepository, 
            IGetRepository<Vehicle> getRepository,
            IGetRepository<Location> getLocationRepository,
            ICreateRepository<Vehicle> createRepository, 
            IUpdateRepository<Vehicle> updateRepository, 
            IDeleteRepository<Vehicle> deleteRepository, 
            HttpClient client
            )
        {
            _getAllRepository = getAllRepository;
            _getRepository = getRepository;
            _getLocationRepository = getLocationRepository;
            _createRepository = createRepository; 
            _updateRepository = updateRepository;
            _deleteRepository = deleteRepository; 
            _client = client;
        }

        public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync() => await _getAllRepository.GetAll();

        public async Task<IEnumerable<Vehicle>> GetAvailableVehiclesAsync() => await _getAllRepository.GetAll(x => x.VehicleStatus == VehicleStatus.Available);

        public async Task<Vehicle> GetVehicleByIdAsync(Guid id) => await _getRepository.Get(id);

        public async Task<bool> CreateVehicleAsync(Vehicle domain)
        {
            var exists = await _getRepository.Get(x => x.LicensePlate == domain.LicensePlate);
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

        public async Task<IEnumerable<Vehicle>> RecommendRelevantVehiclesAsync(RecommendationDto data)
        { 
            PopularityScoreCalculation(data.bookingRecords);
            DailyRentalRateCalculation(); 

            var location = await _getLocationRepository.Get(l => l.City == data.city && l.District == data.district);

            if (location == null)
            {
                 return null;
            }

            var targetVehicles = await _getAllRepository.GetAll(v =>
                                v.CurrentLocationId == location.Id
                                && v.VehicleStatus == VehicleStatus.Available);

            if (!targetVehicles.Any())
            {
                 return null;
            }

            List<(Guid vehicleId, Guid userId)> viewedBookings = new();

            foreach (var vehicle in targetVehicles)
            {
                viewedBookings.Add((vehicle.Id, data.userId));
            }
             
            try
            {
                var json = JsonSerializer.Serialize(viewedBookings);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await _client.PostAsync("https://localhost:7000/api/bookings/view/range", content);
            }
            catch (Exception ex) {
                throw;
            }

            List<Vehicle> recommendedVehicles = new();

            //first time
            if (data.userBookings is null)
            {
                foreach (var vehicle in targetVehicles)
                {
                    if (recommendedVehicles.Any(x => x.Id == vehicle.Id))
                    {
                        continue;
                    }

                    recommendedVehicles.Add(vehicle);
                }
                return recommendedVehicles
                        .OrderBy(x => x.DailyRate)
                        .Take(3)
                        .ToList();
            }

            return recommendedVehicles
                .OrderByDescending(x => x.PopularityScore)
                .Take(10)
                .ToList();
        }

        private async Task PopularityScoreCalculation(List<UserBookingRecords> bookingRecords)
        {
            var vehicles = await _getAllRepository.GetAll();

            foreach (var vehicle in vehicles)
            {
                var vehicleBookings = bookingRecords.Where(x => x.Id == vehicle.Id);

                var bookedCount = vehicleBookings.Where(x => x.InteractionType == InteractionType.BOOKED).Count();

                var viewedCount = vehicleBookings.Where(x => x.InteractionType == InteractionType.VIEWED).Count();
                  
                int score = (bookedCount * 10) + (viewedCount);

                vehicle.PopularityScore = score;
            }
        }

        private async Task DailyRentalRateCalculation()
        {
            var vehicles = await _getAllRepository.GetAll();

            foreach (var vehicle in vehicles)
            {
                decimal adjustedRate = vehicle.DailyRate * (1 + (decimal)vehicle.PopularityScore / 2000);
                 
                vehicle.DailyRate = adjustedRate;
            }
        }
         
    }
}
