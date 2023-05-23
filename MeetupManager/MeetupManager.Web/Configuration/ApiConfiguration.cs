namespace MeetupManager.Web.Configuration;

public static class ApiConfiguration
{
    public static void SetConfiguration(IConfiguration configuration,IServiceCollection services)
    {
        SetServices(services);
        CoreConfiguration.SetConfiguration(configuration,services);
    }

    public static void SetMiddleware(WebApplication application)
    {
        if (application.Environment.IsDevelopment())
        {
            application.UseSwagger();
            application.UseSwaggerUI();
        }

        application.UseHttpsRedirection();
        application.UseAuthorization();
        application.MapControllers();
    }

    private static void SetServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}
