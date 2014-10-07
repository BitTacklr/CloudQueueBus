using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace CloudQueueBus
{
    public class AsyncCloudQueueMessageEnvelopeSender : IAsyncCloudQueueMessageEnvelopeSender
    {
        private readonly IAsyncCloudQueueSender _sender;
        private readonly ICloudQueueMessageEnvelopeJsonWriter _queueWriter;
        private readonly ICloudBlobMessageEnvelopeWriter _blobWriter;

        public AsyncCloudQueueMessageEnvelopeSender(IAsyncCloudQueueSender sender, ICloudQueueMessageEnvelopeJsonWriter queueWriter, ICloudBlobMessageEnvelopeWriter blobWriter)
        {
            if (sender == null) throw new ArgumentNullException("sender");
            if (queueWriter == null) throw new ArgumentNullException("queueWriter");
            if (blobWriter == null) throw new ArgumentNullException("blobWriter");
            _sender = sender;
            _queueWriter = queueWriter;
            _blobWriter = blobWriter;
        }

        public IAsyncCloudQueueSender Sender
        {
            get { return _sender; }
        }

        public ICloudQueueMessageEnvelopeJsonWriter QueueWriter
        {
            get { return _queueWriter; }
        }

        public ICloudBlobMessageEnvelopeWriter BlobWriter
        {
            get { return _blobWriter; }
        }

        public Task SendAsync(IConfigureCloudQueueMessageEnvelope envelope)
        {
            return SendAsync(envelope, CancellationToken.None);
        }

        public Task SendAsync(IConfigureCloudQueueMessageEnvelope envelope, CancellationToken cancellationToken)
        {
            if (envelope == null) throw new ArgumentNullException("envelope");

            var content = WriteEnvelopeToContent(envelope);
            if (content.Length > Limits.CloudQueueMessageContentLimit)
            {
                var blobId = SerialGuid.NewGuid();
                BlobWriter.Write(envelope.ToBlobEnvelope(blobId));

                envelope = envelope.
                    SetContentType(ContentTypes.BlobReference).
                    SetContent(blobId.ToByteArray());

                content = WriteEnvelopeToContent(envelope);
            }
            return Sender.SendAsync(envelope.To, new CloudQueueMessage(content), cancellationToken);
        }

        private byte[] WriteEnvelopeToContent(IConfigureCloudQueueMessageEnvelope envelope)
        {
            using (var stream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(stream))
                {
                    using (var jsonWriter = new JsonTextWriter(streamWriter))
                    {
                        QueueWriter.Write(envelope, jsonWriter);
                        jsonWriter.Flush();
                        return stream.ToArray();
                    }
                }
            }
        }
    }
}