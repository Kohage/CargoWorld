using Microsoft.AspNetCore.Mvc;

namespace CargoWorld.Controllers
{
    public class CargoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
