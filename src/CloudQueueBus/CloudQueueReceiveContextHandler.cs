using System;
using CloudQueueBus.Configuration;

namespace CloudQueueBus
{
    public class CloudQueueReceiveContextHandler : IHandler<IConfigureReceiveContext>
    {
        private readonly ISendContextSender _sender;
        private readonly IHandler<IReceiveContext> _next;
        private readonly Route[] _routes;

        public CloudQueueReceiveContextHandler(ISendContextSender sender, IHandler<IReceiveContext> next, Route[] routes)
        {
            if (sender == null) throw new ArgumentNullException("sender");
            if (next == null) throw new ArgumentNullException("next");
            if (routes == null) throw new ArgumentNullException("routes");
            _sender = sender;
            _next = next;
            _routes = routes;
        }

        public ISendContextSender Sender
        {
            get { return _sender; }
        }

        public IHandler<IReceiveContext> Next
        {
            get { return _next; }
        }

        public Route[] Routes
        {
            get { return _routes; }
        }

        public void Handle(IConfigureReceiveContext value)
        {
            if (value == null) throw new ArgumentNullException("value");
            Next.
                Handle(value.
                    SetSender(
                        new ReceiveContextSender(value, Sender, Routes)));
        }
    }
}