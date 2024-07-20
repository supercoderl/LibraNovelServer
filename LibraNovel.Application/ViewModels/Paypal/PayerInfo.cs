using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Paypal
{
    public class PayerInfo
    {
        public string account_id { get; set; }
        public string email_address { get; set; }
        public string address_status { get; set; }
        public string payer_status { get; set; }
        public PayerName payer_name { get; set; }
        public string country_code { get; set; }
    }
}
