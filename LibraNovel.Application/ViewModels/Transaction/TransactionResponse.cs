using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Transaction
{
    public class TransactionResponse
    {
        public string TransactionID { get; set; }
        public int CardID { get; set; }
        public string Amount { get; set; }
        public string? TransactionDate { get; set; }
        public string? Description { get; set; }
        public string? BankCode { get; set; }
        public string? BankTranNo { get; set; }
        public string? CardType { get; set; }
        public string? ResponseCode { get; set; }
        public string? TnxRef { get; set; }
        public string? TransactionStatus { get; set; }
    }
}
