using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Paypal
{
    public class CreatePlanRequest
    {
        public string product_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public List<BillingCycle> billing_cycles { get; set; }
        public PaymentPreferences payment_preferences { get; set; }
        public Taxes taxes { get; set; }
    }

    public class CreateSubscriptionRequest
    {
        public string plan_id { get; set; }
        public string quantity { get; set; }
        public Amount shipping_amount { get; set; }
        public Subscriber subscriber { get; set; }
        public ApplicationContext application_context { get; set; }
    }

    public class RefundRequest
    {
        public Amount amount { get; set; }
        public string invoice_number { get; set; }
    }

    public class Amount
    {
        public string currency_code { get; set; }
        public string value { get; set; }
    }

    public class SubscriptionStatusChangeRequest
    {
        public string reason { get; set; }
    }

    public sealed class CreateOrderRequest
    {
        public string intent { get; set; }
        public List<PurchaseUnit> purchase_units { get; set; } = new();
    }
}
