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
    public class RouteService : IRouteService
    {
        private readonly IRepo<Route> _RouteRepo = null!;
        public RouteService(IRepo<Route> repo)
        {
            _RouteRepo = repo;
        }
        public async Task<Route> SaveRouteAsync(Route routeModel)
        {
            return await _RouteRepo.SaveAsync(routeModel);
        }
        public IEnumerable<Route> GetAllRoute()
        {
            return _RouteRepo.GetAll();
        }
        public async Task<Route> GetRoute(Guid id)
        {
            return await _RouteRepo.GetById(id);
        }
    }
}
