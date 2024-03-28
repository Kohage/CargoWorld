using DAL;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace Core.Models;

public class UserSettings : BaseModel, IOwnByUser
{
    public bool IsNeedToDrawHeatMap { get; set; }
    public double RadiusOfObjectWithMaxCapacityInKilometers { get; set; } = 300;
    public Guid AppUserId { get; set; }
    public virtual AppUser AppUser { get; set; }
}
