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
        services.AddScoped<IValidator<MeetupDto>,MeetupDtoValidator>();
        services.AddScoped<IValidator<SpeakerDto>, SpeakerDtoValidator>();
        services.AddScoped<IValidator<SponsorDto>, SponsorDtoValidator>();
        services.AddScoped<IApiService<SpeakerDto>, SpeakerService>();
        services.AddScoped<IApiService<SponsorDto>, SponsorService>();
    }
}
