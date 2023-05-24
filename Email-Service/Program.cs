using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using ConsoleTables;
using System.Threading;

string HostName = "localhost";
string QueueName = "email_queue";

List<Message> mensajes = new List<Message>(); // Lista para almacenar los mensajes de la cola
int messageId = 1; // ID autonumérico para los mensajes
bool isReadingMessages = false;

var factory = new ConnectionFactory()
{
    HostName = HostName,
    UserName = "guest",
    Password = "guest"
};

using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    // Verificar si la cola existe
    var queueExists = false;
    try
    {
        var queueDeclareOk = channel.QueueDeclarePassive(QueueName);
        queueExists = true;
    }
    catch (Exception)
    {
        // La cola no existe
    }

    if (!queueExists)
    {
        Console.WriteLine("La cola especificada no existe. Se creará una nueva cola.");
        channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    // Leer los mensajes existentes en la cola
    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);

        // Agregar el mensaje a la lista
        mensajes.Add(new Message { Id = messageId++, Content = message, Timestamp = DateTime.Now });

        Console.WriteLine("Nuevo mensaje recibido: " + message);
        MostrarMensajesEnTabla();
    };
    channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);

    // Iniciar el hilo para leer los mensajes en segundo plano
    isReadingMessages = true; // Bandera para controlar el ciclo de lectura de mensajes

    var leerMensajesThread = new Thread(LeerMensajes);
    leerMensajesThread.Start();

    while (true)
    {
        Console.WriteLine("Ingrese un mensaje para agregar a la cola (o escriba 'salir' para finalizar):");
        string? input = Console.ReadLine();

        if (input?.ToLower() == "salir")
            break;

        // Mensaje a enviar
        string? message = input;

        // Convertir el mensaje a bytes
        var body = Encoding.UTF8.GetBytes(message ?? "");

        // Publicar el mensaje en la cola con propiedades persistentes
        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;
        channel.BasicPublish(exchange: "", routingKey: QueueName, basicProperties: properties, body: body);

        Console.WriteLine("Se ha enviado el mensaje a la cola.");

        // Crear el nuevo mensaje
        var timestamp = DateTime.Now;
        var newMessage = new Message { Id = messageId++, Content = message, Timestamp = timestamp };

        // Agregar el mensaje a la lista
        mensajes.Add(newMessage);

        Console.WriteLine("Mensajes en la cola:");
        MostrarMensajesEnTabla();
    }

    // Detener el hilo de lectura de mensajes
    isReadingMessages = false;
    leerMensajesThread.Join();
}

void LeerMensajes()
{
    var factory = new ConnectionFactory()
    {
        HostName = HostName,
        UserName = "guest",
        Password = "guest"
    };

    using (var connection = factory.CreateConnection())
    using (var channel = connection.CreateModel())
    {
        // Verificar si la cola existe
        var queueExists = false;
        try
        {
            var queueDeclareOk = channel.QueueDeclarePassive(QueueName);
            queueExists = true;
        }
        catch (Exception)
        {
            // La cola no existe
        }

        if (!queueExists)
        {
            Console.WriteLine("La cola especificada no existe. No se pueden leer los mensajes.");
            return;
        }

        // Leer los mensajes existentes en la cola
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            // Agregar el mensaje a la lista
            mensajes.Add(new Message { Id = messageId++, Content = message, Timestamp = DateTime.Now });

            Console.WriteLine("Nuevo mensaje recibido: " + message);
            MostrarMensajesEnTabla();
        };
        channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);

        // Mantener el hilo en ejecución mientras la bandera sea verdadera
        while (isReadingMessages)
        {
            Thread.Sleep(1000);
        }
    }
}

void MostrarMensajesEnTabla()
{
    // Crear la tabla
    var table = new ConsoleTable("ID", "Mensaje", "Hora");

    // Agregar filas a la tabla
    foreach (var msg in mensajes)
    {
        if (msg.Id == messageId - 1)
        {
            // Resaltar el último mensaje agregado en negrita y color verde
            table.AddRow($"\u001b[32m\u001b[1m{msg.Id}\u001b[0m", $"\u001b[32m\u001b[1m{msg.Content}\u001b[0m", $"\u001b[32m\u001b[1m{msg.Timestamp}\u001b[0m");
        }
        else
        {
            table.AddRow(msg.Id, msg.Content, msg.Timestamp);
        }
    }

    // Imprimir la tabla
    Console.WriteLine(table);
}
