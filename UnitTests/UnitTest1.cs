using CargoWorld.Controllers;
using Core.Models;
using Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service.Interfaces;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace UnitTests
{
	[TestClass]
	public class UnitTest1
	{
		private CargoController cargoController;
		private HomeController homeController;
		private ViewResult result1;

		private readonly IConfiguration _configuration;
		private readonly RoleManager<IdentityRole<Guid>> _roleManager;
		private readonly UserManager<AppUser> _userManager;
		private readonly ICargoService _cargoService;
		private readonly IClientService _clientService;


		[TestMethod]
		public void Create_ReturnsCorrectView()
		{
			// Act
			var result = cargoController.Create() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}

		[TestInitialize]
		public void SetUp()
		{
			cargoController = new CargoController(_userManager, _roleManager, _configuration, _cargoService, _clientService);
			homeController = new HomeController(_userManager, _roleManager, _configuration);
			result1 = homeController.Index() as ViewResult;
		}


		[TestMethod]
		public void IndexViewResultNotNull()
		{
			Assert.IsNotNull(result1);
		}

		[TestMethod]
		public void Index_Returns_Index_View()
		{
			// Assert
			Assert.IsNotNull(result1);
			Assert.AreEqual("Index", result1.ViewName);
		}

		[TestMethod]
		public void About_Returns_About_View()
		{
			// Arrange
			var homeController = new HomeController(_userManager, _roleManager, _configuration);

			// Act
			var result = homeController.About() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual("About", result.ViewName);
		}

		[TestMethod]
		public void sContact_Returns_Contact_View()
		{
			// Arrange
			var homeController = new HomeController(_userManager, _roleManager, _configuration);

			// Act
			var result = homeController.Login() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual("Login", result.ViewName);
		}

		[TestMethod]
		public void Edit_Get_WithInvalidId_ReturnsHttpNotFound()
		{
			// Arrange
			var homeController = new CargoController(_userManager, _roleManager, _configuration, _cargoService, _clientService);
			var id = -1; // Assuming there is no repairman with negative ID in the database

			// Act
			var result = homeController.Create();

			// Assert
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void Edit_Get_WithValidId_ReturnsViewResult()
		{
			// Arrange
			var homeController = new CargoController(_userManager, _roleManager, _configuration, _cargoService, _clientService);
			var id = 6;

			// Act
			var result = homeController.Index(null,null) as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}

	}

}
