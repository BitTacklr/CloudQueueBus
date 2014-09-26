using System;

namespace CloudQueueBus.Configuration
{
    public class Route
    {
        readonly Type _message;
        readonly string _queueName;

        public Route(Type message, string queueName)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (queueName == null) throw new ArgumentNullException("queueName");
            _message = message;
            _queueName = queueName;
        }

        public string QueueName
        {
            get { return _queueName; }
        }

        public Type Message
        {
            get { return _message; }
        }

        public bool Equals(Route other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other._message == _message && Equals(other._queueName, _queueName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Route)) return false;
            return Equals((Route)obj);
        }

        public override int GetHashCode()
        {
            return _message.GetHashCode() ^ _queueName.GetHashCode();
        }
    }
}