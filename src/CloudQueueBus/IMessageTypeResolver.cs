namespace CloudQueueBus
{
    public interface IMessageTypeResolver
    {
        string GetContentType(object message);
    }
}