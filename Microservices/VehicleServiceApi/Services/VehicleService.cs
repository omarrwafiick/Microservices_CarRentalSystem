using Common.Dtos; 
using RabbitMQ.Client;
using static VehicleServiceApi.Helpers.EnumHelper;
using System.Linq.Expressions;
using System.Text.Json;
using VehicleServiceApi.Dtos;
using VehicleServiceApi.Enums;
using VehicleServiceApi.Interfaces;
using VehicleServiceApi.Models;
using VehicleServiceApi.Interfaces.UnitOfWork;

namespace VehicleServiceApi.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleUnitOfWork _vehicleUnitOfWork;
        private readonly ILocationUnitOfWork _locationUnitOfWork;
        public VehicleService(IVehicleUnitOfWork vehicleUnitOfWork, ILocationUnitOfWork locationUnitOfWork)
        {
            _vehicleUnitOfWork = vehicleUnitOfWork;
            _locationUnitOfWork = locationUnitOfWork;
        }

        public async Task<ServiceResult<List<Vehicle>>> GetVehiclesAsync()
        {
            var vehicles = await _vehicleUnitOfWork.GetAllVehicleRepository.GetAll();

            return vehicles.Any() ?
                ServiceResult<List<Vehicle>>.Success("Vehicles was found!", vehicles.ToList()) :
                ServiceResult<List<Vehicle>>.Failure("No vehicle was found!");
        }

        public async Task<ServiceResult<List<Vehicle>>> GetVehiclesByFilterAsync(string fuelType, string vehicleType, string transmissionType)
        {
            bool fuelState = fuelType.Length > 0;
            bool vehicleState = vehicleType.Length > 0;
            bool transmissionState = transmissionType.Length > 0;

            if ((fuelState && !ValidateEnumValue<FuelType>(fuelType)) ||
                (vehicleState && !ValidateEnumValue<VehicleType>(vehicleType)) ||
                (transmissionState && !ValidateEnumValue<TransmissionType>(transmissionType))
            )
            {
                return ServiceResult<List<Vehicle>>.Failure("Invalid enum type/s");
            } 

            var vehicles = await _vehicleUnitOfWork.GetAllVehicleRepository.GetAll();

            if (!vehicles.Any()) 
            {
                ServiceResult<List<Vehicle>>.Failure("No vehicle was found!");
            }
            
            vehicles = fuelState ? vehicles.Where(vehicle => vehicle.FuelType == Enum.Parse<FuelType>(fuelType)) : vehicles ;
            
            vehicles = vehicleState ? vehicles.Where(vehicle => vehicle.VehicleType == Enum.Parse<VehicleType>(vehicleType)) : vehicles;
     
            vehicles = transmissionState ? vehicles.Where(vehicle => vehicle.Transmission == Enum.Parse<TransmissionType>(transmissionType)) : vehicles;

            return ServiceResult<List<Vehicle>>.Success("Vehicles was found!", vehicles.ToList());
        }

        public async Task<ServiceResult<List<Vehicle>>> GetVehiclesByConditionAsync(Expression<Func<Vehicle, bool>> condition)
        {
            var vehicles = await _vehicleUnitOfWork.GetAllVehicleRepository.GetAll(condition);

            return vehicles.Any() ?
                ServiceResult<List<Vehicle>>.Success("Vehicles was found!", vehicles.ToList()) :
                ServiceResult<List<Vehicle>>.Failure("No vehicle was found!");
        }

        public async Task<ServiceResult<bool>> RegisterVehicleAsync(CreateVehicleDto dto)
        {
            //TODO : CHECK OWNER BY RABBITMQ
            if(Guid.TryParse(dto.OwnerId, out Guid ownerId))
            {
                ServiceResult<bool>.Failure("Invalid owner id");
            }

            Guid.TryParse(dto.CurrentLocationId, out Guid currentLocationId);

            var location = await _locationUnitOfWork.GetLocationRepository.Get(currentLocationId);

            if (location is null)
            {
                ServiceResult<bool>.Failure("Invalid location");
            }

            var now = DateTime.UtcNow;

            if (dto.InsuranceExpiryDate <= now || dto.RegistrationExpiryDate <= now || dto.LastServiceDate > now)
            {
                ServiceResult<List<Vehicle>>.Failure("Invalid date/s and time/s");
            }

            if (!ValidateEnumValue<FuelType>(dto.FuelType)||
               !ValidateEnumValue<VehicleType>(dto.VehicleType)||
               !ValidateEnumValue<TransmissionType>(dto.Transmission)
            )
            {
                return ServiceResult<bool>.Failure("Invalid enum type/s");
            }

            var vehicleExists = await _vehicleUnitOfWork.GetVehicleRepository.Get(
                vehicle => 
                    vehicle.OwnerId == ownerId &&
                    vehicle.LicensePlate == dto.LicensePlate &&
                    vehicle.Make == dto.Make &&
                    vehicle.Year == dto.Year &&
                    vehicle.VIN == dto.VIN);

            if (vehicleExists is not null)
            {
                ServiceResult<List<Vehicle>>.Failure("Vehicle already exists");
            }

            var fuelType = Enum.Parse<FuelType>(dto.FuelType);

            var vehicleType = Enum.Parse<VehicleType>(dto.VehicleType);

            var transmissionType = Enum.Parse<TransmissionType>(dto.Transmission);

            var newVehicle = Vehicle.Factory(ownerId, currentLocationId, dto.LicensePlate, dto.VIN,
                dto.Make, dto.Model, dto.Year, vehicleType, dto.RegistrationExpiryDate, dto.InsurancePolicyNumber,
                dto.InsuranceExpiryDate, dto.DailyRate, dto.Mileage, fuelType, transmissionType, dto.IsGpsEnabled,
                dto.IsElectric, dto.LastServiceDate, dto.ServiceIntervalKm);

            var result = await _vehicleUnitOfWork.CreateVehicleRepository.CreateAsync(newVehicle);

            return result ?
               ServiceResult<bool>.Success("Vehicles was created successfully") :
               ServiceResult<bool>.Failure("Failed to create a new vehicle");
        }
         
        public async Task<ServiceResult<bool>> UpdateVehicleStatusAsync(Guid id, string status)
        {
            if (!ValidateEnumValue<VehicleStatus>(status))
            {
                return ServiceResult<bool>.Failure("Invalid enum type");
            }

            var vehicle = await _vehicleUnitOfWork.GetVehicleRepository.Get(id);

            if (vehicle is null)
            {
                return ServiceResult<bool>.Failure("Vehicle was not found");
            }

            var newStatus = Enum.Parse<VehicleStatus>(status);

            vehicle.UpdateStatus(newStatus);

            var result = await _vehicleUnitOfWork.UpdateVehicleRepository.UpdateAsync(vehicle);

            return result ?
               ServiceResult<bool>.Success("Vehicle status was deactivated successfully") :
               ServiceResult<bool>.Failure("Failed to deactivate vehicle");
        }

        public async Task<ServiceResult<List<Vehicle>>> RecommendRelevantVehiclesAsync(RecommendationDto data)
        {
            await PopularityScoreCalculation(data.bookingRecords);
            await DailyRentalRateCalculation();

            var location = await _locationUnitOfWork.GetLocationRepository.Get(l => l.City == data.city && l.District == data.district);

            if (location == null)
            {
                return ServiceResult<List<Vehicle>>.Failure("Location was not found");
            }
              
            var targetVehicles = await _vehicleUnitOfWork.GetAllVehicleRepository.GetAll(vehicle =>
                                vehicle.CurrentLocationId == location.Id
                                && vehicle.VehicleStatus == VehicleStatus.Available);

            if (!targetVehicles.Any())
            {
                return ServiceResult<List<Vehicle>>.Failure("Vehicle was not found");
            }

            List<(Guid vehicleId, Guid userId)> viewedBookings = new();

            foreach (var vehicle in targetVehicles)
            {
                viewedBookings.Add((vehicle.Id, data.userId));
            }

            await RabbitMqMessageToBookingsService(viewedBookings);

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
                recommendedVehicles = recommendedVehicles
                                        .OrderBy(x => x.DailyRate)
                                        .Take(3)
                                        .ToList();

                return ServiceResult<List<Vehicle>>.Success("Recommended vehicles was found!", recommendedVehicles);
            }

            recommendedVehicles = recommendedVehicles
                .OrderByDescending(x => x.PopularityScore)
                .Take(10)
                .ToList();

            return ServiceResult<List<Vehicle>>.Success("Recommended vehicles was found!", recommendedVehicles);
        }

        private async Task PopularityScoreCalculation(List<UserBookingRecordDto> bookingRecords)
        {
            var vehicles = await _vehicleUnitOfWork.GetAllVehicleRepository.GetAll();

            foreach (var vehicle in vehicles)
            {
                var vehicleBookings = bookingRecords.Where(x => x.Id == vehicle.Id);

                var bookedCount = vehicleBookings.Where(x => x.InteractionType == InteractionType.BOOKED).Count();

                var viewedCount = vehicleBookings.Where(x => x.InteractionType == InteractionType.VIEWED).Count();

                int score = (bookedCount * 10) + (viewedCount);

                vehicle.UpdatePopularityScore(score);
            }
        }

        private async Task DailyRentalRateCalculation()
        {
            var vehicles = await _vehicleUnitOfWork.GetAllVehicleRepository.GetAll();

            foreach (var vehicle in vehicles)
            {
                decimal adjustedRate = vehicle.DailyRate * (1 + (decimal)vehicle.PopularityScore / 2000);

                vehicle.UpdateDailyRate(adjustedRate);
            }
        }

        private async Task RabbitMqMessageToBookingsService(List<(Guid vehicleId, Guid userId)> viewedBookings)
        {
            Console.WriteLine($"Start Pushing Message by RabbitMq at : {DateTime.UtcNow}");

            //RabbitMq connection
            //Port : 5672
            var factory = new ConnectionFactory { HostName = "localhost" };

            using var connection = await factory.CreateConnectionAsync();

            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "messages",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            byte[] body = JsonSerializer.SerializeToUtf8Bytes(viewedBookings);

            await channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: "messages",
                mandatory: true,
                basicProperties: new BasicProperties { Persistent = true },
                body: body
            );

            Console.WriteLine($"Message was sent by RabbitMq at : {DateTime.UtcNow}");
        }

        public async Task<ServiceResult<bool>> ChangeVehicleStatusAsync(Guid id, bool activate)
        {
            var vehicle = await _vehicleUnitOfWork.GetVehicleRepository.Get(id);

            if (vehicle is null)
            {
                return ServiceResult<bool>.Failure("Vehicle was not found");
            }

            if (activate)
            { 
                vehicle.Activate();
            }
            else
            {
                vehicle.Deactivate();

            }

            var result = await _vehicleUnitOfWork.UpdateVehicleRepository.UpdateAsync(vehicle);

            return result ?
               ServiceResult<bool>.Success("Vehicle status was deactivated successfully") :
               ServiceResult<bool>.Failure("Failed to deactivate vehicle");
        }
         

    }
}
