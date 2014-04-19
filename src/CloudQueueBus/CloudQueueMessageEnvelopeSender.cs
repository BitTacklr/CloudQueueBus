using System;
using System.IO;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace CloudQueueBus
{
    public class CloudQueueMessageEnvelopeSender : ICloudQueueMessageEnvelopeSender
    {
        private readonly ICloudQueueSender _sender;
        private readonly ICloudQueueMessageEnvelopeJsonWriter _writer;

        public CloudQueueMessageEnvelopeSender(ICloudQueueSender sender, ICloudQueueMessageEnvelopeJsonWriter writer)
        {
            if (sender == null) throw new ArgumentNullException("sender");
            if (writer == null) throw new ArgumentNullException("writer");
            _sender = sender;
            _writer = writer;
        }

        public ICloudQueueSender Sender
        {
            get { return _sender; }
        }

        public ICloudQueueMessageEnvelopeJsonWriter Writer
        {
            get { return _writer; }
        }

        public void Send(IConfigureCloudQueueMessageEnvelope envelope)
        {
            if (envelope == null) throw new ArgumentNullException("envelope");
            //TODO: If content too long, move part of it to a blob, rewrite content type and content.

            using (var stream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(stream))
                {
                    using (var jsonWriter = new JsonTextWriter(streamWriter))
                    {
                        Writer.Write(envelope, jsonWriter);
                        jsonWriter.Flush();

                        Sender.Send(envelope.To, new CloudQueueMessage(stream.ToArray()));
                    }
                }
            }
        }
    }
}