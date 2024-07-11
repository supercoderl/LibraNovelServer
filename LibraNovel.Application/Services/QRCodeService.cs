using LibraNovel.Application.Interfaces;
using LibraNovel.Application.ViewModels.QRCode;
using LibraNovel.Application.Wrappers;
using QRCoder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.Services
{
    public class QRCodeService : IQRCodeService
    {
        public QRCodeService()
        {

        }

        public async Task<Response<string>> GenerateQRCode(QRCodeRequestViewModel request)
        {
            await Task.CompletedTask;
            QRCodeGenerator generator = new QRCodeGenerator();
            QRCodeData data = generator.CreateQrCode(string.Concat(request.BaseURL, "/", request.UserCode), QRCodeGenerator.ECCLevel.Q);
            QRCode code = new QRCode(data);
            Bitmap bitmap = code.GetGraphic(60);
            byte[] bitmapArray = BitmapToByteArray(bitmap);
            string qrURL = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(bitmapArray));
            return new Response<string>
            {
                Succeeded = true,
                Data = qrURL,
            };
        }

        private byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}
