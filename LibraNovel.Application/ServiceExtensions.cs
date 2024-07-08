using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace LibraNovel.Application
{
    public static class ServiceExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            // Setting AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // we can add other services here

        }
    }
}
