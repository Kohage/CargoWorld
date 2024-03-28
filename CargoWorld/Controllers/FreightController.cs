using Microsoft.AspNetCore.Mvc;

namespace CargoWorld.Controllers
{
    public class FreightController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
