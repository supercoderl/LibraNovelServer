using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Paypal
{
    public class ApplicationContext
    {
        public string brand_name { get; set; }
        public string locale { get; set; }
        public string shipping_preference { get; set; }
        public string user_action { get; set; }
        public PaymentMethod payment_method { get; set; }
        public string return_url { get; set; }
        public string cancel_url { get; set; }
    }
}
