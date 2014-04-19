using System;
using System.Linq;
using CloudQueueBus.Configuration;

namespace CloudQueueBus
{
    public class ReceiveContextSender : IReceiveContextSender
    {
        private readonly IReceiveContext _context;
        private readonly ISendContextSender _innerSender;
        private readonly Route[] _routes;

        public ReceiveContextSender(IReceiveContext context, ISendContextSender innerSender, Route[] routes)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (innerSender == null) throw new ArgumentNullException("innerSender");
            if (routes == null) throw new ArgumentNullException("routes");
            _context = context;
            _innerSender = innerSender;
            _routes = routes;
        }

        public ISendContextSender InnerSender
        {
            get { return _innerSender; }
        }

        public IReceiveContext ReceiveContext
        {
            get { return _context; }
        }

        public Route[] Routes
        {
            get { return _routes; }
        }

        public void Send(Guid id, object message)
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

            InnerSender.Send(context);
        }

        private Uri GetMessageAddress(Type message)
        {
            var route = Routes.FirstOrDefault(_ => _.Message == message);
            return route != null ? route.Address : null;
        }
    }
}