using edson268_landing_page_api.Core.Entities;
using edson268_landing_page_api.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace edson268_landing_page_api.Data.Repositories
{
    internal class CertificateRepository : ICertificateRepository
    {
        private readonly LandingPageDbContext _context;

        public CertificateRepository(LandingPageDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Certificate>> GetAllAsync()
        {
            return await _context.Certificates
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Certificate?> GetByIdAsync(Guid id)
        {
            return await _context.Certificates.FindAsync(id);
        }

        public async Task<Certificate> AddAsync(Certificate certificate)
        {
            _context.Certificates.Add(certificate);
            await _context.SaveChangesAsync();
            return certificate;
        }

        public async Task UpdateAsync(Certificate certificate)
        {
            _context.Entry(certificate).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var certificate = await GetByIdAsync(id);
            if (certificate != null)
            {
                _context.Certificates.Remove(certificate);
                await _context.SaveChangesAsync();
            }
        }
    }
}