namespace MeetupManager.Infrastructure.Interfaces;

public interface IMessageBrokerService
{
    public byte[] SendMessage<TMessage>(TMessage message);
    public string ReceiveMessage();
}
