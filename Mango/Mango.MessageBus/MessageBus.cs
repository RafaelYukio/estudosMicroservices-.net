using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

namespace Mango.MessageBus
{
    public class MessageBus : IMessageBus
    {
        // Geralmente, quando temos service bus em cada serviço, a connectionString é salva no appsettings
        private string connectionString = "Endpoint=sb://estudosmicroservices.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=3wWoeWlH37IpSFHE31BEXijfvoFWBCuk5+ASbJMHxbE=";

        public async Task PublishMessage(object message, string topic_queue_name)
        {
            await using var client = new ServiceBusClient(connectionString);

            ServiceBusSender sender = client.CreateSender(topic_queue_name);

            var jsonMessage = JsonConvert.SerializeObject(message);
            ServiceBusMessage finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString()
            };

            await sender.SendMessageAsync(finalMessage);
            await client.DisposeAsync();
        }
    }
}
