 
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
         
    }
}
