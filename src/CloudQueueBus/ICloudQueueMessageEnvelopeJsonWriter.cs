using Newtonsoft.Json;

namespace CloudQueueBus
{
    public interface ICloudQueueMessageEnvelopeJsonWriter
    {
        void Write(IConfigureCloudQueueMessageEnvelope envelope, JsonWriter writer);
    }
}