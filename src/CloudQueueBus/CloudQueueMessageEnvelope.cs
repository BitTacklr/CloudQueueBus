using System;

namespace CloudQueueBus
{
    public class CloudQueueMessageEnvelope : IConfigureCloudQueueMessageEnvelope
    {
        public string From { get; private set; }
        public string To { get; private set; }
        public Guid MessageId { get; private set; }
        public Guid? RelatesToMessageId { get; private set; }
        public Guid CorrelationId { get; private set; }
        public string ContentType { get; private set; }
        public byte[] Content { get; private set; }
        public DateTimeOffset Time { get; private set; }

        public CloudQueueMessageEnvelope()
        {
            Content = new byte[0];
        }

        CloudQueueMessageEnvelope(string @from, string to, Guid messageId, Guid? relatesToMessageId, Guid correlationId, string contentType, byte[] content, DateTimeOffset time)
        {
            From = @from;
            To = to;
            MessageId = messageId;
            RelatesToMessageId = relatesToMessageId;
            CorrelationId = correlationId;
            ContentType = contentType;
            Content = content;
            Time = time;
        }

        public IConfigureCloudQueueMessageEnvelope SetFrom(string value)
        {
            return new CloudQueueMessageEnvelope(value, To, MessageId, RelatesToMessageId, CorrelationId, ContentType, Content, Time);
        }

        public IConfigureCloudQueueMessageEnvelope SetTo(string value)
        {
            return new CloudQueueMessageEnvelope(From, value, MessageId, RelatesToMessageId, CorrelationId, ContentType, Content, Time);
        }

        public IConfigureCloudQueueMessageEnvelope SetMessageId(Guid value)
        {
            return new CloudQueueMessageEnvelope(From, To, value, RelatesToMessageId, CorrelationId, ContentType, Content, Time);
        }

        public IConfigureCloudQueueMessageEnvelope SetRelatesToMessageId(Guid? value)
        {
            return new CloudQueueMessageEnvelope(From, To, MessageId, value, CorrelationId, ContentType, Content, Time);
        }

        public IConfigureCloudQueueMessageEnvelope SetCorrelationId(Guid value)
        {
            return new CloudQueueMessageEnvelope(From, To, MessageId, RelatesToMessageId, value, ContentType, Content, Time);
        }

        public IConfigureCloudQueueMessageEnvelope SetContentType(string value)
        {
            return new CloudQueueMessageEnvelope(From, To, MessageId, RelatesToMessageId, CorrelationId, value, Content, Time);
        }

        public IConfigureCloudQueueMessageEnvelope SetContent(byte[] value)
        {
            return new CloudQueueMessageEnvelope(From, To, MessageId, RelatesToMessageId, CorrelationId, ContentType, value, Time);
        }

        public IConfigureCloudQueueMessageEnvelope SetTime(DateTimeOffset value)
        {
            return new CloudQueueMessageEnvelope(From, To, MessageId, RelatesToMessageId, CorrelationId, ContentType, Content, value);
        }

        public IConfigureCloudBlobMessageEnvelope ToBlobEnvelope(Guid blobId)
        {
            return new CloudBlobMessageEnvelope().
                SetBlobId(blobId).
                SetMessageId(MessageId).
                SetContentType(ContentType).
                SetContent(Content).
                SetTime(Time);
        }
    }
}