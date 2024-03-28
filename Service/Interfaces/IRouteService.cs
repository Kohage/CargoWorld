using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IRouteService
    {
        Task<Route> SaveRouteAsync(Route routeModel);
        IEnumerable<Route> GetAllRoute();
        Route GetRoute(int id);
    }
}
