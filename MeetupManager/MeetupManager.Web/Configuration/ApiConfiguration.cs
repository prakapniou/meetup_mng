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
}
