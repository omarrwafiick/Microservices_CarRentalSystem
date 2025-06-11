using VehicleServiceApi.Dtos;
using VehicleServiceApi.Models;

namespace VehicleServiceApi.Extensions
{
    public static class VehicleExtensions
    {
        public static GetVehicleDto MapFromDomainToDto(this Vehicle domain)
        {
            return new GetVehicleDto(
                domain.Id,
                domain.LicensePlate,
                domain.Make,
                domain.Model,
                domain.Year, 
                domain.VehicleType,
                domain.DailyRate,
                domain.VehicleStatus,
                domain.CurrentLocationId
                );
        }

        public static Vehicle CreateMapFromDtoToDomain(this CreateVehicleDto dto)
        {
            return new Vehicle
            { 
                LicensePlate = dto.LicensePlate,
                Make = dto.Make,
                Model = dto.Model,
                Year = dto.Year,
                VehicleType = dto.VehicleType,
                DailyRate = dto.DailyRate,
                VehicleStatus = dto.VehicleStatus,
                CurrentLocationId = dto.LocationId
            };
        }

        public static Vehicle UpdateMapFromDtoToDomain(this UpdateVehicleDto dto, Vehicle domain)
        {
            domain.CurrentLocationId = dto.LocationId; 
            domain.DailyRate = dto.DailyRate;
            domain.VehicleType = dto.VehicleType;
            domain.VehicleStatus = dto.VehicleStatus;
            return domain;
        }
    }
}
