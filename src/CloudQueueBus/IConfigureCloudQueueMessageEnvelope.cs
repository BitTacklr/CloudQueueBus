using System;

namespace CloudQueueBus
{
    public interface IConfigureCloudQueueMessageEnvelope : ICloudQueueMessageEnvelope
    {
        void SetFrom(Uri value);
        void SetTo(Uri value);
        void SetMessageId(Guid value);
        void SetRelatesToMessageId(Guid? value);
        void SetCorrelationId(Guid value);
        void SetContentType(string value);
        void SetContent(byte[] value);
    }
}