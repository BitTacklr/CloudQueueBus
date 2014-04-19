using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace CloudQueueBus.Configuration
{
    public class JsonFileBasedSubscriptionSource : ISubscriptionSource
    {
        private readonly JsonSerializer _serializer;
        private readonly string _subscriptionFile;

        public JsonFileBasedSubscriptionSource(JsonSerializer serializer, string subscriptionFile)
        {
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (String.IsNullOrEmpty(subscriptionFile)) throw new ArgumentNullException("subscriptionFile");
            _serializer = serializer;
            _subscriptionFile = subscriptionFile;
        }

        public JsonSerializer Serializer
        {
            get { return _serializer; }
        }

        public string SubscriptionFile
        {
            get { return _subscriptionFile; }
        }

        public IEnumerable<Subscription> Read()
        {
            using (var file = File.Open(SubscriptionFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var reader = new StreamReader(file))
                {
                    using (var jsonReader = new JsonTextReader(reader))
                    {
                        foreach (var data in Serializer.Deserialize<SubscriptionData[]>(jsonReader))
                        {
                            yield return new Subscription(
                                Type.GetType(data.Message, true, true),
                                data.Address);
                        }
                    }
                }
            }
        }

        [JsonObject(MemberSerialization.OptIn)]
        // ReSharper disable ClassNeverInstantiated.Local
        class SubscriptionData
        {
            // ReSharper restore ClassNeverInstantiated.Local
            [JsonProperty]
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            public String Message { get; set; }
            // ReSharper restore UnusedAutoPropertyAccessor.Local
            [JsonProperty]
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            public Uri Address { get; set; }
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }
    }
}