using Core.Models;
using Data;
using Data.Repo;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using Service.Services;
using Microsoft.AspNetCore.Identity.UI;
using IdentityRole = Microsoft.AspNetCore.Identity.IdentityRole;
using IdentityUser = Microsoft.AspNetCore.Identity.IdentityUser;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNet.Identity;

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

services.AddScoped<IRepo<Client>, Repo<Client>>();
services.AddScoped<IClientService, ClientService>();

services.AddScoped<IRepo<Freight>, Repo<Freight>>();
services.AddScoped<IFreightService, FreightService>();

services.AddScoped<IRepo<Core.Models.Route>, Repo<Core.Models.Route>>();
services.AddScoped<IRouteService, RouteService>();

services.AddScoped<IRepo<Transport>, Repo<Transport>>();
services.AddScoped<ITransportService, TransportService>();



services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql("Host=localhost; Database=CargoWorld; Username=postgres; Password=MyPassword;"));


services.AddIdentity<Client, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();



services.AddControllersWithViews();

var app = builder.Build();

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