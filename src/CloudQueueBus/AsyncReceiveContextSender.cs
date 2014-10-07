using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CloudQueueBus.Configuration;

namespace CloudQueueBus
{
    public class AsyncReceiveContextSender : IAsyncReceiveContextSender
    {
        private readonly IAsyncReceiveContext _context;
        private readonly IAsyncSendContextSender _innerSender;
        private readonly Route[] _routes;

        public AsyncReceiveContextSender(IAsyncReceiveContext context, IAsyncSendContextSender innerSender, Route[] routes)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (innerSender == null) throw new ArgumentNullException("innerSender");
            if (routes == null) throw new ArgumentNullException("routes");
            _context = context;
            _innerSender = innerSender;
            _routes = routes;
        }

        public IAsyncSendContextSender InnerSender
        {
            get { return _innerSender; }
        }

        public IAsyncReceiveContext ReceiveContext
        {
            get { return _context; }
        }

        public Route[] Routes
        {
            get { return _routes; }
        }

        public Task SendAsync(Guid id, object message)
        {
            return SendAsync(id, message, CancellationToken.None);
        }

        public Task SendAsync(Guid id, object message, CancellationToken cancellationToken)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            var context = new SendContext().
                SetFrom(ReceiveContext.To).
                SetTo(GetMessageAddress(message.GetType())).
                SetCorrelationId(ReceiveContext.CorrelationId).
                SetMessageId(id).
                SetRelatesToMessageId(ReceiveContext.MessageId).
                SetMessage(message);

            return InnerSender.SendAsync(context, cancellationToken);
        }

        private string GetMessageAddress(Type message)
        {
            var route = Routes.FirstOrDefault(_ => _.Message == message);
            return route != null ? route.QueueName : null;
        }
    }
}