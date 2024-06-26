﻿using Service.Interfaces;
using Data.Repo;
using Core.Models;

namespace Service.Services
{
    public class TransportService : ITransportService
    {
        private readonly IRepo<Transport> _TransportRepo = null!;
        public TransportService(IRepo<Transport> repo)
        {
            _TransportRepo = repo;
        }
        public async Task<Transport> SaveTransportAsync(Transport transportModel)
        {
            return await _TransportRepo.SaveAsync(transportModel);
        }
        public IEnumerable<Transport> GetAllTransport()
        {
            return  _TransportRepo.GetAll();
        }
        public async Task<Transport> GetTransport(Guid id)
        {
            return await _TransportRepo.GetById(id);
        }
    }
}
