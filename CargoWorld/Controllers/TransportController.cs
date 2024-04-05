using Core.Models;
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace TransportWorld.Controllers
{
	public class TransportController : Controller
	{

		private readonly IConfiguration _configuration;
		private readonly RoleManager<IdentityRole<Guid>> _roleManager;
		private readonly UserManager<AppUser> _userManager;
		private readonly ITransportService _cargoService;

		public TransportController(UserManager<AppUser> userManager,
			RoleManager<IdentityRole<Guid>> roleManager,
			IConfiguration configuration, ITransportService cargoService)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_configuration = configuration;
			_cargoService = cargoService;
		}



		[Authorize]
		public IActionResult Index()
		{
			var cargoes = _cargoService.GetAllTransport();
			return View(cargoes);
		}
	}
}