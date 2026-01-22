using edson268_landing_page_api.Core.Entities;

namespace edson268_landing_page_api.Core.Interfaces
{
    public interface ICertificateRepository
    {
        Task<IEnumerable<Certificate>> GetAllAsync();
        Task<Certificate?> GetByIdAsync(Guid id);
        Task<Certificate> AddAsync(Certificate certificate);
        Task UpdateAsync(Certificate certificate);
        Task DeleteAsync(Guid id);
    }
}
