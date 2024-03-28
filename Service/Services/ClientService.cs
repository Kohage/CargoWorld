using Core.Models;
using Data.Repo;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class ClientService : IClientService
    {
        private readonly IRepo<Client> _ClientRepo = null!;
        public ClientService(IRepo<Client> repo)
        {
            _ClientRepo = repo;
        }
        public async Task<Client> SaveClientAsync(Client clientModel)
        {
            return await _ClientRepo.SaveAsync(clientModel);
        }
        public IEnumerable<Client> GetAllClient()
        {
            return _ClientRepo.GetAll();
        }
        public Client GetClient(int id)
        {
            return _ClientRepo.GetById(id);
        }
    }
}
