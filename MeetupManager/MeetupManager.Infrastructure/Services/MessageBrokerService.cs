namespace MeetupManager.Infrastructure.Services;

public class MessageBrokerService: IMessageBrokerService
{
    private readonly ILogger<MessageBrokerService> _logger;
    private readonly IMessageBroker _broker;

    public MessageBrokerService(
        ILogger<MessageBrokerService> logger,
        IMessageBroker broker)
    {
        _logger = logger;
        _broker = broker;
    }

    public byte[] SendMessage<TMessage>(TMessage message)
    {
        var body=_broker.ProduceMessage(message);
        _logger.LogInformation("Message sent");
        return body;
    }

    public string ReceiveMessage()
    {
        var message=_broker.ConsumeMessage();
        _logger.LogInformation("Message recieved");
        return message;
    }    
}
