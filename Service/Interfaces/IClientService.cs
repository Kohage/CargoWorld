using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IClientService
    {
        Task<AppUser> SaveClientAsync(AppUser clientModel);
        IEnumerable<AppUser> GetAllClient();
        Task<AppUser> GetClient(Guid id);
    }
}
