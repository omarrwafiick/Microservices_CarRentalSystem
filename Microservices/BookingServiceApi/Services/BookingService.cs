using BookingServiceApi.Dtos;
using BookingServiceApi.Interfaces;
using BookingServiceApi.Models;
using Common.Interfaces;

namespace BookingServiceApi.Services
{
    public class BookingService : IBookingService
    {
        private readonly IGetAllRepository<Booking> _getAllRepository;
        private readonly IGetRepository<Booking> _getRepository;
        private readonly ICreateRepository<Booking> _createRepository;
        private readonly IUpdateRepository<Booking> _updateRepository; 
        public BookingService(
            IGetAllRepository<Booking> getAllRepository,
            IGetRepository<Booking> getRepository, 
            ICreateRepository<Booking> createRepository,
            IUpdateRepository<Booking> updateRepository)
        {
            _getAllRepository = getAllRepository;
            _getRepository = getRepository;
            _createRepository = createRepository;
            _updateRepository = updateRepository; 
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync() => await _getAllRepository.GetAll();

        public async Task<Booking> GetBookingByIdAsync(Guid id) => await _getRepository.Get(id);
    
        public async Task<IEnumerable<Booking>> GetBookingsByUserAsync(Guid id) => await _getAllRepository.GetAll(x => x.RenterId == id);

        public async Task<bool> CreateBookingAsync(Booking domain)
        {
            var exists = await _getAllRepository.GetAll(x => x.VehicleId == domain.VehicleId && x.EndDate == domain.EndDate);
            if(exists.Any()) return false;
            var result = await _createRepository.CreateAsync(domain);
            return result;
        }

        public async Task<bool> CancelBookingAsync(Guid userId, Guid vehicleId)
        { 
            var result = await _createRepository.CreateAsync(new Booking { VehicleId = vehicleId, RenterId = userId, InteractionType = InteractionType.CANCELLED });
            return result;
        }
        public async Task<bool> CompleteBookingAsync(CompleteBookingDto dto)
        {
            var result = await _createRepository.CreateAsync(
                new Booking {   VehicleId = dto.VehicleId, RenterId = dto.UserId, InteractionType = InteractionType.BOOKED,
                                StartDate = dto.StartDate, EndDate = dto.EndDate });
            return result;
        } 
          
        public async Task<bool> DislikeBookingAsync(Guid userId, Guid vehicleId)
        {
            var result = await _createRepository.CreateAsync(new Booking { VehicleId = vehicleId, RenterId = userId, InteractionType = InteractionType.DISLIKED });
            return result;
        }

        public async Task<bool> RecordViewBookingsAsync(List<(Guid vehicleId, Guid userId)> viewedBookings)
        {
            foreach (var booking in viewedBookings)
            {
                var result = await _createRepository.CreateAsync(new Booking { VehicleId = booking.vehicleId, RenterId = booking.userId, InteractionType = InteractionType.VIEWED});
                if(!result) return false;
            }
            return true;
        }
    }
}
