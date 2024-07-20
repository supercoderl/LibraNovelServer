using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Paypal
{
    public class AuctionInfo
    {
        public string auction_site { get; set; }
        public string auction_item_site { get; set; }
        public string auction_buyer_id { get; set; }
        public DateTime auction_closing_date { get; set; }
    }
}
