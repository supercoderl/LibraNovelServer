using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Paypal
{
    public class TransactionDetail
    {
        public TransactionInfo transaction_info { get; set; }
        public PayerInfo payer_info { get; set; }
        public Shipping shipping_info { get; set; }
        public CartInfo cart_info { get; set; }
        public AuctionInfo auction_info { get; set; }
    }
}
