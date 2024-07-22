using LibraNovel.Application.Interfaces;
using LibraNovel.Application.ViewModels.Transaction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraNovel.WebAPI.Controllers
{
    public class TransactionController : BaseApiController
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("/get-transactions")]
        public async Task<IActionResult> GetTransactions(int cardID)
        {
            return Ok(await _transactionService.GetTransactions(cardID));   
        }

        [HttpPost("/store-transaction-vnpay")]
        public async Task<IActionResult> StoreTransaction(CreateTransactionViewModel request)
        {
            return Ok(await _transactionService.StoreTransaction(request)); 
        }
    }
}
