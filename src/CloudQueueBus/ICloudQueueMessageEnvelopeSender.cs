namespace CloudQueueBus
{
    public interface ICloudQueueMessageEnvelopeSender
    {
        void Send(IConfigureCloudQueueMessageEnvelope envelope);
    }
}