// See https://aka.ms/new-console-template for more information
using Azure.Messaging.ServiceBus;

//Create Connection
await using var client = new ServiceBusClient("Endpoint=sb://sb-bo-az-dev.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=dyfzhU1RpZ+6TI2nce9ZsA5Ggytfq3m9b+ASbJbEA/s=;EntityPath=sbq-bo-az-partpurchase");

//Create Sender
ServiceBusSender sender = client.CreateSender("sbq-bo-az-partpurchase");

string response = string.Empty;

while (response.ToUpper() != "X")
{
    //Send Message
    Console.WriteLine("Please write your message or Enter X to exit");

    response = Console.ReadLine();

    await sender.SendMessageAsync(new ServiceBusMessage(response));

}

Console.WriteLine("Bye Bye");

Console.ReadLine();