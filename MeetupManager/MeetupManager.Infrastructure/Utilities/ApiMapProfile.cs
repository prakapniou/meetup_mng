namespace MeetupManager.Infrastructure.Utilities;

public sealed class ApiMapProfile:Profile
{
    public ApiMapProfile()
    {
        CreateMap<Speaker, SpeakerDto>().ReverseMap();
        CreateMap<Sponsor, SponsorDto>().ReverseMap();
        CreateMap<Meetup, MeetupDto>().ReverseMap();
    }
}
