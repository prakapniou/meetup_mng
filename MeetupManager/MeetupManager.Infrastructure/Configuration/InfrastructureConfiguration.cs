namespace MeetupManager.Infrastructure.Configuration;

public static class InfrastructureConfiguration
{
    public static void SetConfiguration(IServiceCollection services)
    {
        SetServices(services);
    }
    private static void SetServices(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ApiMapProfile));
    }
}
