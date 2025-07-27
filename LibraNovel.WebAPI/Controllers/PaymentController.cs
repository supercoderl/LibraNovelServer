using LibraNovel.Application.Interfaces;
using LibraNovel.Application.ViewModels.Payment;
using LibraNovel.Application.ViewModels.Paypal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraNovel.WebAPI.Controllers
{
    public class PaymentController : BaseApiController
    {
        private readonly IPaypalService _paypalService;
        private readonly IVnPayService _vnPayService;
        private readonly IPayOsService _payOsService;
        private readonly IStripeService _stripeService;

        public PaymentController(IPaypalService paypalService, IVnPayService vnPayService, IPayOsService payOsService, IStripeService stripeService)
        {
            _paypalService = paypalService;
            _vnPayService = vnPayService;
            _payOsService = payOsService;
            _stripeService = stripeService;
        }

/*        [HttpPost("/authorize-paypal")]
        public async Task<IActionResult> GetAccessToken()
        {
            return Ok(await _paypalService.GetAuthorizationRequest());
        }*/

        [HttpPost("/create-order")]
        public async Task<IActionResult> CreateOrder()
        {
            var price = "10.00";
            var currency = "USD";

            // "reference" is the transaction key
            var reference = "INV001";

            return Ok(await _paypalService.CreateOrder(price, currency, reference));
        }

        [HttpGet("/capture-order/{orderID}")]
        public async Task<IActionResult> GetPlanDetails(string orderID)
        {
            return Ok(await _paypalService.CaptureOrder(orderID));
        }

        [HttpGet("/get-transaction")]
        public async Task<IActionResult> GetTransactions()
        {
            return Ok(await _paypalService.GetTransactions());
        }

        [HttpPost("/pay-vnpay")]
        public async Task<IActionResult> PayVnPay()
        {
            return Ok(await _vnPayService.Pay(false, true, false, "vn"));
        }

        [HttpPost("/pay-payos")]
        public async Task<IActionResult> PayPayOS(CreatePayOSViewModel request)
        {
            return Ok(await _payOsService.CreateOrderPayOS(request));
        }

        [HttpPost("/cancel-payos")]
        public async Task<IActionResult> CancelPayOS(long orderID)
        {
            return Ok(await _payOsService.CancelOrder(orderID));
        }

        [HttpPost("/pay-stripe")]
        public async Task<IActionResult> PayStripe(SessionStripe request)
        {
            return Ok(await _stripeService.CreateOrderStripe(request));
        }

        [HttpGet("/retrieve-stripe")]
        public async Task<IActionResult> RetrieveStripe(string id)
        {
            return Ok(await _stripeService.RetrieveSession(id));
        }
    }
}
