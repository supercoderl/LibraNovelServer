using LibraNovel.Application.ViewModels.Payment;
using LibraNovel.Application.ViewModels.Paypal;
using LibraNovel.Application.Wrappers;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.Interfaces
{
    public interface IVnPayService
    {
        Task<Response<string>> Pay(bool bankcode_Vnpayqr, bool bankcode_Vnbank, bool bankcode_Intcard, string locale);
    }

    public interface IPaypalService
    {
        Task<Response<CreateOrderResponse?>> CreateOrder(string value, string currency, string reference);
        Task<Response<CaptureOrderResponse?>> CaptureOrder(string orderId);
        Task<Response<string>> GetTransactions();
    }

    public interface IPayOsService
    {
        Task<Response<CreatePaymentResult>> CreateOrderPayOS(CreatePayOSViewModel request);
        Task<Response<PaymentLinkInformation>> CancelOrder(long orderID);
    }
}
