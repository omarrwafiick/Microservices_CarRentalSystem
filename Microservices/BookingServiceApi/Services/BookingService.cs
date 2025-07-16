 
using BookingServiceApi.Interfaces;
using BookingServiceApi.Models;
using Common.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace BookingServiceApi.Services
{
    public class BookingService : IBookingService
    {
        private readonly IGetAllRepository<Booking> _getAllRepository;
        private readonly IGetRepository<Booking> _getRepository;
        private readonly ICreateRepository<Booking> _createRepository;
        private readonly IUpdateRepository<Booking> _updateRepository; 
        public BookingService(
            IGetAllRepository<Booking> getAllRepository,
            IGetRepository<Booking> getRepository, 
            ICreateRepository<Booking> createRepository,
            IUpdateRepository<Booking> updateRepository)
        {
            _getAllRepository = getAllRepository;
            _getRepository = getRepository;
            _createRepository = createRepository;
            _updateRepository = updateRepository; 
        }
         
        public async Task ConsumeBookingsUpdateFromVehicleService()
        {
            Console.WriteLine($"Start Prepare for Messages from RabbitMq at : {DateTime.UtcNow}");

            //RabbitMq connection
            //Port : 5672
            var factory = new ConnectionFactory { HostName = "localhost" };

            using var connection = await factory.CreateConnectionAsync();

            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "messages",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            Console.WriteLine($"Waiting for messages from RabbitMq at : {DateTime.UtcNow}");

            var consumer = new AsyncEventingBasicConsumer(channel);
            try
            {
                consumer.ReceivedAsync += async (sender, eventArgs) =>
                {
                    byte[] body = eventArgs.Body.ToArray();

                    List<(Guid vehicleId, Guid userId)> viewedBookings =
                            JsonSerializer.Deserialize<List<(Guid vehicleId, Guid userId)>>(body);

                   await ((AsyncEventingBasicConsumer)sender).Channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
                }; 
            }
            catch (Exception ex) {

                Console.WriteLine($"Error while getting messages at : {DateTime.UtcNow} - exception : {ex.Message}");
            }
            await channel.BasicConsumeAsync("messages", autoAck: false, consumer);

            Console.WriteLine($"Messages was received from RabbitMq at : {DateTime.UtcNow}");
        }
    }
}
