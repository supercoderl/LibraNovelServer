using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Paypal
{
    public class Root
    {
        public List<TransactionDetail> transaction_details { get; set; }
        public string account_number { get; set; }
        public DateTime last_refreshed_datetime { get; set; }
        public int page { get; set; }
        public int total_items { get; set; }
        public int total_pages { get; set; }
        public List<Link> links { get; set; }
    }
}
