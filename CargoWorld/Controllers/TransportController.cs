using Microsoft.AspNetCore.Mvc;

namespace CargoWorld.Controllers
{
    public class TransportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
