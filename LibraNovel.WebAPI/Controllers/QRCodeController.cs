using Asp.Versioning;
using LibraNovel.Application.Interfaces;
using LibraNovel.Application.ViewModels.QRCode;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraNovel.WebAPI.Controllers
{
    public class QRCodeController : BaseApiController
    {
        private readonly IQRCodeService _qRCodeService;
        private readonly IConfiguration _configuration;

        public QRCodeController(IQRCodeService qRCodeService, IConfiguration configuration)
        {
            _qRCodeService = qRCodeService;
            _configuration = configuration;
        }

        [HttpPost("/generate-qrcode")]
        public async Task<IActionResult> GenerateQRCode(QRCodeRequestViewModel request)
        {
            request.BaseURL = string.Concat(_configuration["JWT_Configuration:Audience"], "/information");
            return Ok(await _qRCodeService.GenerateQRCode(request));    
        }
    }
}
