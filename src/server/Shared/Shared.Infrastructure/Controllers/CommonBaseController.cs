using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Infrastructure.Controllers;

[ApiController]
[Route(BasePath + "/[controller]")]
public abstract class CommonBaseController : ControllerBase
{
    protected const string BasePath = "api/v{version:apiVersion}";

    private IMapper _mapperInstance;

    private IMediator _mediatorInstance;
    protected IMediator Mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>();
    protected IMapper Mapper => _mapperInstance ??= HttpContext.RequestServices.GetService<IMapper>();
}