using Newtonsoft.Json;

namespace CloudQueueBus
{
    public interface ICloudQueueMessageEnvelopeJsonReader
    {
        IConfigureCloudQueueMessageEnvelope Read(JsonReader reader);
    }
}