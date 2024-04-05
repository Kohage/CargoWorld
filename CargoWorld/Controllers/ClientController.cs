using Microsoft.AspNetCore.Mvc;

namespace CargoWorld.Controllers
{
    public class ClientController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
