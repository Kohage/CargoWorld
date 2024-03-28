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
        Task<Client> SaveClientAsync(Client clientModel);
        IEnumerable<Client> GetAllClient();
        Client GetClient(int id);
    }
}
