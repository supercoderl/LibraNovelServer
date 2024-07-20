using LibraNovel.Application.Interfaces;
using LibraNovel.Application.Libraries;
using LibraNovel.Application.ViewModels.VnPay;
using LibraNovel.Application.Wrappers;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Asn1.X9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.Services
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;

        public VnPayService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Response<string>> Pay(bool bankcode_Vnpayqr, bool bankcode_Vnbank, bool bankcode_Intcard, string locale)
        {
            await Task.CompletedTask;

            //Get Config Info
            string? vnp_Returnurl = _configuration["VnPayConfiguration:ReturnURL"]; //URL nhan ket qua tra ve 
            string? vnp_Url = _configuration["VnPayConfiguration:BaseURL"]; //URL thanh toan cua VNPAY 
            string? vnp_TmnCode = _configuration["VnPayConfiguration:TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string? vnp_HashSecret = _configuration["VnPayConfiguration:HashSecret"]; //Secret Key

            //Get payment input
            OrderInfo order = new OrderInfo();
            order.OrderId = DateTime.Now.Ticks; // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
            order.Amount = 10000; // Giả lập số tiền thanh toán hệ thống merchant gửi sang VNPAY 100,000 VND
            order.Status = "0"; //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending" khởi tạo giao dịch chưa có IPN
            order.CreatedDate = DateTime.Now;
            //Save order to db

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            if (bankcode_Vnpayqr == true)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            }
            else if (bankcode_Vnbank == true)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            }
            else if (bankcode_Intcard == true)
            {
                vnpay.AddRequestData("vnp_BankCode", "INTCARD");
            }

            vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", "13.160.92.202");

            vnpay.AddRequestData("vnp_Locale", locale);

            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + order.OrderId);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //Billing

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            return new Response<string>
            {
                Succeeded = true,
                Data = paymentUrl
            };
        }
    }
}
