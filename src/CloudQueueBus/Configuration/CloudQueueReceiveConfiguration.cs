//using System;
//using Microsoft.WindowsAzure.Storage.Queue;

//namespace CloudQueueBus.Configuration
//{
//    public class CloudQueueReceiveConfiguration : ICloudQueueReceiveConfiguration
//    {
//        public CloudQueueReceiveConfiguration(Uri address, TimeSpan delayBetweenIdleReceives, IObserver<IReceiveContext> observer, QueueRequestOptions queueRequestOptions, int batchCount, TimeSpan? visibilityTimeout, Type[] messages)
//        {
//            ReceiveAddress = address;
//            DelayBetweenIdleReceives = delayBetweenIdleReceives;
//            Observer = observer;
//            QueueRequestOptions = queueRequestOptions;
//            BatchCount = batchCount;
//            VisibilityTimeout = visibilityTimeout;
//            Messages = messages;
//        }

//        public Uri ReceiveAddress { get; private set; }
//        public TimeSpan DelayBetweenIdleReceives { get; private set; }
//        public IObserver<IReceiveContext> Observer { get; private set; }
//        public QueueRequestOptions QueueRequestOptions { get; private set; }
//        public int BatchCount { get; private set; }
//        public TimeSpan? VisibilityTimeout { get; private set; }
//        public Type[] Messages { get; private set; }

//        public ICloudQueueReceiverConfiguration ToReceiverConfiguration()
//        {
//            return new CloudQueueReceiverConfiguration(
//                ReceiveAddress,
//                DelayBetweenIdleReceives,
//                QueueRequestOptions,
//                BatchCount,
//                VisibilityTimeout);
//        }
//    }
//}