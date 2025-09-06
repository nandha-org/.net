using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ThunderPay.Database.Sagas;

namespace ThunderPay.Database;

public static class DatabaseIoC
{
    public static IServiceCollection RegisterDatabaseServices(IServiceCollection services)
    {
        services.AddDbContext<ThunderPayDbContext>(options =>
            options.UseNpgsql(ConstructDbConnection(), npg =>
                npg.MigrationsHistoryTable("__EFMigrationsHistory", DatabaseConsts.ThunderPayDefaultSchema)));

        services.AddDbContext<PaymentSagaDbContext>(options =>
            options.UseNpgsql(ConstructDbConnection(), npg =>
                npg.MigrationsAssembly(typeof(PaymentSagaDbContext).Assembly.GetName().Name)
                    .MigrationsHistoryTable("__EFMigrationsHistory", DatabaseConsts.SagaDefaultSchema)));

        return services;
    }

    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var appDbContext = scope.ServiceProvider.GetRequiredService<ThunderPayDbContext>();
        var sagaDbContext = scope.ServiceProvider.GetRequiredService<PaymentSagaDbContext>();

        try
        {
            Console.WriteLine($"Pending saga migrations '{sagaDbContext.Database.GetPendingMigrations().Count()}'");
            sagaDbContext.Database.Migrate();
            Console.WriteLine($"Saga migrations applied to database. Pending migrations '{sagaDbContext.Database.GetPendingMigrations().Count()}'");

            Console.WriteLine($"Pending app migrations '{appDbContext.Database.GetPendingMigrations().Count()}'");
            appDbContext.Database.Migrate();
            Console.WriteLine($"App migrations applied to database. Pending migrations '{appDbContext.Database.GetPendingMigrations().Count()}'");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while applying migrations: {ex.Message}");
            throw;
        }
    }

    internal static string ConstructDbConnection()
    {
        // Retrieve environment variables
        string dbHost = Env.GetString("DB_HOST") ?? throw new Exception("DB_HOST not found");
        string dbPort = Env.GetString("DB_PORT") ?? throw new Exception("DB_PORT not found");
        string dbName = Env.GetString("DB_NAME") ?? throw new Exception("DB_NAME not found");
        string dbUsername = Env.GetString("DB_USERNAME") ?? throw new Exception("DB_USERNAME not found");
        string dbPassword = Env.GetString("DB_PASSWORD") ?? throw new Exception("DB_PASSWORD not found");

        // Construct the connection string
        string connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUsername};Password={dbPassword};Include Error Detail=true";

        return connectionString;
    }
}
