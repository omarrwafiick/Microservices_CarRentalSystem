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
                domain.Type,
                domain.DailyRate,
                domain.IsAvailable,
                domain.Location
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
                Type = dto.Type,
                DailyRate = dto.DailyRate,
                IsAvailable = dto.IsAvailable,
                Location = dto.Location
            };
        }

        public static Vehicle UpdateMapFromDtoToDomain(this UpdateVehicleDto dto, Vehicle domain)
        {
            domain.Location = dto.Location;
            domain.LicensePlate = dto.LicensePlate;
            domain.DailyRate = dto.DailyRate;
            domain.Type = dto.Type;
            domain.IsAvailable = dto.IsAvailable;
            return domain;
        }
    }
}
