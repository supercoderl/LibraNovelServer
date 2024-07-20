using LibraNovel.Application;
using LibraNovel.Application.Interfaces;
using LibraNovel.Application.Services;
using LibraNovel.Application.ViewModels.Email;
using LibraNovel.Application.ViewModels.Genre;
using LibraNovel.Infrastructure.Data.Context;
using LibraNovel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Asp.Versioning;
using Microsoft.Extensions.Configuration;

namespace LibraNovel.Infrastructure.IoC
{
    public static class DependencyContainer
    {
        public static void AddIoCService(this IServiceCollection services, IConfiguration configuration)
        {
            // IoC - Inversion Of Control
            services.AddApplicationServices();
            services.AddDatabaseServices(configuration);
            services.AddAspNetCoreServices();
            services.AddConfigurationServices(configuration);
        }

        public static void AddApplicationServices(this IServiceCollection services)
        {
            // Application
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<INovelService, NovelService>();
            services.AddScoped<IChapterService, ChapterService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IBookmarkService, BookmarkService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<ITokenCache, TokenCache>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IQRCodeService, QRCodeService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IPaypalService, PaypalService>();
            services.AddScoped<ICardService, CardService>();
            services.AddScoped<IVnPayService, VnPayService>();
        }

        public static void AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Database
            services.AddDbContext<LibraNovelContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("RedisConnection");
            });
        }

        public static void AddAspNetCoreServices(this IServiceCollection services)
        {
            //Asp.NET Core Services
            services.AddResponseCaching();
            services.AddMemoryCache();
            services.AddControllers();
            services.AddHttpClient();
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            services.AddCors(o => o.AddPolicy("Policy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddHttpContextAccessor();
        }

        public static void AddConfigurationServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Configuration
            services.AddSingleton(configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
        }
    }
}
