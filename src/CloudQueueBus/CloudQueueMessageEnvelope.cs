using System;

namespace CloudQueueBus
{
    public class CloudQueueMessageEnvelope : IConfigureCloudQueueMessageEnvelope
    {
        public Uri From { get; private set; }
        public Uri To { get; private set; }
        public Guid MessageId { get; private set; }
        public Guid? RelatesToMessageId { get; private set; }
        public Guid CorrelationId { get; private set; }
        public string ContentType { get; private set; }
        public byte[] Content { get; private set; }

        public void SetFrom(Uri value)
        {
            From = value;
        }

        public void SetTo(Uri value)
        {
            To = value;
        }

        public void SetMessageId(Guid value)
        {
            MessageId = value;
        }

        public void SetRelatesToMessageId(Guid? value)
        {
            RelatesToMessageId = value;
        }

        public void SetCorrelationId(Guid value)
        {
            CorrelationId = value;
        }

        public void SetContentType(string value)
        {
            ContentType = value;
        }

        public void SetContent(byte[] value)
        {
            Content = value;
        }
    }
}