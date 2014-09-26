using System;

namespace CloudQueueBus
{
    public interface IConfigureCloudBlobMessageEnvelope : ICloudBlobMessageEnvelope
    {
        IConfigureCloudBlobMessageEnvelope SetBlobId(Guid value);
        IConfigureCloudBlobMessageEnvelope SetMessageId(Guid value);
        IConfigureCloudBlobMessageEnvelope SetContentType(string value);
        IConfigureCloudBlobMessageEnvelope SetContent(byte[] value);
        IConfigureCloudBlobMessageEnvelope SetTime(DateTimeOffset value);
    }
}