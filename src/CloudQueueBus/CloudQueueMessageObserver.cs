using System;
using System.IO;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace CloudQueueBus
{
    public class CloudQueueMessageObserver : IObserver<CloudQueueMessage>
    {
        private readonly ICloudQueueMessageEnvelopeJsonReader _queueReader;
        private readonly ICloudBlobMessageEnvelopeReader _blobReader;
        private readonly IObserver<ICloudQueueMessageEnvelope> _observer;

        public CloudQueueMessageObserver(ICloudQueueMessageEnvelopeJsonReader queueReader, ICloudBlobMessageEnvelopeReader blobReader, IObserver<ICloudQueueMessageEnvelope> observer)
        {
            if (queueReader == null) throw new ArgumentNullException("queueReader");
            if (blobReader == null) throw new ArgumentNullException("blobReader");
            if (observer == null) throw new ArgumentNullException("observer");
            _queueReader = queueReader;
            _blobReader = blobReader;
            _observer = observer;
        }

        public IObserver<ICloudQueueMessageEnvelope> Observer
        {
            get { return _observer; }
        }

        public ICloudQueueMessageEnvelopeJsonReader QueueReader
        {
            get { return _queueReader; }
        }

        public ICloudBlobMessageEnvelopeReader BlobReader
        {
            get { return _blobReader; }
        }

        public void OnNext(CloudQueueMessage value)
        {
            var envelope = ReadEnvelopeFromContent(value);
            if (envelope.ContentType == ContentTypes.BlobReference)
            {
                var blobId = new Guid(envelope.Content);
                var blobEnvelope = _blobReader.Read(blobId);

                envelope = envelope.
                    SetContentType(blobEnvelope.ContentType).
                    SetContent(blobEnvelope.Content);
            }
            Observer.OnNext(envelope);
        }

        private IConfigureCloudQueueMessageEnvelope ReadEnvelopeFromContent(CloudQueueMessage value)
        {
            using (var stream = new MemoryStream(value.AsBytes))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    using (var jsonReader = new JsonTextReader(streamReader))
                    {
                        return QueueReader.Read(jsonReader);
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