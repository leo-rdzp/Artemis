using Microsoft.EntityFrameworkCore;

namespace Artemis.Backend.Connections.Database
{
    public static class ArtemisDBConnect
    {
        public static IServiceCollection AddArtemisDatabase(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ArtemisDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("ArtemisConnection"),
                    b => b.MigrationsAssembly(typeof(ArtemisDbContext).Assembly.FullName)));

            // Validate connection
            try
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ArtemisDbContext>();
                context.Database.OpenConnection();
                context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                // Log the error
                var logger = services.BuildServiceProvider()
                    .GetRequiredService<ILogger<ArtemisDbContext>>();
                logger.LogError(ex, "Database connection failed");

                throw new ApplicationException("Could not connect to database. Please check your connection string and ensure the database is available.", ex);
            }

            return services;
        }
    }
}
