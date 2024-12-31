using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using URL.Shortener.API.Settings;
using URL.Shortener.Common;
using URL.Shortener.Interface;
using URL.Shortener.Repository;

namespace URL.Shortener.API
{
    public interface IDatabaseInitializer
    {
        Task SeedAsync();
    }

    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly IShortenedUrlService _shortenedUrlService;
        private readonly URLShortenerSettings _settings;
        private readonly ILogger _logger;

        public DatabaseInitializer(
            ApplicationDbContext context, 
            IShortenedUrlService shortenedUrlService, 
            URLShortenerSettings settings,
            ILogger<DatabaseInitializer> logger)
        {
            _context = context;
            _shortenedUrlService = shortenedUrlService;
            _settings = settings;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            await SeedShortenedUrls();
            await _context.SaveChangesAsync()
                .ContinueWith(task => {
                    if (task.IsFaulted) {
                        var exceptions = task.Exception.Flatten();
                        var exception = exceptions.InnerExceptions.FirstOrDefault();
                        _logger.LogError(exception, "SeedAsync() exception was thrown");
                    }
                });
        }

        // Seed DB with a few demo ShortenedUrls
        private async Task SeedShortenedUrls()
        {
            if (!await _context.ShortenedUrls.AnyAsync())
            {
                // Generage 5 fake urls to init DB
                var items = ShortenerUtil.ShortenedUrls(10, _settings.Alphabet);
                await _context.ShortenedUrls.AddRangeAsync(items);
                _logger.LogInformation("SeedShortenedUrls completed");
            }
        }
    }
}
