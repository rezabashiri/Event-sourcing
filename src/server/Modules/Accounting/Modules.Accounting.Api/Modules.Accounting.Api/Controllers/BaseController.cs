using Microsoft.AspNetCore.Mvc;
using Shared.Infrastructure.Controllers;

namespace Modules.Accounting.Application.Controllers;

[ApiController]
[Route(BasePath + "/[controller]")]
public class BaseController : CommonBaseController
{
    private new const string BasePath = CommonBaseController.BasePath + "/accounting";
}