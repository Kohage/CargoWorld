using Core.Models;

namespace Service.Interfaces
{
    public interface ITransportService
    {
        Task<Transport> SaveTransportAsync(Transport transportModel);
        IEnumerable<Transport> GetAllTransport();
        Task<Transport> GetTransport(Guid id);
    }
}
