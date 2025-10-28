using Microsoft.AspNetCore.Mvc;

namespace Rentix.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BaseController : Controller
    {

    }
}
