using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Paypal
{
    public class TransactionInfo
    {
        public string paypal_account_id { get; set; }
        public string transaction_id { get; set; }
        public string transaction_event_code { get; set; }
        public DateTime transaction_initiation_date { get; set; }
        public DateTime transaction_updated_date { get; set; }
        public Amount transaction_amount { get; set; }
        public Amount fee_amount { get; set; }
        public string transaction_status { get; set; }
        public string protection_eligibility { get; set; }
    }
}
