
using BlockedCountries.Application.Interfaces;
using BlockedCountries.Application.Services;
using BlockedCountries.Infrastructure.Repositories;

namespace BlockedCountries.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHostedService<TemporalBlockBackgroundService>();
            builder.Services.AddSingleton<ICountryRepository, CountryRepository>();
            builder.Services.AddSingleton<IBlockedAttemptRepository, BlockedAttemptRepository>();
            builder.Services.AddHttpClient<IGeolocationService, GeolocationService>(client =>
            {
                client.BaseAddress = new Uri("https://api.ipgeolocation.io"); 
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
