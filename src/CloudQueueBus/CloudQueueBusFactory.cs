using System;
using CloudQueueBus.Configuration;

namespace CloudQueueBus
{
    public static class CloudQueueBusFactory
    {
        public static CloudQueuePublisherBus NewPublisherBus(Action<ICloudQueuePublisherBusConfigurationBuilder> configure)
        {
            if (configure == null) throw new ArgumentNullException("configure");
            var builder = new CloudQueuePublisherBusConfigurationBuilder();
            configure(builder);
            var configuration = builder.Build();
            var sender = new SendContextSender(
                new CloudQueueMessageEnvelopeSender(
                    new CloudQueueSender(
                        new CloudQueuePool(
                            configuration.StorageAccount.CreateCloudQueueClient()), 
                            configuration.SenderConfiguration),
                    new CloudQueueMessageEnvelopeJsonWriter()),
                new MessageTypeResolver(), 
                configuration.Serializer);
            return new CloudQueuePublisherBus(configuration, sender);
        }

        public static CloudQueueServerBus NewServerBus(Action<ICloudQueueServerBusConfigurationBuilder> configure)
        {
            if (configure == null) throw new ArgumentNullException("configure");
            var builder = new CloudQueueServerBusConfigurationBuilder();
            configure(builder);
            var configuration = builder.Build();
            var pool = new CloudQueuePool(
                configuration.StorageAccount.CreateCloudQueueClient());
            var sender = new SendContextSender(
                new CloudQueueMessageEnvelopeSender(
                    new CloudQueueSender(
                        new CloudQueuePool(configuration.StorageAccount.CreateCloudQueueClient()),
                        configuration.SenderConfiguration),
                    new CloudQueueMessageEnvelopeJsonWriter()),
                new MessageTypeResolver(),
                configuration.Serializer);
            var receiver = new CloudQueueReceiver(
                pool,
                new CloudQueueMessageObserver(
                    new CloudQueueMessageEnvelopeJsonReader(),
                    new CloudQueueMessageEnvelopeObserver(
                        new MessageContentTypeResolver(configuration.Messages),
                        configuration.Serializer,
                        new CloudQueueReceiveContextObserver(
                            sender,
                            configuration.Observer, 
                            configuration.Routes))),
                configuration.ReceiverConfiguration);
            return new CloudQueueServerBus(configuration, receiver);
        }

        public static CloudQueueSendOnlyBus NewSendOnlyBus(Action<ICloudQueueSendOnlyBusConfigurationBuilder> configure)
        {
            if (configure == null) throw new ArgumentNullException("configure");
            var builder = new CloudQueueSendOnlyBusConfigurationBuilder();
            configure(builder);
            var configuration = builder.Build();
            var pool = new CloudQueuePool(
                configuration.StorageAccount.CreateCloudQueueClient());
            var sender = new SendContextSender(
                new CloudQueueMessageEnvelopeSender(
                    new CloudQueueSender(
                        pool,
                        configuration.SenderConfiguration),
                    new CloudQueueMessageEnvelopeJsonWriter()),
                new MessageTypeResolver(),
                configuration.Serializer);
            return new CloudQueueSendOnlyBus(configuration, sender);
        }
    }
}