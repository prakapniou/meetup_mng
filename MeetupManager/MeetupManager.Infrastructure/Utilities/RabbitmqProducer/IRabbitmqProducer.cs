namespace MeetupManager.Infrastructure.Utilities.RabbitmqProducer;

public interface IRabbitmqProducer
{
    public void SendMeetupMessage<T>(T message) where T : class;
}
