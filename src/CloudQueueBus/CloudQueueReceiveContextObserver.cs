using System;
using CloudQueueBus.Configuration;

namespace CloudQueueBus
{
    public class CloudQueueReceiveContextObserver : IObserver<IConfigureReceiveContext>
    {
        private readonly ISendContextSender _sender;
        private readonly IObserver<IReceiveContext> _observer;
        private readonly Route[] _routes;

        public CloudQueueReceiveContextObserver(ISendContextSender sender, IObserver<IReceiveContext> observer, Route[] routes)
        {
            if (sender == null) throw new ArgumentNullException("sender");
            if (observer == null) throw new ArgumentNullException("observer");
            if (routes == null) throw new ArgumentNullException("routes");
            _sender = sender;
            _observer = observer;
            _routes = routes;
        }

        public ISendContextSender Sender
        {
            get { return _sender; }
        }

        public IObserver<IReceiveContext> Observer
        {
            get { return _observer; }
        }

        public Route[] Routes
        {
            get { return _routes; }
        }

        public void OnNext(IConfigureReceiveContext value)
        {
            Observer.
                OnNext(value.
                    SetSender(
                        new ReceiveContextSender(value, Sender, Routes)));
        }

        public void OnError(Exception error)
        {
            Observer.OnError(error);
        }

        public void OnCompleted()
        {
            Observer.OnCompleted();
        }
    }
}