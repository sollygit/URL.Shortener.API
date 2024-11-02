using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using URL.Shortener.API.Settings;
using URL.Shortener.Common;
using URL.Shortener.Interface;
using URL.Shortener.Model;
using URL.Shortener.Model.ViewModels;
using URL.Shortener.Repository;

namespace URL.Shortener.API.Services
{
    public class ShortenedUrlService : IShortenedUrlService
    {
        private readonly ILogger<ShortenedUrlService> _logger;
        private readonly IShortenedUrlRepository _repo;
        private readonly URLShortenerSettings _settings;
        private readonly Random _random = new();

        public ShortenedUrlService(
            ILogger<ShortenedUrlService> logger,
            IShortenedUrlRepository repo,
            URLShortenerSettings settings)
        {
            _logger = logger;
            _repo = repo;
            _settings = settings;
        }

        public async Task<IEnumerable<ShortenedUrlView>> GetAllAsync()
        {
            // Get all shortened urls
            var items = await _repo.GetAllAsync();
            return Mapper.Map<ShortenedUrlView[]>(items);
        }

        public async Task<ShortenedUrlView> CreateAsync(string url)
        {
            // Validate url value
            if (string.IsNullOrEmpty(url.Trim())) throw new ServiceException("LongUrl cannot be empty");

            _logger.LogDebug("Generate a unique code for shortened url");
            
            var code = ShortenerUtil.GenerateUniqueCode(_settings.CodeLength, _settings.Alphabet);
            var shortenedUrl = new ShortenedUrl {
                Code = code,
                LongUrl = url, 
                ShortUrl = $"{Constants.Shortly}/{code}",
            };

            // Store in ShortenedUrl table
            var item = await _repo.CreateAsync(shortenedUrl);

            // Ensure no code duplicates
            return item == null
                ? throw new ServiceException($"ShortenedUrl with code {code} already exists in DB")
                : Mapper.Map<ShortenedUrlView>(item);
        }

        public async Task<ShortenedUrlView> GetAsync(string code)
        {
            var item = await _repo.GetByCodeAsync(code);
            return Mapper.Map<ShortenedUrlView>(item);
        }
    }
}
