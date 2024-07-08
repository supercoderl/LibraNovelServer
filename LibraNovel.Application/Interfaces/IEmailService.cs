using LibraNovel.Application.ViewModels.Email;
using LibraNovel.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.Interfaces
{
    public interface IEmailService
    {
        Task<Response<string>> SendEmail(Message message);
    }
}
