using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IFreightService
    {
        Task<Freight> SaveFreightAsync(Freight freightModel);
        IEnumerable<Freight> GetAllFreight();
        Freight GetFreight(int id);
    }
}
