using System;
using System.Globalization;
using System.Xml;

namespace CloudQueueBus
{
    public class CloudBlobMessageEnvelopeWriter : ICloudBlobMessageEnvelopeWriter
    {
        private readonly ICloudBlobContainerPool _pool;
        private readonly string _overflowBlobContainerName;

        public CloudBlobMessageEnvelopeWriter(ICloudBlobContainerPool pool, string overflowBlobContainerName)
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

        public void Write(IConfigureCloudBlobMessageEnvelope envelope)
        {
            if (envelope == null) throw new ArgumentNullException("envelope");
            var container = Pool.Take(_overflowBlobContainerName);
            try
            {
                var reference = container.GetBlobReferenceFromServer(envelope.BlobId.ToString("N"));
                reference.Metadata[BlobMetaData.ContentType] = 
                    envelope.ContentType;
                reference.Metadata[BlobMetaData.ContentLength] =
                    envelope.Content.Length.ToString(CultureInfo.InvariantCulture);
                reference.Metadata[BlobMetaData.Time] = 
                    XmlConvert.ToString(envelope.Time);
                reference.SetMetadata();
                reference.UploadFromByteArray(envelope.Content, 0, envelope.Content.Length);
            }
            finally
            {
                Pool.Return(container);
            }
        }
    }
}