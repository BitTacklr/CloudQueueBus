using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace CloudQueueBus.Configuration
{
    public class JsonFileBasedRouteSource : IRouteSource
    {
        private readonly JsonSerializer _serializer;
        private readonly string _routeFile;

        public JsonFileBasedRouteSource(JsonSerializer serializer, string routeFile)
        {
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (String.IsNullOrEmpty(routeFile)) throw new ArgumentNullException("routeFile");
            _serializer = serializer;
            _routeFile = routeFile;
        }

        public JsonSerializer Serializer
        {
            get { return _serializer; }
        }

        public string RouteFile
        {
            get { return _routeFile; }
        }

        public IEnumerable<Route> Read()
        {
            using (var file = File.Open(RouteFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var reader = new StreamReader(file))
                {
                    using (var jsonReader = new JsonTextReader(reader))
                    {
                        foreach (var data in Serializer.Deserialize<RouteData[]>(jsonReader))
                        {
                            yield return new Route(
                                Type.GetType(data.Message, true, true),
                                data.Address);
                        }
                    }
                }
            }
        }

        [JsonObject(MemberSerialization.OptIn)]
        // ReSharper disable ClassNeverInstantiated.Local
        class RouteData
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