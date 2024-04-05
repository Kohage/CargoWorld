using Core.Models;
using Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Mime;
using DocumentFormat.OpenXml;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Document = System.Reflection.Metadata.Document;
using Table = Microsoft.EntityFrameworkCore.Metadata.Internal.Table;

namespace CargoWorld.Controllers
{
    public class CargoController : Controller
	{

		private readonly IConfiguration _configuration;
		private readonly RoleManager<IdentityRole<Guid>> _roleManager;
		private readonly UserManager<AppUser> _userManager;
		private readonly ICargoService _cargoService;
		private readonly IClientService _clientService;

		private AppDbContext db = new AppDbContext();
		

		public CargoController(UserManager<AppUser> userManager,
			RoleManager<IdentityRole<Guid>> roleManager,
			IConfiguration configuration, ICargoService cargoService, IClientService clientService)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_configuration = configuration;
			_cargoService = cargoService;
			_clientService = clientService;
		}


		public IActionResult Index(string? sortOrder,string? searchString)
		{
				string? token;
			
					token = Request.Cookies["token"];

					var handler = new JwtSecurityTokenHandler();
					var jwtSecurityToken = handler.ReadJwtToken(token);

					ViewBag.VolumeSortParam = String.IsNullOrEmpty(sortOrder) ? "vol_desc" : "";
					ViewBag.DiscriptionSortParam = sortOrder == "Description" ? "disc_desc" : "Description";
					var foperations = from f in db.Cargos select f;

					switch (sortOrder)
					{
						case "vol_desc":
							foperations = foperations.OrderByDescending(f => f.Volume);
							break;
						case "disc_desc":
							foperations = foperations.OrderByDescending(f => f.Description);
							break;
						case "Description":
							foperations = foperations.OrderBy(f => f.Volume);
							break;
						case "":
							foperations = foperations.OrderBy(f => f.Volume);
							break;
					}

					if (!String.IsNullOrEmpty(searchString))
					{
						foperations = foperations.Where(c => c.Description.Contains(searchString));
					}



					var cargoes = _cargoService.GetAllCargo();
					return View(foperations.ToList());


		}

		[HttpGet]
		public ActionResult Create()
		{
			ViewBag.Id = new SelectList( db.Clients, "Id", "Id");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(Cargo cargo, IFormFile Image)
		{

			// validate that our model meets the requirement

				try
				{
					if(Image != null){
					var memoryStream = new MemoryStream();
					Image.CopyTo(memoryStream);
					cargo.Image = memoryStream.ToArray();
					}
					else
					{
						cargo.Image = new[]{(byte)10};
					}
					// sync the changes of ef code in memory with the database
					await _cargoService.SaveCargoAsync(cargo);
					
					return RedirectToAction("Index");
				}
				catch (Exception ex)
				{
					ModelState.AddModelError(string.Empty, $"Something went wrong {ex.Message}");
				}

			ViewBag.Id = new SelectList(db.Clients, "Id", "Id");
			ModelState.AddModelError(string.Empty, $"Something went wrong, invalid model");

			// We return the object back to view
			return View(cargo);
		}

		public IActionResult Edit()
		{
			var cargoes = _cargoService.GetAllCargo();
			return View(cargoes.ToList());
		}

		public IActionResult Delete()
		{
			var cargoes = _cargoService.GetAllCargo();
			return View(cargoes.ToList());
		}

		public byte[] ImageToByteArray(System.Drawing.Image imageIn)
		{
			using (var ms = new MemoryStream())
			{
				imageIn.Save(ms, imageIn.RawFormat);
				return ms.ToArray();
			}
		}

		public ActionResult ExportToWord()
		{

				// создаем документ word 
				using (var document = WordprocessingDocument.Create("C:\\Users\\89829\\Downloads\\table.docx",
					       WordprocessingDocumentType.Document))
				{
					// добавляем основную часть документа 
					var mainPart = document.AddMainDocumentPart();
					mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();
					// добавляем тело документа 
					var body = mainPart.Document.AppendChild(new Body());
					// добавляем параграф с заголовком 
					var paragraph = body.AppendChild(new Paragraph());
					DocumentFormat.OpenXml.Wordprocessing.Run run =
						paragraph.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
					run.AppendChild(new Text("Таблица"));
					// добавляем таблицу 
					var table = body.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Table());
					// добавляем свойства таблицы 
					var tableProperties = table.AppendChild(new TableProperties(
						new TableBorders(
							new TopBorder
							{
								Val = new EnumValue<BorderValues>(BorderValues.Single),
								Size = 12
							},
							new BottomBorder
							{
								Val = new EnumValue<BorderValues>(BorderValues.Single),
								Size = 12
							},
							new LeftBorder
							{
								Val = new EnumValue<BorderValues>(BorderValues.Single),
								Size = 12
							},
							new RightBorder
							{
								Val = new EnumValue<BorderValues>(BorderValues.Single),
								Size = 12
							},
							new InsideHorizontalBorder
							{
								Val = new EnumValue<BorderValues>(BorderValues.Single),
								Size = 12
							},
							new InsideVerticalBorder
							{
								Val = new EnumValue<BorderValues>(BorderValues.Single),
								Size = 12
							}
						)
					));
					// добавляем строку с заголовками столбцов 
					var headerRow = table.AppendChild(new TableRow());
					headerRow.AppendChild(new TableCell(new Paragraph(new Run(new Text("Id")))));
					headerRow.AppendChild(new TableCell(new Paragraph(new Run(new Text("Description")))));
					headerRow.AppendChild(new TableCell(new Paragraph(new Run(new Text("Volume")))));
					headerRow.AppendChild(new TableCell(new Paragraph(new Run(new Text("Weight")))));
					headerRow.AppendChild(new TableCell(new Paragraph(new Run(new Text("Client_Id")))));
					// получаем данные из базы данных 
					var stored = db.Cargos.ToList();
					// добавляем строки с данными 
					foreach (var item in stored)
					{
						var dataRow = table.AppendChild(new TableRow());
						dataRow.AppendChild(new TableCell(new Paragraph(new Run(new Text(item.Id.ToString())))));
						dataRow.AppendChild(new TableCell(new Paragraph(new Run(new Text(item.Description)))));
						dataRow.AppendChild(new TableCell(new Paragraph(new Run(new Text(item.Volume.ToString())))));
						dataRow.AppendChild(new TableCell(new Paragraph(new Run(new Text(item.Weight.ToString())))));
						dataRow.AppendChild(new TableCell(new Paragraph(new Run(new Text(item.Client_Id.ToString())))));
					}

					// сохраняем документ 
					mainPart.Document.Save();
				}

				// возвращаем файловый результат с документом word 
				return RedirectToAction("Index");
			
		}
	}
}
