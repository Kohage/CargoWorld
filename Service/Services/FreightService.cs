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
    public class FreightService : IFreightService
    {
        private readonly IRepo<Freight> _FreightRepo = null!;
        public FreightService(IRepo<Freight> repo)
        {
            _FreightRepo = repo;
        }
        public async Task<Freight> SaveFreightAsync(Freight freightModel)
        {
            return await _FreightRepo.SaveAsync(freightModel);
        }
        public IEnumerable<Freight> GetAllFreight()
        {
            return _FreightRepo.GetAll();
        }
        public Freight GetFreight(int id)
        {
            return _FreightRepo.GetById(id);
        }
    }
}
