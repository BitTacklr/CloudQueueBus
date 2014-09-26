using System;

namespace CloudQueueBus
{
    public interface IConfigureCloudQueueMessageEnvelope : ICloudQueueMessageEnvelope
    {
        IConfigureCloudQueueMessageEnvelope SetFrom(string value);
        IConfigureCloudQueueMessageEnvelope SetTo(string value);
        IConfigureCloudQueueMessageEnvelope SetMessageId(Guid value);
        IConfigureCloudQueueMessageEnvelope SetRelatesToMessageId(Guid? value);
        IConfigureCloudQueueMessageEnvelope SetCorrelationId(Guid value);
        IConfigureCloudQueueMessageEnvelope SetContentType(string value);
        IConfigureCloudQueueMessageEnvelope SetContent(byte[] value);
        IConfigureCloudQueueMessageEnvelope SetTime(DateTimeOffset value);
    }
}