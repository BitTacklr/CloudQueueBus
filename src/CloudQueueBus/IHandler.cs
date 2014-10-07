namespace CloudQueueBus
{
    public interface IHandler<in T>
    {
        void Handle(T value);
    }
}