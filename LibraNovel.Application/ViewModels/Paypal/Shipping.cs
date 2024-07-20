using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Paypal
{
    public class Shipping
    {
        public Address address { get; set; }
        public string name { get; set; }
        public string method { get; set; }
    }
}
