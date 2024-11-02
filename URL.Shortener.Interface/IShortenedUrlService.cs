using System.Collections.Generic;
using System.Threading.Tasks;
using URL.Shortener.Model.ViewModels;

namespace URL.Shortener.Interface
{
    public interface IShortenedUrlService
    {
        Task<IEnumerable<ShortenedUrlView>> GetAllAsync();
        Task<ShortenedUrlView> CreateAsync(string url);
        Task<ShortenedUrlView> GetAsync(string code);
    }
}
