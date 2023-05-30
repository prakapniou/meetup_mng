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
        SetServices(services,configuration);
        SetAuth(services);
        CoreConfiguration.SetConfiguration(configuration,services);
        InfrastructureConfiguration.SetConfiguration(services);
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
        application.UseAuthentication();
        application.MapControllers();
        application.UseMiddleware<ExceptionHandlerMiddleware>();
    }

    private static void SetServices(IServiceCollection services,IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddScoped<IRabbitmqProducer,RabbitmqProducer>();
        services.AddTransient<ExceptionHandlerMiddleware>();
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

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            options.EnableAnnotations();            
        });
    }

    private static void SetAuth(IServiceCollection services)
    {
        services.AddSwaggerGen(options=>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },

                    Array.Empty<string>()
                }
            });
        });

        services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme) //bearer is default schema
            .AddIdentityServerAuthentication(options =>
            {
                //the URL on which the IdentityServer is up and running
                options.Authority="http://localhost:44390";
                //the name of the WebAPI resource
                options.ApiName="myApi";
                //select false for the development
                options.RequireHttpsMetadata= false;
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
