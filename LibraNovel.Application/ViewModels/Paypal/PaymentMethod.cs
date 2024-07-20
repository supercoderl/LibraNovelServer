using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Paypal
{
    public class PaymentMethod
    {
        public string payer_selected { get; set; }
        public string payee_preferred { get; set; }
    }
}
