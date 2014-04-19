using System;

namespace CloudQueueBus.Configuration
{
    public class Subscription
    {
        readonly Type _message;
        readonly Uri _address;

        public Subscription(Type message, Uri address)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (address == null) throw new ArgumentNullException("address");
            _message = message;
            _address = address;
        }

        public Uri Address
        {
            get { return _address; }
        }

        public Type Message
        {
            get { return _message; }
        }

        public bool Equals(Subscription other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other._message == _message && Equals(other._address, _address);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Subscription)) return false;
            return Equals((Subscription)obj);
        }

        public override int GetHashCode()
        {
            return _message.GetHashCode() ^ _address.GetHashCode();
        }
    }
}