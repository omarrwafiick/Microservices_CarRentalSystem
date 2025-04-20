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
        private readonly IGetRepository<BookingStatus> _getBookingStatusRepository;
        public BookingService(
            IGetAllRepository<Booking> getAllRepository,
            IGetRepository<Booking> getRepository, 
            ICreateRepository<Booking> createRepository,
            IUpdateRepository<Booking> updateRepository,
            IGetRepository<BookingStatus> getBookingStatusRepository)
        {
            _getAllRepository = getAllRepository;
            _getRepository = getRepository;
            _createRepository = createRepository;
            _updateRepository = updateRepository;
            _getBookingStatusRepository = getBookingStatusRepository;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync() => await _getAllRepository.GetAll();

        public async Task<Booking> GetBookingByIdAsync(Guid id) => await _getRepository.Get(id);
    
        public async Task<IEnumerable<Booking>> GetBookingsByUserAsync(Guid id) => await _getAllRepository.GetAll(x => x.UserId == id);

        public async Task<bool> CreateBookingAsync(Booking domain)
        {
            var exists = await _getAllRepository.GetAll(x => x.VehicleId == domain.VehicleId && x.EndDate == domain.EndDate);
            if(exists.Any()) return false;
            var result = await _createRepository.CreateAsync(domain);
            return result;
        }

        public async Task<bool> CancelBookingAsync(Guid id)
        {
            var status = await _getBookingStatusRepository.Get(x=>x.Status == "Cancelled"); 
            return await HandleStatusAsync(status, id);
        }

        public async Task<bool> CompleteBookingAsync(Guid id)
        {
            var status = await _getBookingStatusRepository.Get(x => x.Status == "Completed");
            return await HandleStatusAsync(status, id);
        }

        private async Task<bool> HandleStatusAsync(BookingStatus status, Guid id)
        {
            if (status is null) return false;
            var booking = await GetBookingByIdAsync(id);
            if (booking is null) return false;
            booking.BookingStatusId = status.Id;
            var result = await _updateRepository.UpdateAsync(booking);
            return result;
        }
    }
}
