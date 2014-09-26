using System;

namespace CloudQueueBus
{
    public class CloudBlobMessageEnvelope : IConfigureCloudBlobMessageEnvelope
    {
        public Guid BlobId { get; private set; }
        public Guid MessageId { get; private set; }
        public string ContentType { get; private set; }
        public byte[] Content { get; private set; }
        public DateTimeOffset Time { get; private set; }

        public CloudBlobMessageEnvelope()
        {
            Content = new byte[0];
        }

        CloudBlobMessageEnvelope(Guid blobId, Guid messageId, string contentType, byte[] content, DateTimeOffset time)
        {
            BlobId = blobId;
            MessageId = messageId;
            ContentType = contentType;
            Content = content;
            Time = time;
        }

        public IConfigureCloudBlobMessageEnvelope SetBlobId(Guid value)
        {
            return new CloudBlobMessageEnvelope(value, MessageId, ContentType, Content, Time);
        }

        public IConfigureCloudBlobMessageEnvelope SetMessageId(Guid value)
        {
            return new CloudBlobMessageEnvelope(BlobId, value, ContentType, Content, Time);
        }

        public IConfigureCloudBlobMessageEnvelope SetContentType(string value)
        {
            return new CloudBlobMessageEnvelope(BlobId, MessageId, value, Content, Time);
        }

        public IConfigureCloudBlobMessageEnvelope SetContent(byte[] value)
        {
            return new CloudBlobMessageEnvelope(BlobId, MessageId, ContentType, value, Time);
        }

        public IConfigureCloudBlobMessageEnvelope SetTime(DateTimeOffset value)
        {
            return new CloudBlobMessageEnvelope(BlobId, MessageId, ContentType, Content, value);
        }
    }
}