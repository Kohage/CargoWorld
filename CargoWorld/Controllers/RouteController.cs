using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CargoWorld.Controllers
{
    [Authorize]
    public class RouteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
