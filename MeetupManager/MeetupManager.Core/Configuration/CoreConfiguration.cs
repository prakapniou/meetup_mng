using MeetupManager.Core.Utilities.MessageBrocker;

namespace MeetupManager.Core.Configuration;

public static class CoreConfiguration
{
    public static void SetConfiguration(IConfiguration configuration, IServiceCollection services)
    {
        SetDbContext(configuration, services);
        SetServices(services);
    }

    private static void SetDbContext(IConfiguration configuration, IServiceCollection services)
    {
        string connectionName = Connections.MsSqlServerDevelopment;
        string connectionString=configuration
            .GetConnectionString(connectionName)
            ?? throw new InvalidOperationException($"Connection \"{connectionName}\" not found");

        services.AddDbContext<ApiDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
            options.EnableSensitiveDataLogging();
        });
    }

    private static void SetServices(IServiceCollection services)
    {
        services.AddScoped(typeof(IApiRepository<>), typeof(ApiRepository<>));
        services.AddScoped<IMessageBroker,MessageBroker>();
    }
}
