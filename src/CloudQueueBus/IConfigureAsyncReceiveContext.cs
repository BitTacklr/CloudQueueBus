using System;

namespace CloudQueueBus
{
    public interface IConfigureAsyncReceiveContext : IAsyncReceiveContext
    {
        IConfigureAsyncReceiveContext SetFrom(string value);
        IConfigureAsyncReceiveContext SetTo(string value);
        IConfigureAsyncReceiveContext SetMessageId(Guid value);
        IConfigureAsyncReceiveContext SetRelatesToMessageId(Guid? value);
        IConfigureAsyncReceiveContext SetCorrelationId(Guid value);
        IConfigureAsyncReceiveContext SetMessage(object value);
        IConfigureAsyncReceiveContext SetAsyncSender(IAsyncReceiveContextSender value);
    }
}