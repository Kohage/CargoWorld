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
        private readonly IRepo<AppUser> _ClientRepo = null!;
        public ClientService(IRepo<AppUser> repo)
        {
            _ClientRepo = repo;
        }
        public async Task<AppUser> SaveClientAsync(AppUser clientModel)
        {
            return await _ClientRepo.SaveAsync(clientModel);
        }
        public IEnumerable<AppUser> GetAllClient()
        {
            return  _ClientRepo.GetAll();
        }
        public async Task<AppUser> GetClient(Guid id)
        {
            return await _ClientRepo.GetById(id);
        }
    }
}
