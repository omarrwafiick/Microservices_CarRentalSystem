using Common.Interfaces;
using PaymentService.Models;
using PaymentServiceApi.Interfaces;
using PaymentServiceApi.Models;

namespace PaymentServiceApi.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IGetAllRepository<Payment> _getAllRepository;
        private readonly IGetRepository<Payment> _getRepository;
        private readonly ICreateRepository<Payment> _createRepository;
        private readonly IUpdateRepository<Payment> _updateRepository;
        private readonly IGetRepository<PaymentStatus> _getPaymentStatusRepository;
        public PaymentService(
            IGetAllRepository<Payment> getAllRepository, 
            IGetRepository<Payment> getRepository, 
            ICreateRepository<Payment> createRepository, 
            IUpdateRepository<Payment> updateRepository,
            IGetRepository<PaymentStatus> getPaymentStatusRepository)
        {
            _getAllRepository = getAllRepository;
            _getRepository = getRepository;
            _createRepository = createRepository;
            _updateRepository = updateRepository;
            _getPaymentStatusRepository = getPaymentStatusRepository;
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync() => await _getAllRepository.GetAll();

        public async Task<Payment> GetPaymentByIdAsync(Guid id) => await _getRepository.Get(id);

        public async Task<IEnumerable<Payment>> GetPaymentsByUserAsync(Guid id) => await _getAllRepository.GetAll(x => x.UserId == id);

        public async Task<bool> CreatePaymentAsync(Payment domain)
        {
            var exists = await _getAllRepository.GetAll(x => x.BookingId == domain.BookingId);
            if (exists.Any()) return false;
            var result = await _createRepository.CreateAsync(domain);
            return result;
        }

        public async Task<bool> RefundPaymentAsync(Guid id)
        {
            var status = await _getPaymentStatusRepository.Get(x=>x.Status == "Failed");
            if (status == null) return false;
            var payment = await _getRepository.Get(id);
            if (payment == null) return false;
            payment.PaymentStatusId = status.Id;
            var result = await _updateRepository.UpdateAsync(payment);
            return result;
        }
    }
}
