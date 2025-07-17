 
using Common.Dtos;
using PaymentService.Models;
using PaymentServiceApi.Dtos;
using System.Linq.Expressions;

namespace PaymentServiceApi.Interfaces
{
    public interface IPaymentService
    {
        Task<ServiceResult<List<PaymentRecord>>> GetPaymentRecordsAsync();
        Task<ServiceResult<List<PaymentRecord>>> GetPaymentRecordsByConditionAsync(Expression<Func<PaymentRecord, bool>> condition);
        Task<List<PaymentSummaryDto>> GetPaymentSummary();
        Task<ServiceResult<bool>> RegisterPaymentRecordsAsync(CreatePaymentDto dto);
        Task<ServiceResult<bool>> UpdatePaymentRecordsAsync(Guid id, UpdatePaymentStatusDto dto);
    }
}
