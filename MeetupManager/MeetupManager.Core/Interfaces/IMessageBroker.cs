namespace MeetupManager.Core.Interfaces;

public interface IMessageBroker
{
    public byte[] ProduceMessage<TMessage>(TMessage message);
    public string ConsumeMessage();
}
