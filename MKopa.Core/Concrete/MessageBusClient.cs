using Microsoft.Extensions.Configuration;
using MKopa.Core.Abstract;
using MKopa.Core.Dtos;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MKopa.Core.Concrete
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory() { HostName = _configuration["RabbitMQHost"], Port = int.Parse(_configuration["RabbitMQPort"]) };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("--> Connected to Mesaage Bus");
            }
            catch (Exception ex)
            {

                Console.WriteLine($"--> Could not connect to the Message Bus: {ex.Message}");
            }
        }
        public void PublishNewSendSMS(PublishedDto publishedDto)
        {
            var message = JsonSerializer.Serialize(publishedDto);
            if (_connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ Connection Open, seending message");
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("--> RabbitMQ Connection closed, not seending message");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "trigger", routingKey: "", basicProperties: null, body: body);
            Console.WriteLine($"--> We have sent {message}");
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }

        public void Dispose()
        {
            Console.WriteLine("--> Message Disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
    }
}
