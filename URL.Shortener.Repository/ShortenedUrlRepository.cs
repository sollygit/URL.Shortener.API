using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URL.Shortener.Interface;
using URL.Shortener.Model;

namespace URL.Shortener.Repository
{
    public interface IShortenedUrlRepository : IRepository<ShortenedUrl>
    {
        Task<IEnumerable<ShortenedUrl>> GetAllAsync();
        Task<ShortenedUrl> CreateAsync(ShortenedUrl shortenedUrl);
        Task<ShortenedUrl> GetByCodeAsync(string code);
    }

    public class ShortenedUrlRepository : BaseRepository<ShortenedUrl>, IShortenedUrlRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ShortenedUrlRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<ShortenedUrl>> GetAllAsync()
        {
            return await _dbContext.ShortenedUrls
                .OrderBy(p => p.CreatedOn)
                .ToListAsync();
        }

        public async Task<ShortenedUrl> CreateAsync(ShortenedUrl shortenedUrl)
        {
            // Throw if unique code already exists in DB
            if (_dbContext.ShortenedUrls.Any(m => m.Code == shortenedUrl.Code)) return null;

            var entry = _dbContext.Add(shortenedUrl);
            await _dbContext.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<ShortenedUrl> GetByCodeAsync(string code)
        {
            return await _dbContext.ShortenedUrls
                .SingleOrDefaultAsync(m => m.Code == code);
        }
    }
}
