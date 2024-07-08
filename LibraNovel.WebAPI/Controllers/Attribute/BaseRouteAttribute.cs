using Microsoft.AspNetCore.Mvc;

namespace LibraNovel.WebAPI.Controllers.Attribute
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class BaseRouteAttribute : RouteAttribute
    {
        public BaseRouteAttribute() : base("api/v{version:apiVersion}/[controller]")
        {
        }
    }
}
