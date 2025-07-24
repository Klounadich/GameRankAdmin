using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
namespace GameRankAdminPanel.Services.RabbitMQ;

public class RabbitMQService
{

    public RabbitMQService()
    {



    }

    public async Task Send()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",

        };
        using var connection = await factory.CreateConnectionAsync();
        using var chanell = await connection.CreateChannelAsync();

        await chanell.QueueDeclareAsync(queue: "Admin", durable: false, exclusive: false, autoDelete: false,
            arguments: null);
        const string message = "User is Admin";
        var body = Encoding.UTF8.GetBytes(message);

        await chanell.BasicPublishAsync(exchange: string.Empty, routingKey: "Admin", body: body);

    }

    public async Task Get()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: "Admin", durable: false, exclusive: false, autoDelete: false,
            arguments: null);

        Console.WriteLine(" [*] Waiting for messages.");

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($" [x] Received {message}");
            return Task.CompletedTask;
        };

        await channel.BasicConsumeAsync("Admin", autoAck: true, consumer: consumer);
    }
}