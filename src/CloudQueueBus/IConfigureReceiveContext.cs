using System;

namespace CloudQueueBus
{
    public interface IConfigureReceiveContext : IReceiveContext
    {
        IConfigureReceiveContext SetFrom(string value);
        IConfigureReceiveContext SetTo(string value);
        IConfigureReceiveContext SetMessageId(Guid value);
        IConfigureReceiveContext SetRelatesToMessageId(Guid? value);
        IConfigureReceiveContext SetCorrelationId(Guid value);
        IConfigureReceiveContext SetMessage(object value);
        IConfigureReceiveContext SetSender(IReceiveContextSender value);
    }
}