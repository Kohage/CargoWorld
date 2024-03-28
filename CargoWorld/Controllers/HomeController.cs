using Core.Models;
using Data;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Service.Interfaces;
using Service.Services;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace CargoWorld.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context; 
        private readonly IClientService _clientService;
        private readonly UserManager<Client> _userManager;
        private readonly SignInManager<Client> _signInManager;
        public HomeController(AppDbContext context, IClientService clientService, SignInManager<Client> signInManager, UserManager<Client> userManager)
        {
            _context = context;
            _clientService = clientService;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(Client user)
        {
                user.UserName = user.Name;
                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, user.Password);
                if (result.Succeeded)
                {
                    // установка куки
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            
            return View(user);
        }

        public IActionResult Login(Client client)
        {
            _clientService.SaveClientAsync(client);
            return View();
        }


    }
}
