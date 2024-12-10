using Microsoft.AspNetCore.Mvc;
using Shared.Infrastructure.Controllers;

namespace Modules.EmployeeManagement.Controllers;

[ApiController]
[Route(BasePath + "/[controller]")]
public class BaseController : CommonBaseController
{
    protected new const string BasePath = CommonBaseController.BasePath + "/staff";
}