using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using LibraNovel.WebAPI.Controllers.Attribute;

namespace LibraNovel.WebAPI.Controllers
{
    [BaseRoute]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        private IMediator? _mediator;
        protected IMediator? Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    }
}
