using Bogus;
using System;
using System.Collections.Generic;
using URL.Shortener.Model;

namespace URL.Shortener.Common
{
    public static class ShortenerUtil
    {
        static readonly Random _random = new Random();

        public static string GenerateUniqueCode(int codeLength, string alphabet)
        {
            var codeChars = new char[codeLength];
            int maxValue = alphabet.Length;

            for (var i = 0; i < codeLength; i++)
            {
                var randomIndex = _random.Next(maxValue);
                codeChars[i] = alphabet[randomIndex];
            }

            return new string(codeChars);
        }

        public static List<ShortenedUrl> ShortenedUrls(int count, string alphabet)
        {
            var faker = new Faker();
            var urls = new List<ShortenedUrl>();

            for (int i = 0; i < count; i++) 
            {
                var longUrl = faker.Internet.UrlWithPath();
                var code = GenerateUniqueCode(8, alphabet);
                var shortUrl = $"{Constants.Shortly}/{code}";

                urls.Add(new ShortenedUrl { LongUrl = longUrl, ShortUrl = shortUrl, Code = code });
            }

            return urls;
        }
    }
}
