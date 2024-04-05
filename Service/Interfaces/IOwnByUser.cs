using Core.Models;

namespace Service.Interfaces
{
    public interface IOwnByUser
    {
        public Guid AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
