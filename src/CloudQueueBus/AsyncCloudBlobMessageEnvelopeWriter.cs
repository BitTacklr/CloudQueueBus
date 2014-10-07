using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace CloudQueueBus
{
    public class AsyncCloudBlobMessageEnvelopeWriter : IAsyncCloudBlobMessageEnvelopeWriter
    {
        private readonly ICloudBlobContainerPool _pool;
        private readonly string _overflowBlobContainerName;

        public AsyncCloudBlobMessageEnvelopeWriter(ICloudBlobContainerPool pool, string overflowBlobContainerName)
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

        public Task WriteAsync(IConfigureCloudBlobMessageEnvelope envelope)
        {
            return WriteAsync(envelope, CancellationToken.None);
        }

        public async Task WriteAsync(IConfigureCloudBlobMessageEnvelope envelope, CancellationToken cancellationToken)
        {
            if (envelope == null) throw new ArgumentNullException("envelope");
            var container = Pool.Take(_overflowBlobContainerName);
            try
            {
                var reference = await container.GetBlobReferenceFromServerAsync(envelope.BlobId.ToString("N"), cancellationToken);
                reference.Metadata[BlobMetaData.ContentType] =
                    envelope.ContentType;
                reference.Metadata[BlobMetaData.ContentLength] =
                    envelope.Content.Length.ToString(CultureInfo.InvariantCulture);
                reference.Metadata[BlobMetaData.Time] =
                    XmlConvert.ToString(envelope.Time);
                await reference.SetMetadataAsync(cancellationToken);
                await reference.UploadFromByteArrayAsync(envelope.Content, 0, envelope.Content.Length, cancellationToken);
            }
            finally
            {
                Pool.Return(container);
            } 
        }
    }
}