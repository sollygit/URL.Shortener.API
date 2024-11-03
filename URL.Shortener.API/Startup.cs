using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using URL.Shortener.API.Services;
using URL.Shortener.API.Settings;
using URL.Shortener.Interface;
using URL.Shortener.Model;
using URL.Shortener.Repository;

namespace URL.Shortener.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    // Angular app origin URL
                    builder => builder.WithOrigins(Configuration["CorsUrl"])
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            // Mapping profile setup
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });

            var settings = Configuration.GetSection("URLShortenerSettings");

            // Configurations
            services.Configure<URLShortenerSettings>(settings);

            services.AddMemoryCache();

            // Use SqlServer localdb ConnectionString
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration["ConnectionStrings:DefaultConnection"],
                    b => b.MigrationsAssembly("URL.Shortener.API")));

            //Use InMemoryDatabase
            //services.AddDbContext<ApplicationDbContext>(o => {
            //    o.UseInMemoryDatabase("ShortenedUrl");
            //});

            services.AddSingleton(provider => settings.Get<URLShortenerSettings>());
            services.AddTransient<IShortenedUrlRepository, ShortenedUrlRepository>();
            services.AddScoped<IShortenedUrlService, ShortenedUrlService>();
            services
                .AddRouting(o => o.LowercaseUrls = true)
                .AddControllers()
                .AddJsonOptions(o => o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

            // Using Fluent validation
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<ShortenedUrlValidator>(ServiceLifetime.Transient);

            // Swagger doco added
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "URL Shortener API", Version = "v1" });
            });

            // DB seeding with fake URL's
            services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "Swagger - URL Shortener API";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "URL Shortener API v1");
                c.RoutePrefix = string.Empty;
            });
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
