namespace CloudQueueBus
{
    public interface ICloudBlobMessageEnvelopeWriter
    {
        void Write(IConfigureCloudBlobMessageEnvelope envelope);
    }
}