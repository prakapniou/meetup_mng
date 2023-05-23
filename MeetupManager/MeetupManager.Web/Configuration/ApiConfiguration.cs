using ILogger = Serilog.ILogger;

namespace MeetupManager.Web.Configuration;

/// <summary>
/// 
/// </summary>
public static class ApiConfiguration
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="services"></param>
    /// <param name="logging"></param>
    /// <returns></returns>
    public static ILogger SetConfiguration(
        IConfiguration configuration,
        IServiceCollection services,
        ILoggingBuilder logging)
    {
        var logger = SetLogger(configuration,logging);
        SetServices(services);
        CoreConfiguration.SetConfiguration(configuration,services);
        return logger;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="application"></param>
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
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Meetup managment",
                Description = "Management Api on the example of meetups based on .Net 7.0",
                TermsOfService = new Uri("https://example.com/terms"),

                Contact = new OpenApiContact
                {
                    Name = "Example Contact",
                    Url = new Uri("https://example.com/contact")
                },

                License = new OpenApiLicense
                {
                    Name = "Example License",
                    Url = new Uri("https://example.com/license")
                }
            });
        });
    }

    private static ILogger SetLogger(IConfiguration configuration,ILoggingBuilder logging)
    {
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        logging.ClearProviders();
        logging.AddSerilog(logger);

        return logger;
    }
}
