using MeetupManager.Core.Utilities.MessageBroker;

namespace MeetupManager.Core.Utilities.MessageBrocker;

/// <summary>
/// 
/// </summary>
public sealed class MessageBroker: IMessageBroker
{
    private readonly QueueDetails _queueDetails;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    public MessageBroker(IConfiguration configuration)
    {
        _queueDetails=SetQueueDetails(configuration);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="Tmessage"></typeparam>
    /// <param name="message"></param>
    /// <returns></returns>
    public byte[] ProduceMessage<TMessage>(TMessage message)
    {
        var factory = new ConnectionFactory
        {
            HostName = _queueDetails.HostName
        };

        var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(_queueDetails.QueueName, exclusive: false);
        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);
        channel.BasicPublish(exchange: "", routingKey: _queueDetails.QueueName, body: body);
        return body;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string ConsumeMessage()
    {
        var factory = new ConnectionFactory
        {
            HostName = _queueDetails.HostName
        };

        var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(_queueDetails.QueueName, exclusive: false);
        var consumer = new EventingBasicConsumer(channel);
        var message = string.Empty;

        consumer.Received += (model, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            message = Encoding.UTF8.GetString(body);
        };

        channel.BasicConsume(queue: _queueDetails.QueueName, autoAck: true, consumer: consumer);
        return message;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    private static QueueDetails SetQueueDetails(IConfiguration configuration)
    {
        var details = new QueueDetails
        {
            HostName=configuration["RabbitMQ:HostName"],
            QueueName=configuration["RabbitMQ:QueueName"]
        };

        return details;
    }
}
