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
            var queuePool = new CloudQueuePool(
                configuration.StorageAccount.CreateCloudQueueClient());
            var blobContainerPool = new CloudBlobContainerPool(
                configuration.StorageAccount.CreateCloudBlobClient());
            var sender = new SendContextSender(
                new CloudQueueMessageEnvelopeSender(
                    new CloudQueueSender(queuePool, configuration.SenderConfiguration),
                    new CloudQueueMessageEnvelopeJsonWriter(),
                    new CloudBlobMessageEnvelopeWriter(blobContainerPool, configuration.OverflowBlobContainerName)),
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
            var queuePool = new CloudQueuePool(
                configuration.StorageAccount.CreateCloudQueueClient());
            var blobContainerPool = new CloudBlobContainerPool(
                configuration.StorageAccount.CreateCloudBlobClient());
            var sender = new SendContextSender(
                new CloudQueueMessageEnvelopeSender(
                    new CloudQueueSender(
                        queuePool,
                        configuration.SenderConfiguration),
                    new CloudQueueMessageEnvelopeJsonWriter(),
                    new CloudBlobMessageEnvelopeWriter(
                        blobContainerPool, configuration.OverflowBlobContainerName)),
                new MessageTypeResolver(),
                configuration.Serializer);
            var receiver = new CloudQueueReceiver(
                queuePool,
                new CloudQueueMessageObserver(
                    new CloudQueueMessageEnvelopeJsonReader(),
                    new CloudBlobMessageEnvelopeReader(blobContainerPool, configuration.OverflowBlobContainerName), 
                    new CloudQueueMessageEnvelopeObserver(
                        new MessageContentTypeResolver(configuration.Messages),
                        configuration.Serializer,
                        new CloudQueueReceiveContextObserver(
                            sender,
                            configuration.Observer, 
                            configuration.Routes))),
                configuration.ReceiverConfiguration,
                configuration.ErrorConfiguration);
            return new CloudQueueServerBus(configuration, receiver);
        }

        public static CloudQueueSendOnlyBus NewSendOnlyBus(Action<ICloudQueueSendOnlyBusConfigurationBuilder> configure)
        {
            if (configure == null) throw new ArgumentNullException("configure");
            var builder = new CloudQueueSendOnlyBusConfigurationBuilder();
            configure(builder);
            var configuration = builder.Build();
            var queuePool = new CloudQueuePool(
                configuration.StorageAccount.CreateCloudQueueClient());
            var blobContainerPool = new CloudBlobContainerPool(
                configuration.StorageAccount.CreateCloudBlobClient());
            var sender = new SendContextSender(
                new CloudQueueMessageEnvelopeSender(
                    new CloudQueueSender(queuePool, configuration.SenderConfiguration),
                    new CloudQueueMessageEnvelopeJsonWriter(),
                    new CloudBlobMessageEnvelopeWriter(blobContainerPool, configuration.OverflowBlobContainerName)),
                new MessageTypeResolver(),
                configuration.Serializer);
            return new CloudQueueSendOnlyBus(configuration, sender);
        }
    }
}