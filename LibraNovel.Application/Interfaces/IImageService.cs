using LibraNovel.Application.Wrappers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.Interfaces
{
    public interface IImageService
    {
        Task<Response<string>> UploadImage(IFormFile file);
    }
}
