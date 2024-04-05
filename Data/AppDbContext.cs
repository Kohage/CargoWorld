
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {

	    public AppDbContext() { }
	    public AppDbContext(DbContextOptions<AppDbContext> options)
		    : base(options)
	    { }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost; Database=CargoWorld; Username=postgres; Password=MyPassword;");
                
            }
            
        }

        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<AppUser> Clients { get; set; }
        public DbSet<Freight> Freights { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Transport> Transports { get; set; }
    }
}
