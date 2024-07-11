using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.QRCode
{
    public class QRCodeRequestViewModel
    {
        public string? UserCode { get; set; }
        public string? BaseURL { get; set; }
    }
}
