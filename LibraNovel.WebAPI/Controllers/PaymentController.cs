using LibraNovel.Application.Interfaces;
using LibraNovel.Application.ViewModels.Paypal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraNovel.WebAPI.Controllers
{
    public class PaymentController : BaseApiController
    {
        private readonly IPaypalService _paypalService;
        private readonly IVnPayService _vnPayService;

        public PaymentController(IPaypalService paypalService, IVnPayService vnPayService)
        {
            _paypalService = paypalService;
            _vnPayService = vnPayService;
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
    }
}
