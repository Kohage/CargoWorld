using System.ComponentModel;
using Core.Models;
using Data;
using Data.Repo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using Service.Services;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.FileSystemGlobbing;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

services.AddControllersWithViews();
services.AddRazorPages();
services.AddServerSideBlazor();
services.AddMvc();
services.AddRazorPages();

services.AddScoped<IRepo<Cargo>, Repo<Cargo>>();
services.AddScoped<ICargoService, CargoService>();

services.AddScoped<IRepo<AppUser>, Repo<AppUser>>();
services.AddScoped<IClientService, ClientService>();

services.AddScoped<IRepo<Freight>, Repo<Freight>>();
services.AddScoped<IFreightService, FreightService>();

services.AddScoped<IRepo<Core.Models.Route>, Repo<Core.Models.Route>>();
services.AddScoped<IRouteService, RouteService>();

services.AddScoped<IRepo<Transport>, Repo<Transport>>();
services.AddScoped<ITransportService, TransportService>();



services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql("Host=localhost; Database=CargoWorld; Username=postgres; Password=MyPassword;"));

services.AddIdentity<AppUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


//add authentication service with an encrypted cookie
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options => {
		options.Events = new JwtBearerEvents
		{
			OnMessageReceived = context =>
			{
				context.Token = context.Request.Cookies["token"];
				return Task.CompletedTask;
			}
		};
	});

services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
    {
	    policy.RequireClaim("Admin", "true");
    });

    options.AddPolicy("UserPolicy", policy =>
    {
	    policy.RequireClaim("User", "true");
    });

});

services.AddControllersWithViews();

var app = builder.Build();

CreateRoles(app.Services);
CreateAdmin(app.Services);
CreateUsers(app.Services);

app.MapGet("get", () =>
{
	return Results.Ok();
}).RequireAuthorization("AdminPolicy");

app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();    // подключение аутентификации
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

async Task CreateRoles(IServiceProvider serviceProvider)
{
    using var scope = app.Services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

    string[] roleNames = { "Admin", "User"};
    IdentityResult roleResult;

    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            //create the roles and seed them to the database: Question 2
            roleResult = await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
        }
    }
}

async Task CreateAdmin(IServiceProvider serviceProvider)
{
    using var scope = app.Services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        
    IdentityResult result;

    var _user = await userManager.FindByEmailAsync("123@bk.ru");

     if (_user == null)
    {
        var poweruser = new AppUser
        {
            Id = new Guid(),
            UserName = "123@bk.ru",
            Email = "123@bk.ru",
            FirstName = "Admin",
            SecondName = "Admin"
        };
        var userPWD = "E-lite1337";
        var createPowerUser = await userManager.CreateAsync(poweruser, userPWD);
        if (createPowerUser.Succeeded)
        {
            //here we tie the new user to the role
            await userManager.AddToRoleAsync(poweruser, "Admin");

        }
    }
}

async Task CreateUsers(IServiceProvider serviceProvider)
{
	using var scope = app.Services.CreateScope();
	var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

	IdentityResult result;

	var _users =  userManager.Users.ToList();


		var clients = new List<AppUser>
		{
			new AppUser
			{
				Id = new Guid(),
				UserName = "egasnoll0@dell.com",
				Email = "egasnoll0@dell.com",
				FirstName = "Eloise",
				SecondName = "Gasnoll", 
				UserSettingsId = default
			},
			new AppUser
			{
				Id = new Guid(),
				UserName = "ega@dell.com",
				Email = "ega0@dell.com",
				FirstName = "Davida",
				SecondName = "Abbis",
				UserSettingsId = default
			},
			new AppUser
			{
				Id = new Guid(),
				UserName = "el0@dell.com",
				Email = "e0@dell.com",
				FirstName = "Brucie",
				SecondName = "Rousel",
				UserSettingsId = default
			},
			new AppUser
			{
				Id = new Guid(),
				UserName = "ll0@dell.com",
				Email = "ll0@dell.com",
				FirstName = "Reggler",
				SecondName = "Lucien",
				UserSettingsId = default
			},
			new AppUser
			{
				Id = new Guid(),
				UserName = "bpylkynyton9@dot.gov",
				Email = "egasnoll0@dell.combpylkynyton9@dot.gov",
				FirstName= "Pylkynyton",
				SecondName = "Benny",
				UserSettingsId = default
			},
			new AppUser
			{
				Id = new Guid(),
				UserName = "egasnoll0@dell.com",
				Email = "egasnoll0@dell.com",
				FirstName = "Heijne",
				SecondName = "Haleigh",
				UserSettingsId = default
			},
			new AppUser
			{
				Id = new Guid(),
				UserName = "wlambg@seesaa.net",
				Email = "wlambg@seesaa.net",
				FirstName = "Giacomo",
				SecondName = "Drage",
				UserSettingsId = default
			},
			new AppUser
			{
				Id = new Guid(),
				UserName = "ktwelftreej@mysql.com'",
				Email = "ktwelftreej@mysql.com'",
				FirstName = "Abbott",
				SecondName = "Strugnell",
				UserSettingsId = default
			},
			new AppUser
			{
				Id = new Guid(),
				UserName = "lregglera@de.vu",
				Email = "lregglera@de.vu",
				FirstName = "Thatcher",
				SecondName = "Bakhrushin",
				UserSettingsId = default
			},
		};
		var userPWD = "E-lite1337";
		foreach (var client in clients)
		{
			var createPowerUser = await userManager.CreateAsync(client, userPWD);
			if (createPowerUser.Succeeded)
			{
				//here we tie the new user to the role
				await userManager.AddToRoleAsync(client, "Admin");

			}
		}
		
	
}

async Task CreateCargoes(IServiceProvider serviceProvider)
{
	using var scope = app.Services.CreateScope();
	var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

	IdentityResult result;

	var _user = await userManager.FindByEmailAsync("123@bk.ru");

	if (_user == null)
	{
		var poweruser = new AppUser
		{
			Id = new Guid(),
			UserName = "123@bk.ru",
			Email = "123@bk.ru",
			FirstName = "Admin",
			SecondName = "Admin"
		};
		var userPWD = "E-lite1337";
		var createPowerUser = await userManager.CreateAsync(poweruser, userPWD);
		if (createPowerUser.Succeeded)
		{
			//here we tie the new user to the role
			await userManager.AddToRoleAsync(poweruser, "Admin");

		}
	}
}

async Task CreateFreights(IServiceProvider serviceProvider)
{
	using var scope = app.Services.CreateScope();
	var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

	IdentityResult result;

	var _user = await userManager.FindByEmailAsync("123@bk.ru");

	if (_user == null)
	{
		var poweruser = new AppUser
		{
			Id = new Guid(),
			UserName = "123@bk.ru",
			Email = "123@bk.ru",
			FirstName = "Admin",
			SecondName = "Admin"
		};
		var userPWD = "E-lite1337";
		var createPowerUser = await userManager.CreateAsync(poweruser, userPWD);
		if (createPowerUser.Succeeded)
		{
			//here we tie the new user to the role
			await userManager.AddToRoleAsync(poweruser, "Admin");

		}
	}
}

async Task CreateRoutes(IServiceProvider serviceProvider)
{
	using var scope = app.Services.CreateScope();
	var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

	IdentityResult result;

	var _user = await userManager.FindByEmailAsync("123@bk.ru");

	if (_user == null)
	{
		var poweruser = new AppUser
		{
			Id = new Guid(),
			UserName = "123@bk.ru",
			Email = "123@bk.ru",
			FirstName = "Admin",
			SecondName = "Admin"
		};
		var userPWD = "E-lite1337";
		var createPowerUser = await userManager.CreateAsync(poweruser, userPWD);
		if (createPowerUser.Succeeded)
		{
			//here we tie the new user to the role
			await userManager.AddToRoleAsync(poweruser, "Admin");

		}
	}
}

async Task CreateTransport(IServiceProvider serviceProvider)
{
	using var scope = app.Services.CreateScope();
	var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

	IdentityResult result;

	var _user = await userManager.FindByEmailAsync("123@bk.ru");

	if (_user == null)
	{
		var poweruser = new AppUser
		{
			Id = new Guid(),
			UserName = "123@bk.ru",
			Email = "123@bk.ru",
			FirstName = "Admin",
			SecondName = "Admin"
		};
		var userPWD = "E-lite1337";
		var createPowerUser = await userManager.CreateAsync(poweruser, userPWD);
		if (createPowerUser.Succeeded)
		{
			//here we tie the new user to the role
			await userManager.AddToRoleAsync(poweruser, "Admin");

		}
	}
}
