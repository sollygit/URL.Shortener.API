using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using URL.Shortener.Model;

namespace URL.Shortener.Repository.Configuration
{
    internal class ShortenerConfiguration : IEntityTypeConfiguration<ShortenedUrl>
    {
        public void Configure(EntityTypeBuilder<ShortenedUrl> builder)
        {
            builder.HasKey(m => m.ID).HasName("PK_ShortenedUrl");
            builder.Property(m => m.ID).IsRequired().HasDefaultValueSql("NEWID()");
            builder.Property(m => m.LongUrl).IsRequired();
            builder.Property(m => m.ShortUrl);
            builder.Property(m => m.Code);
            builder.ToTable("ShortenedUrl");
        }
    }
}
