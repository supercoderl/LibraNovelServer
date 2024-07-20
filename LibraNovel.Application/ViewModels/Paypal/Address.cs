using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Paypal
{
    public class Address
    {
        public string line1 { get; set; }
        public string city { get; set; }
        public string country_code { get; set; }
        public string postal_code { get; set; }
    }
}
