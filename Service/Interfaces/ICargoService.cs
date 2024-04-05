using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ICargoService
    {
        Task<Cargo> SaveCargoAsync(Cargo cargoModel);
        IEnumerable<Cargo> GetAllCargo();
        Task<Cargo> GetCargo(Guid id);


    }
}
