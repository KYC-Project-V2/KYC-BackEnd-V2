using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KYCServiceApi.Controllers
{
    [Authorize]
    public class BaseController : ControllerBase
    {

    }
}
