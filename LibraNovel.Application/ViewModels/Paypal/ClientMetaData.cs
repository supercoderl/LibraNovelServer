using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Paypal
{
    public class ClientMetaData
    {
        public string name { get; set; }
        public string display_name { get; set; }
        public string logo_uri { get; set; }
        public List<string> scopes { get; set; }
        public string ui_type { get; set; }
    }
}
