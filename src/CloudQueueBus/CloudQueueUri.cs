using System;

namespace CloudQueueBus
{
    public class CloudQueueUri
    {
        private readonly Uri _base;
        private readonly string _name;

        public CloudQueueUri(Uri @base, string name)
        {
            if (@base == null) throw new ArgumentNullException("base");
            if (name == null) throw new ArgumentNullException("name");
            _base = @base;
            _name = name;
        }

        public Uri Base
        {
            get { return _base; }
        }

        public string Name
        {
            get { return _name; }
        }

        public Uri Uri
        {
            get { return new Uri(Base,Name); }
        }

        public static CloudQueueUri ParseUsing(Uri @base, Uri address)
        {
            return new CloudQueueUri(@base, @base.MakeRelativeUri(address).OriginalString);
        }
    }
}