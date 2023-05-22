using RabbitMQ.Client;
using System.Text;

string HostName = "localhost";
string QueueName = "email_queue";

var factory = new ConnectionFactory() { HostName = HostName };
using (var connection = factory.CreateConnection())
{
    using (var channel = connection.CreateModel())
    {
        // Declarar la cola de RabbitMQ
        channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        // Mensaje a enviar
        string message = "¡Hola! Esto es un ejemplo de correo electrónico.";

        // Convertir el mensaje a bytes
        var body = Encoding.UTF8.GetBytes(message);

        // Publicar el mensaje en la cola
        channel.BasicPublish(exchange: "", routingKey: QueueName, basicProperties: null, body: body);

        Console.WriteLine("Se ha enviado el correo electrónico a la cola.");
        
    }
}

 Console.ReadLine();
