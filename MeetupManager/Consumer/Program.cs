using Consumer;

//var services = new ServiceCollection();
//services.AddScoped(typeof(IMessageBroker<>), typeof(MessageBroker<>));
//services.AddScoped(typeof(IMessageBrokerService<>), typeof(MessageBrokerService<>));
//services.AddTransient<ILaunchService, LaunchService>();

//services.BuildServiceProvider().GetRequiredService<ILaunchService>().Run();



var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true);//throw excep

var config=builder.Build();
var queueDetails=SetQueueDetails(config);

var factory = new ConnectionFactory
{
    HostName = queueDetails.HostName
};

var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
channel.QueueDeclare(queueDetails.QueueName, exclusive: false);
var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Action(message);
};


channel.BasicConsume(queue: queueDetails.QueueName, autoAck: true, consumer: consumer);
Console.ReadKey();

static void Action(string message)
{
    Console.WriteLine(message);
}

static QueueDetails SetQueueDetails(IConfiguration configuration)
{
    var details = new QueueDetails
    {
        HostName=configuration["RabbitMQ:HostName"],
        QueueName=configuration["RabbitMQ:QueueName"]
    };

    return details;
}