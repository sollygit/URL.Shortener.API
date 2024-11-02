using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using URL.Shortener.Model;
using URL.Shortener.Repository.Configuration;

namespace URL.Shortener.Repository
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ShortenedUrl> ShortenedUrls { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new ShortenerConfiguration());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditEntities();
            return await base.SaveChangesAsync();
        }

        private void UpdateAuditEntities()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditable && x.State == EntityState.Added);
            foreach (var entry in modifiedEntries)
            {
                var entity = (IAuditable)entry.Entity;
                var now = DateTime.Now;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedOn = now;
                }
            }
        }
    }
}
