using Core.Models;
using Data.Repo;
using Service.Interfaces;

namespace Service.Services
{
    public class CargoService : ICargoService
    {
        private readonly IRepo<Cargo> _CargoRepo = null!;
        public CargoService(IRepo<Cargo> repo)
        {
            _CargoRepo = repo;
        }
        public async Task<Cargo> SaveCargoAsync(Cargo cargoModel)
        {
            return await _CargoRepo.SaveAsync(cargoModel);
        }
        public IEnumerable<Cargo> GetAllCargo()
        {
            return _CargoRepo.GetAll();
        }
        public async Task<Cargo> GetCargo(Guid id)
        {
            return await _CargoRepo.GetById(id);
        }
    }
}
