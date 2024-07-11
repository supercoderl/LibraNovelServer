using LibraNovel.Application.ViewModels.QRCode;
using LibraNovel.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.Interfaces
{
    public interface IQRCodeService
    {
        Task<Response<string>> GenerateQRCode(QRCodeRequestViewModel request);
    }
}
