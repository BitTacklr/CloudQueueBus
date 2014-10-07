using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace CloudQueueBus
{
    public class AsyncCloudQueueMessageHandler : IAsyncHandler<CloudQueueMessage>
    {
        private readonly ICloudQueueMessageEnvelopeJsonReader _queueReader;
        private readonly ICloudBlobMessageEnvelopeReader _blobReader;
        private readonly IAsyncHandler<ICloudQueueMessageEnvelope> _next;

        public AsyncCloudQueueMessageHandler(ICloudQueueMessageEnvelopeJsonReader queueReader, ICloudBlobMessageEnvelopeReader blobReader, IAsyncHandler<ICloudQueueMessageEnvelope> next)
        {
            if (queueReader == null) throw new ArgumentNullException("queueReader");
            if (blobReader == null) throw new ArgumentNullException("blobReader");
            if (next == null) throw new ArgumentNullException("next");
            _queueReader = queueReader;
            _blobReader = blobReader;
            _next = next;
        }

        public IAsyncHandler<ICloudQueueMessageEnvelope> Next
        {
            get { return _next; }
        }

        public ICloudQueueMessageEnvelopeJsonReader QueueReader
        {
            get { return _queueReader; }
        }

        public ICloudBlobMessageEnvelopeReader BlobReader
        {
            get { return _blobReader; }
        }

        public Task HandleAsync(CloudQueueMessage value)
        {
            return HandleAsync(value, CancellationToken.None);
        }

        public Task HandleAsync(CloudQueueMessage value, CancellationToken cancellationToken)
        {
            if (value == null) throw new ArgumentNullException("message");
            var envelope = ReadEnvelopeFromContent(value);
            if (envelope.ContentType == ContentTypes.BlobReference)
            {
                var blobId = new Guid(envelope.Content);
                var blobEnvelope = _blobReader.Read(blobId);

                envelope = envelope.
                    SetContentType(blobEnvelope.ContentType).
                    SetContent(blobEnvelope.Content);
            }
            return Next.HandleAsync(envelope, cancellationToken);
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