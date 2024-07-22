using LibraNovel.Application.ViewModels.Transaction;
using LibraNovel.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<Response<List<TransactionResponse>>> GetTransactions(int cardID);
        Task<Response<string>> StoreTransaction(CreateTransactionViewModel request);
    }
}
