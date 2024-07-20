using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Paypal
{
    public class PaymentPreferences
    {
        public bool auto_bill_outstanding { get; set; }
        public Amount setup_fee { get; set; }
        public string setup_fee_failure_action { get; set; }
        public int payment_failure_threshold { get; set; }
    }
}
