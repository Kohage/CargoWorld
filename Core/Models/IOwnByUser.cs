namespace Core.Models;

public interface IOwnByUser
{
    public Guid AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}