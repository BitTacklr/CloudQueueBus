using System;
using Newtonsoft.Json;

namespace CloudQueueBus
{
    public class CloudQueueMessageEnvelopeJsonReader : ICloudQueueMessageEnvelopeJsonReader
    {
        public IConfigureCloudQueueMessageEnvelope Read(JsonReader reader)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            IConfigureCloudQueueMessageEnvelope envelope = new CloudQueueMessageEnvelope();
            reader.Read(); //StartObject
            reader.Read(); //PropertyName:From
            envelope.SetFrom(new Uri(reader.ReadAsString())); //String:From value
            reader.Read(); //PropertyName:To
            envelope.SetTo(new Uri(reader.ReadAsString())); //String:To value
            reader.Read(); //PropertyName:MessageId
            envelope.SetMessageId(new Guid(reader.ReadAsString())); //String:MessageId value
            reader.Read();
            if (((string)reader.Value) == "RelatesToMessageId") //PropertyName:Possibly RelatesToMessageId
            {
                envelope.SetRelatesToMessageId(new Guid(reader.ReadAsString()));
                reader.Read(); //PropertyName:CorrelationId
            }
            envelope.SetCorrelationId(new Guid(reader.ReadAsString())); //String:CorrelationId value
            reader.Read(); //PropertyName:ContentType
            envelope.SetContentType(reader.ReadAsString()); //String:ContentType value
            reader.Read(); //PropertyName:Content
            envelope.SetContent(reader.ReadAsBytes()); //String:Content value
            reader.Read(); //EndObject
            return envelope;
        }
    }
}