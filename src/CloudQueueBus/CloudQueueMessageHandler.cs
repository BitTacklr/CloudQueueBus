using System;
using System.IO;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace CloudQueueBus
{
    public class CloudQueueMessageHandler : IHandler<CloudQueueMessage>
    {
        private readonly ICloudQueueMessageEnvelopeJsonReader _queueReader;
        private readonly ICloudBlobMessageEnvelopeReader _blobReader;
        private readonly IObserver<ICloudQueueMessageEnvelope> _observer;

        public CloudQueueMessageHandler(ICloudQueueMessageEnvelopeJsonReader queueReader, ICloudBlobMessageEnvelopeReader blobReader, IObserver<ICloudQueueMessageEnvelope> observer)
        {
            if (queueReader == null) throw new ArgumentNullException("queueReader");
            if (blobReader == null) throw new ArgumentNullException("blobReader");
            if (observer == null) throw new ArgumentNullException("next");
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

        public void Handle(CloudQueueMessage message)
        {
            var envelope = ReadEnvelopeFromContent(message);
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
    }
}