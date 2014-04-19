namespace CloudQueueBus
{
    public interface ISendContextSender
    {
        void Send(IConfigureSendContext context);
    }
}