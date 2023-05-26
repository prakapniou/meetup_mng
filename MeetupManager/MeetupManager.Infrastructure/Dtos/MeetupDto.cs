namespace MeetupManager.Infrastructure.Dtos;

public sealed class MeetupDto:BaseDto
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string Name { get; set; }
    public string Topic { get; set; }
    public string Description { get; set; }
    public string Schedule { get; set; }
    public string Address { get; set; }
    public DateTime Spending { get; set; }
    public List<Guid> SpeakerIds { get; } = new();
    public List<Guid> SponsorIds { get; } = new();
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

}
