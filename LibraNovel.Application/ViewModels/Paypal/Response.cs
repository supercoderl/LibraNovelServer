using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Paypal
{
    public class AuthorizationResponseData
    {
        public string scope { get; set; }
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string app_id { get; set; }
        public int expires_in { get; set; }
        public List<string> supported_authn_schemes { get; set; }
        public string nonce { get; set; }
        public ClientMetaData client_metadata { get; set; }
    }

    public class CreatePlanResponse
    {
        public string id { get; set; }
        public string product_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public List<BillingCycle> billing_cycles { get; set; }
        public PaymentPreferences payment_preferences { get; set; }
        public Taxes taxes { get; set; }
        public DateTime create_time { get; set; }
        public DateTime update_time { get; set; }
        public List<Link> links { get; set; }
    }

    public class CreateSubscriptionResponse
    {
        public string id { get; set; }
        public string status { get; set; }
        public DateTime status_update_time { get; set; }
        public string plan_id { get; set; }
        public bool plan_overridden { get; set; }
        public DateTime start_time { get; set; }
        public string quantity { get; set; }
        public Amount shipping_amount { get; set; }
        public Subscriber subscriber { get; set; }
        public DateTime create_time { get; set; }
        public List<Link> links { get; set; }
    }

    public class WebhookVerificationResponse
    {
        public string verification_status { get; set; }
    }

    public sealed class CreateOrderResponse
    {
        public string id { get; set; }
        public string status { get; set; }
        public List<Link> links { get; set; }
    }

    public sealed class CaptureOrderResponse
    {
        public string id { get; set; }
        public string status { get; set; }
        public PaymentSource payment_source { get; set; }
        public List<PurchaseUnit> purchase_units { get; set; }
        public Payer payer { get; set; }
        public List<Link> links { get; set; }
    }
}
