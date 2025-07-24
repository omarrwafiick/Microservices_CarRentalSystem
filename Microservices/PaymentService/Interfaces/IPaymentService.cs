 
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
        Task<ServiceResult<int>> RegisterPaymentRecordsAsync(CreatePaymentDto dto);
        Task<ServiceResult<bool>> UpdatePaymentRecordsAsync(int id, UpdatePaymentStatusDto dto);
    }
}
