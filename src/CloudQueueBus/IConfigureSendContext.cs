using System;

namespace CloudQueueBus
{
    public interface IConfigureSendContext : ISendContext
    {
        IConfigureSendContext SetFrom(Uri value);
        IConfigureSendContext SetTo(Uri value);
        IConfigureSendContext SetMessageId(Guid value);
        IConfigureSendContext SetRelatesToMessageId(Guid? value);
        IConfigureSendContext SetCorrelationId(Guid value);
        IConfigureSendContext SetMessage(object value);
    }
}