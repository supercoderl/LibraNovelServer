using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using LibraNovel.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _web;
        private readonly IConfiguration _configuration;
        private Cloudinary _cloudinary;

        public ImageService(IWebHostEnvironment web, IConfiguration configuration)
        {
            _web = web;
            _configuration = configuration;
            _cloudinary = new Cloudinary(new
                Account(
                    configuration["CloudinaryConfiguration:CloudName"],
                    configuration["CloudinaryConfiguration:ApiKey"],
                    configuration["CloudinaryConfiguration:ApiSecret"]
                ));
        }

        //Upload image
        public async Task<Wrappers.Response<string>> UploadImage(IFormFile file)
        {
            return new Wrappers.Response<string>
            {
                Data = await UploadFile(file),
                Succeeded = true,
            };
        }

        //Send file to cloud and get url string
        private async Task<string> UploadFile(IFormFile file)
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Seek(0, SeekOrigin.Begin);
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream)
                };
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                return uploadResult.Url.ToString();
            }
        }
    }
}
