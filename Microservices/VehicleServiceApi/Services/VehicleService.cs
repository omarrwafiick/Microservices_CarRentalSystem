﻿using Common.Dtos; 
using RabbitMQ.Client;
using static VehicleServiceApi.Helpers.EnumHelper;
using System.Linq.Expressions;
using System.Text.Json;
using VehicleServiceApi.Dtos;
using VehicleServiceApi.Enums;
using VehicleServiceApi.Interfaces;
using VehicleServiceApi.Models;
using VehicleServiceApi.Interfaces.UnitOfWork;
using Microsoft.Extensions.Caching.Memory;
using RabbitMQ.Client.Events;

namespace VehicleServiceApi.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleUnitOfWork _vehicleUnitOfWork; 
        private readonly IMemoryCache _cache;
        private readonly ILogger _logger;
        public VehicleService(IVehicleUnitOfWork vehicleUnitOfWork, IMemoryCache cache, ILogger logger)
        {
            _vehicleUnitOfWork = vehicleUnitOfWork; 
            _cache = cache;
            _logger = logger;
        }

        public async Task<ServiceResult<List<Vehicle>>> GetVehiclesAsync(Expression<Func<Vehicle, object>> include1, Expression<Func<Vehicle, object>> include2, Expression<Func<Vehicle, object>> include3)
        {
            var vehicles = await _vehicleUnitOfWork.GetAllVehicleRepository.GetAll(include1, include2, include3);

            return vehicles.Any() ?
                ServiceResult<List<Vehicle>>.Success("Vehicles was found!", vehicles.ToList()) :
                ServiceResult<List<Vehicle>>.Failure("No vehicle was found!");
        }

        public async Task<ServiceResult<List<Vehicle>>> GetVehiclesByFilterAsync(Expression<Func<Vehicle, object>> include1, Expression<Func<Vehicle, object>> include2, Expression<Func<Vehicle, object>> include3,string fuelType, string vehicleType, string transmissionType)
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

            var vehicles = await _vehicleUnitOfWork.GetAllVehicleRepository.GetAll(include1, include2, include3);

            if (!vehicles.Any()) 
            {
                ServiceResult<List<Vehicle>>.Failure("No vehicle was found!");
            }
            
            vehicles = fuelState ? vehicles.Where(vehicle => vehicle.FuelType == Enum.Parse<FuelType>(fuelType)) : vehicles ;
            
            vehicles = vehicleState ? vehicles.Where(vehicle => vehicle.VehicleType == Enum.Parse<VehicleType>(vehicleType)) : vehicles;
     
            vehicles = transmissionState ? vehicles.Where(vehicle => vehicle.Transmission == Enum.Parse<TransmissionType>(transmissionType)) : vehicles;

            return ServiceResult<List<Vehicle>>.Success("Vehicles was found!", vehicles.ToList());
        }

        public async Task<ServiceResult<List<Vehicle>>> GetVehiclesByConditionAsync(Expression<Func<Vehicle, object>> include1, Expression<Func<Vehicle, object>> include2, Expression<Func<Vehicle, object>> include3, Func<Vehicle, bool> condition)
        {
            var vehicles = await _vehicleUnitOfWork.GetAllVehicleRepository.GetAll(include1, include2, include3);

            return vehicles.Any() ?
                ServiceResult<List<Vehicle>>.Success("Vehicles was found!", vehicles.Where(condition).ToList()) :
                ServiceResult<List<Vehicle>>.Failure("No vehicle was found!");
        }

        public async Task<ServiceResult<int>> RegisterVehicleAsync(CreateVehicleDto dto)
        { 
            #region Variables
            var ownerId = dto.OwnerId;
            var currentLocationId = dto.CurrentLocationId;
            var modelId = dto.ModelId;
            #endregion

            var model = await _vehicleUnitOfWork.GetVehicleRepository.Get(modelId);

            if (model is null)
            {
                ServiceResult<int>.Failure("Model was not found");
            }

            var location = await _vehicleUnitOfWork.GetLocationRepository.Get(currentLocationId);

            if (location is null)
            {
                ServiceResult<int>.Failure("Location was not found");
            }

            var now = DateTime.UtcNow;

            if (dto.InsuranceExpiryDate <= now || dto.RegistrationExpiryDate <= now || dto.LastServiceDate > now)
            {
                return ServiceResult<int>.Failure("Invalid date/s and time/s");
            }

            if (!ValidateEnumValue<FuelType>(dto.FuelType)||
               !ValidateEnumValue<VehicleType>(dto.VehicleType)||
               !ValidateEnumValue<TransmissionType>(dto.Transmission)
            )
            {
                return ServiceResult<int>.Failure("Invalid enum type/s");
            }

            var validateUser = await ValidateEntityViaMediator(dto.OwnerId, "validate-user");

            if(!validateUser.SuccessOrNot)
            {
                return ServiceResult<int>.Failure($"Invalid user with id {dto.OwnerId}");
            }

            var vehicleExists = await _vehicleUnitOfWork.GetVehicleRepository.Get(
                vehicle => 
                    vehicle.OwnerId == ownerId &&
                    vehicle.LicensePlate == dto.LicensePlate &&
                    vehicle.ModelId == modelId &&
                    vehicle.Year == dto.Year &&
                    vehicle.VIN == dto.VIN);

            if (vehicleExists is not null)
            {
                ServiceResult<int>.Failure("Vehicle already exists");
            }

            var fuelType = Enum.Parse<FuelType>(dto.FuelType);

            var vehicleType = Enum.Parse<VehicleType>(dto.VehicleType);

            var transmissionType = Enum.Parse<TransmissionType>(dto.Transmission);

            var newVehicle = Vehicle.Factory(ownerId, currentLocationId, dto.LicensePlate, dto.VIN,
                modelId, dto.Year, vehicleType, dto.RegistrationExpiryDate, dto.InsurancePolicyNumber,
                dto.InsuranceExpiryDate, dto.DailyRate, dto.Mileage, fuelType, transmissionType, dto.IsGpsEnabled,
                dto.IsElectric, dto.LastServiceDate, dto.ServiceIntervalKm);

            var result = await _vehicleUnitOfWork.CreateVehicleRepository.CreateAsync(newVehicle);

            if(!result)
            {
                return ServiceResult<int>.Failure("Failed to create a new vehicle"); 
            }
            _cache.Remove(Globals.VEHICLES_CACHEKEY);

            return ServiceResult<int>.Success("Vehicles was created successfully", newVehicle.Id);
        }
         
        public async Task<ServiceResult<bool>> UpdateVehicleStatusAsync(int id, string status)
        {
            if (!ValidateEnumValue<VehicleStatus>(status))
            {
                return ServiceResult<bool>.Failure("Invalid enum type");
            }

            var vehicle = await _vehicleUnitOfWork.GetVehicleRepository.GetWithTracking(id);

            if (vehicle is null)
            {
                return ServiceResult<bool>.Failure("Vehicle was not found");
            }

            var newStatus = Enum.Parse<VehicleStatus>(status);

            vehicle.UpdateStatus(newStatus);

            var result = await _vehicleUnitOfWork.UpdateVehicleRepository.UpdateAsync(vehicle);

            if (!result)
            {
                return ServiceResult<bool>.Failure("Failed to deactivate vehicle");
            }

            _cache.Remove(Globals.VEHICLES_CACHEKEY);

            return ServiceResult<bool>.Success("Vehicle status was deactivated successfully") ;
              
        }

        public async Task<ServiceResult<List<Vehicle>>> RecommendRelevantVehiclesAsync(RecommendationDto data)
        {
            await PopularityScoreCalculation(data.bookingRecords);
            await DailyRentalRateCalculation();

            var location = await _vehicleUnitOfWork.GetLocationRepository.Get(l => l.City == data.city && l.District == data.district);

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

            List<(int vehicleId, int userId)> viewedBookings = new();

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

        private async Task RabbitMqMessageToBookingsService(List<(int vehicleId, int userId)> viewedBookings)
        { 
            _logger.LogInformation($"Start Pushing Message by RabbitMq at : {DateTime.UtcNow}");

            //RabbitMq connection
            //Port : 5672
            var factory = new ConnectionFactory { HostName = "localhost" };

            using var connection = await factory.CreateConnectionAsync();

            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "messages-bookings",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            byte[] body = JsonSerializer.SerializeToUtf8Bytes(viewedBookings);

            await channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: "messages-bookings",
                mandatory: true,
                basicProperties: new BasicProperties { Persistent = true },
                body: body
            );

            _logger.LogInformation($"Message was sent by RabbitMq at : {DateTime.UtcNow}"); 
        }

        private async Task<ServiceResult<bool>> ValidateEntityViaMediator(int Id, string routingKey)
        {
            _logger.LogInformation($"Started to validate entity via mediator with id: {Id} - queue key: {routingKey} - at: {DateTime.UtcNow}");

            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            var replyQueue = await channel.QueueDeclareAsync(queue: routingKey, exclusive: true);
            var replyQueueName = replyQueue.QueueName;

            var correlationId = Guid.NewGuid().ToString();

            var props = new BasicProperties
            {
                CorrelationId = correlationId,
                ReplyTo = replyQueueName,
                Persistent = true
            };

            byte[] messageBody = JsonSerializer.SerializeToUtf8Bytes(Id);

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: routingKey,
                mandatory: true,
                basicProperties: props,
                body: messageBody
            );

            var tcs = new TaskCompletionSource<bool>();

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (sender, ea) =>
            {
                if (ea.BasicProperties?.CorrelationId == correlationId)
                {
                    var response = JsonSerializer.Deserialize<bool>(ea.Body.ToArray());
                    tcs.SetResult(response);
                    await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                }
            };

            await channel.BasicConsumeAsync(
                queue: replyQueueName,
                autoAck: false,
                consumer: consumer
            );

            var isValidUser = await tcs.Task;

            if (!isValidUser)
            {
                _logger.LogError($"Failed to validate entity with id: {Id} - queue key: {routingKey} - at: {DateTime.UtcNow}");
                ServiceResult<bool>.Failure("");
            }
            _logger.LogInformation($"Entity was validated successfully via mediator with id: {Id} - queue key: {routingKey} - at: {DateTime.UtcNow}");

            return ServiceResult<bool>.Success("", isValidUser);
        }

        public async Task<ServiceResult<bool>> ChangeVehicleStatusAsync(int id, bool activate)
        {
            var vehicle = await _vehicleUnitOfWork.GetVehicleRepository.GetWithTracking(id);

            if (vehicle is null)
            {
                return ServiceResult<bool>.Failure("Vehicle was not found");
            }

            if (activate && !vehicle.IsActive)
            { 
                vehicle.Activate();
            }
            else if(!activate && vehicle.IsActive)
            {
                vehicle.Deactivate(); 
            }
            else
            { 
                return ServiceResult<bool>.Failure("Failed to deactivate vehicle");
            }

            var result = await _vehicleUnitOfWork.UpdateVehicleRepository.UpdateAsync(vehicle);

            if (!result)
            {
                return ServiceResult<bool>.Failure("Failed to deactivate vehicle");
            }

            _cache.Remove(Globals.VEHICLES_CACHEKEY);

            return ServiceResult<bool>.Success("Vehicle status was deactivated successfully");
               
        }
         
    }
}
