using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace CloudQueueBus
{
    public class AsyncCloudBlobMessageEnvelopeReader : IAsyncCloudBlobMessageEnvelopeReader
    {
        private readonly ICloudBlobContainerPool _pool;
        private readonly string _overflowBlobContainerName;

        public AsyncCloudBlobMessageEnvelopeReader(ICloudBlobContainerPool pool, string overflowBlobContainerName)
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

        public Task<ICloudBlobMessageEnvelope> ReadAsync(Guid blobId)
        {
            return ReadAsync(blobId, CancellationToken.None);
        }

        public async Task<ICloudBlobMessageEnvelope> ReadAsync(Guid blobId, CancellationToken cancellationToken)
        {
            var container = Pool.Take(_overflowBlobContainerName);
            try
            {
                var reference = await container.GetBlobReferenceFromServerAsync(blobId.ToString("N"), cancellationToken);
                await reference.FetchAttributesAsync(cancellationToken);

                var contentType = reference.Metadata[BlobMetaData.ContentType];
                var messageId = new Guid(reference.Metadata[BlobMetaData.MessageId]);
                var time = XmlConvert.ToDateTimeOffset(reference.Metadata[BlobMetaData.Time]);

                var expectedLength = int.Parse(reference.Metadata[BlobMetaData.ContentLength],
                    CultureInfo.InvariantCulture);
                var content = new byte[expectedLength];
                var actualLength = await reference.DownloadToByteArrayAsync(content, 0, cancellationToken);
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