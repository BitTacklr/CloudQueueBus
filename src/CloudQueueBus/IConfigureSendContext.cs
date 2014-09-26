using System;

namespace CloudQueueBus
{
    public interface IConfigureSendContext : ISendContext
    {
        IConfigureSendContext SetFrom(string value);
        IConfigureSendContext SetTo(string value);
        IConfigureSendContext SetMessageId(Guid value);
        IConfigureSendContext SetRelatesToMessageId(Guid? value);
        IConfigureSendContext SetCorrelationId(Guid value);
        IConfigureSendContext SetMessage(object value);
    }
}