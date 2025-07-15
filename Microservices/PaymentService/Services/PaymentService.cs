using Common.Interfaces;
using PaymentService.Models;
using PaymentServiceApi.Interfaces;   

namespace PaymentServiceApi.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IGetAllRepository<PaymentsRecord> _getAllRepository;
        private readonly IGetRepository<PaymentsRecord> _getRepository;
        private readonly ICreateRepository<PaymentsRecord> _createRepository;
        private readonly IUpdateRepository<PaymentsRecord> _updateRepository; 
        public PaymentService(
            IGetAllRepository<PaymentsRecord> getAllRepository, 
            IGetRepository<PaymentsRecord> getRepository, 
            ICreateRepository<PaymentsRecord> createRepository, 
            IUpdateRepository<PaymentsRecord> updateRepository)
        {
            _getAllRepository = getAllRepository;
            _getRepository = getRepository;
            _createRepository = createRepository;
            _updateRepository = updateRepository; 
        }
         
    }
}
