using PaymentService.Models;

namespace PaymentServiceApi.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<Payment>> GetAllPaymentsAsync();
        Task<Payment> GetPaymentByIdAsync(Guid id);
        Task<bool> CreatePaymentAsync(Payment domain);
        Task<IEnumerable<Payment>> GetPaymentsByUserAsync(Guid id); 
        Task<bool> RefundPaymentAsync(Guid id);
    }
}
