using System;
using System.IO;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace CloudQueueBus
{
    public class CloudQueueMessageObserver : IObserver<CloudQueueMessage>
    {
        private readonly ICloudQueueMessageEnvelopeJsonReader _reader;
        private readonly IObserver<ICloudQueueMessageEnvelope> _observer;

        public CloudQueueMessageObserver(ICloudQueueMessageEnvelopeJsonReader reader, IObserver<ICloudQueueMessageEnvelope> observer)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (observer == null) throw new ArgumentNullException("observer");
            _reader = reader;
            _observer = observer;
        }

        public IObserver<ICloudQueueMessageEnvelope> Observer
        {
            get { return _observer; }
        }

        public ICloudQueueMessageEnvelopeJsonReader Reader
        {
            get { return _reader; }
        }

        public void OnNext(CloudQueueMessage value)
        {
            using (var stream = new MemoryStream(value.AsBytes))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    using (var jsonReader = new JsonTextReader(streamReader))
                    {
                        var envelope = Reader.Read(jsonReader);
                        //if (envelope.ContentType == "BlobReference")
                        //{
                        //    //Message content is stored in blob storage
                        //    envelope.SetContentType("what-we-have-read-from-the-blob");
                        //    envelope.SetContent(new byte[0]);
                        //}
                        Observer.OnNext(envelope);
                    }
                }
            }
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
