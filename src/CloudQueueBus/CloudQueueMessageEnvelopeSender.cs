using System;
using System.IO;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace CloudQueueBus
{
    public class CloudQueueMessageEnvelopeSender : ICloudQueueMessageEnvelopeSender
    {
        private readonly ICloudQueueSender _sender;
        private readonly ICloudQueueMessageEnvelopeJsonWriter _queueWriter;
        private readonly ICloudBlobMessageEnvelopeWriter _blobWriter;

        public CloudQueueMessageEnvelopeSender(ICloudQueueSender sender, ICloudQueueMessageEnvelopeJsonWriter queueWriter, ICloudBlobMessageEnvelopeWriter blobWriter)
        {
            if (sender == null) throw new ArgumentNullException("sender");
            if (queueWriter == null) throw new ArgumentNullException("queueWriter");
            if (blobWriter == null) throw new ArgumentNullException("blobWriter");
            _sender = sender;
            _queueWriter = queueWriter;
            _blobWriter = blobWriter;
        }

        public ICloudQueueSender Sender
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

        public void Send(IConfigureCloudQueueMessageEnvelope envelope)
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
            Sender.Send(envelope.To, new CloudQueueMessage(content));
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