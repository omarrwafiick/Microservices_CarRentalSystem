 
using Common.Dtos;
using PaymentService.Models;
using PaymentServiceApi.Dtos;
using System.Linq.Expressions;

namespace PaymentServiceApi.Interfaces
{
    public interface IPaymentService
    {
        Task<ServiceResult<List<PaymentRecord>>> GetPaymentRecordsAsync(HttpContext context);
        Task<ServiceResult<List<PaymentRecord>>> GetPaymentRecordsByConditionAsync(HttpContext context, Expression<Func<PaymentRecord, bool>> condition);
        Task<List<PaymentSummaryDto>> GetPaymentSummary(HttpContext context);
        Task<ServiceResult<int>> RegisterPaymentRecordsAsync(HttpContext context, CreatePaymentDto dto);
        Task<ServiceResult<bool>> UpdatePaymentRecordsAsync(HttpContext context,int id, UpdatePaymentStatusDto dto);
    }
}
