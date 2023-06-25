// See https://aka.ms/new-console-template for more information
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Amqp.Framing;

async Task MessageHandler(ProcessMessageEventArgs args)
{
    var body = args.Message.Body.ToString();
    Console.WriteLine($"Received: {body}");
}

async Task MessageErrorHandler(ProcessErrorEventArgs args)
{
    Console.WriteLine(args.Exception)
}


var queueName = "sbq-bo-az-partpurchase";

var SBConnectionStr = "Endpoint=sb://sb-bo-az-dev.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=dyfzhU1RpZ+6TI2nce9ZsA5Ggytfq3m9b+ASbJbEA/s=;EntityPath" + queueName;

var SBClientOption = new ServiceBusClientOptions()
{
    TransportType = ServiceBusTransportType.AmqpWebSockets
};

//Create Connection
await using var client = new ServiceBusClient(SBConnectionStr, SBClientOption);

//Receive via Processing
//Create Processor
ServiceBusProcessor processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());

processor.ProcessMessageAsync += MessageHandler;

//static async Task Main(string[] args)
//{

//Receive without Processing
//Create Receiver
//ServiceBusReceiver receiver = client.CreateReceiver(queueName);
//Receive Message
//ServiceBusReceivedMessage message = await receiver.ReceiveMessageAsync();


//Console.WriteLine
//        ("Select Process" +
//        "\n(1) Receive and Complete" +
//        "\n(2) Receive but Abort" +
//        "\n(3) Defer and Receive" +
//        "\n(4) Put it to Deadletter and Receive" +
//        "\n(X) Exit"
//        );

//var response = Console.ReadLine() ?? string.Empty;

//while (response.ToUpper() != "X")
//{
//    switch (response)
//    {
//        case "1":
//            //Complete Message (so mq will remove it from queue)
//            await receiver.CompleteMessageAsync(message);
//            Console.WriteLine(message.Body.ToString());
//            break;
//        case "2":
//            //Quit processing (so mq will not remove it from queue)
//            await receiver.AbandonMessageAsync(message);
//            break;
//        case "3":
//            //Quit processing (so mq will not remove it from queue but it will be available for reprocessing)
//            //Keep message.SequenceNumber number to read it in future
//            await receiver.DeferMessageAsync(message);
//            //Read defered messager
//            ServiceBusReceivedMessage defMessage = await receiver.ReceiveDeferredMessageAsync(message.SequenceNumber);
//            Console.WriteLine(defMessage.Body.ToString());
//            break;
//        case "4":
//            //Put message to Deadletter
//            await receiver.DeadLetterMessageAsync(message, "reason", "description");
//            //Receive deadletter
//            ServiceBusReceiver deatletterqueue = client.CreateReceiver("sbq-bo-az-partpurchase", new ServiceBusReceiverOptions
//            {
//                SubQueue = SubQueue.DeadLetter
//            });
//            ServiceBusReceivedMessage deadletter = await deatletterqueue.ReceiveMessageAsync();
//            Console.WriteLine($"{deadletter.Body} {deadletter.DeadLetterReason}");
//            break;
//        default:
//            Console.WriteLine("Quitting");
//            break;
//    }
//}