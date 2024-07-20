using LibraNovel.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.Interfaces
{
    public interface IVnPayService
    {
        Task<Response<string>> Pay(bool bankcode_Vnpayqr, bool bankcode_Vnbank, bool bankcode_Intcard, string locale);
    }
}
