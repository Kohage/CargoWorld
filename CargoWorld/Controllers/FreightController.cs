using Core.Models;
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace CargoWorld.Controllers
{
	public class FreightController : Controller
	{

		private readonly IConfiguration _configuration;
		private readonly RoleManager<IdentityRole<Guid>> _roleManager;
		private readonly UserManager<AppUser> _userManager;
		private readonly IFreightService _freightService;

		public FreightController(UserManager<AppUser> userManager,
			RoleManager<IdentityRole<Guid>> roleManager,
			IConfiguration configuration, ICargoService cargoService, IFreightService freightService)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_configuration = configuration;
			_freightService = freightService;
		}



		[Authorize]
		public IActionResult Index()
		{
			var cargoes = _freightService.GetAllFreight();
			return View(cargoes);
		}
	}
}