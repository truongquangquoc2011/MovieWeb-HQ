using MovieWeb_HQ.Models.Momo;
using MovieWeb_HQ.Models.Order;

namespace MovieWeb_HQ.Interface
{
    public interface IMomoService
    {
        Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfoModel model);
        MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
    }
}
