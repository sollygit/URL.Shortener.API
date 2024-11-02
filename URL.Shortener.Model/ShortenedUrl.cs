using FluentValidation;
using System;

namespace URL.Shortener.Model
{
    public class ShortenedUrl : AuditableEntity
    {
        public Guid ID { get; set; }
        public string LongUrl { get; set; }
        public string ShortUrl { get; set; }
        public string Code { get; set; }
    }

    public class ShortenedUrlValidator : AbstractValidator<ShortenedUrl>
    {
        public ShortenedUrlValidator()
        {
            RuleFor(x => x.LongUrl).NotEmpty().WithMessage("LongUrl cannot be empty");
        }
    }
}
