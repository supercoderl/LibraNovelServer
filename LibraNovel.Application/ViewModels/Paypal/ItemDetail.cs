using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Paypal
{
    public class ItemDetail
    {
        public string item_code { get; set; }
        public string item_name { get; set; }
        public string item_quantity { get; set; }
        public Amount item_unit_price { get; set; }
        public Amount item_amount { get; set; }
        public List<Amount> tax_amounts { get; set; }
        public Amount basic_shipping_amount { get; set; }
        public Amount total_item_amount { get; set; }
    }
}
