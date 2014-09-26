using System;
using System.Diagnostics;
using System.Globalization;
using System.Xml;

namespace CloudQueueBus
{
    public class CloudBlobMessageEnvelopeReader : ICloudBlobMessageEnvelopeReader
    {
        private readonly ICloudBlobContainerPool _pool;
        private readonly string _overflowBlobContainerName;

        public CloudBlobMessageEnvelopeReader(ICloudBlobContainerPool pool, string overflowBlobContainerName)
        {
            if (pool == null) throw new ArgumentNullException("pool");
            if (overflowBlobContainerName == null) throw new ArgumentNullException("overflowBlobContainerName");
            _pool = pool;
            _overflowBlobContainerName = overflowBlobContainerName;
        }

        public ICloudBlobContainerPool Pool
        {
            get { return _pool; }
        }

        public ICloudBlobMessageEnvelope Read(Guid blobId)
        {
            var container = Pool.Take(_overflowBlobContainerName);
            try
            {
                var reference = container.GetBlobReferenceFromServer(blobId.ToString("N"));
                reference.FetchAttributes();

                var contentType = reference.Metadata[BlobMetaData.ContentType];
                var messageId = new Guid(reference.Metadata[BlobMetaData.MessageId]);
                var time = XmlConvert.ToDateTimeOffset(reference.Metadata[BlobMetaData.Time]);

                var expectedLength = int.Parse(reference.Metadata[BlobMetaData.ContentLength],
                    CultureInfo.InvariantCulture);
                var content = new byte[expectedLength];
                var actualLength = reference.DownloadToByteArray(content, 0);
                Trace.Assert(content.Length == actualLength,
                    string.Format(
                        "The expected content length ({0}) and actual content length ({1}) of blob {2} must match.",
                        content.Length, actualLength, blobId.ToString("N")));

                return new CloudBlobMessageEnvelope().
                    SetBlobId(blobId).
                    SetMessageId(messageId).
                    SetContentType(contentType).
                    SetContent(content).
                    SetTime(time);
            }
            finally
            {
                Pool.Return(container);
            }
        }
    }
}