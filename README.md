# Email Service

Este repositorio contiene un fragmento de código que demuestra cómo enviar un correo electrónico utilizando RabbitMQ. El código está escrito en C# y utiliza la biblioteca RabbitMQ.Client.

## Prerrequisitos

Antes de ejecutar el código, asegúrate de tener lo siguiente:

- RabbitMQ instalado y en funcionamiento localmente.
- La biblioteca RabbitMQ.Client agregada a tu proyecto.

## Configuración

1. Establece la variable `HostName` con el nombre del servidor RabbitMQ. Si se ejecuta localmente, puedes dejarlo como "localhost".
2. Establece la variable `QueueName` con el nombre de la cola de correo electrónico que deseas utilizar.
3. Asegúrate de que la cola esté declarada en RabbitMQ utilizando los siguientes parámetros:
   - Cola: `QueueName`
   - Durabilidad: `false`
   - Exclusiva: `false`
   - Eliminación automática: `false`
   - Argumentos: `null`

## Envío de un correo electrónico

El fragmento de código demuestra cómo enviar un correo electrónico publicando un mensaje en la cola de RabbitMQ.

1. Convierte el mensaje de correo electrónico a bytes utilizando la codificación UTF-8.
2. Establece una conexión con RabbitMQ utilizando el `HostName` proporcionado.
3. Crea un canal dentro de la conexión.
4. Declara la cola utilizando los parámetros proporcionados.
5. Publica el mensaje de correo electrónico en la cola utilizando el método `BasicPublish`.
6. Muestra un mensaje de confirmación una vez que el correo electrónico se haya enviado a la cola.

## Ejecución del código

1. Asegúrate de que el servidor RabbitMQ esté en ejecución.
2. Compila y ejecuta el código.
3. Una vez ejecutado, el mensaje de correo electrónico se enviará a la cola de RabbitMQ especificada.
4. La consola mostrará un mensaje confirmando que el correo electrónico se ha enviado a la cola.
5. Presiona cualquier tecla para salir del programa.

Siéntete libre de modificar el código según tus requisitos. ¡Feliz codificación!

## Tags

- RabbitMQ
- C#
- Correo electrónico
- Cola
- Mensajería
