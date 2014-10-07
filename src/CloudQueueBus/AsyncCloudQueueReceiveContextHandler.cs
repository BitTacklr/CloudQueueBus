using System;
using System.Threading;
using System.Threading.Tasks;
using CloudQueueBus.Configuration;

namespace CloudQueueBus
{
    public class AsyncCloudQueueReceiveContextHandler : IAsyncHandler<IConfigureAsyncReceiveContext>
    {
        private readonly ISendContextSender _sender;
        private readonly IAsyncSendContextSender _asyncSender;
        private readonly IAsyncHandler<IAsyncReceiveContext> _next;
        private readonly Route[] _routes;

        public AsyncCloudQueueReceiveContextHandler(ISendContextSender sender, IAsyncSendContextSender asyncSender, IAsyncHandler<IAsyncReceiveContext> next, Route[] routes)
        {
            if (sender == null) throw new ArgumentNullException("sender");
            if (asyncSender == null) throw new ArgumentNullException("asyncSender");
            if (next == null) throw new ArgumentNullException("next");
            if (routes == null) throw new ArgumentNullException("routes");
            _sender = sender;
            _asyncSender = asyncSender;
            _next = next;
            _routes = routes;
        }

        public ISendContextSender Sender
        {
            get { return _sender; }
        }

        public IAsyncHandler<IAsyncReceiveContext> Next
        {
            get { return _next; }
        }

        public Route[] Routes
        {
            get { return _routes; }
        }

        public IAsyncSendContextSender AsyncSender
        {
            get { return _asyncSender; }
        }

        public Task HandleAsync(IConfigureAsyncReceiveContext value)
        {
            return HandleAsync(value, CancellationToken.None);
        }

        public Task HandleAsync(IConfigureAsyncReceiveContext value, CancellationToken cancellationToken)
        {
            if (value == null) throw new ArgumentNullException("value");
            return Next.
                HandleAsync(value.
                    SetAsyncSender(
                        new AsyncReceiveContextSender(value, AsyncSender, Routes)),
                    cancellationToken);
        }
    }
}