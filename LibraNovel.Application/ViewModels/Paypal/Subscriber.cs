using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Paypal
{
    public class Subscriber
    {
        public Name name { get; set; }
        public string email_address { get; set; }
        public ShippingAddress shipping_address { get; set; }
    }
}
