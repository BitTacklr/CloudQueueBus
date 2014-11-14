using System;
using NUnit.Framework;

namespace CloudQueueBus.Tests
{
    [TestFixture]
    public class CloudBlobMessageEnvelopeReaderTests : CloudBlobClientFixture
    {
        private CloudBlobMessageEnvelopeReader _sut;

        public override void SetUp()
        {
            base.SetUp();
            _sut = new CloudBlobMessageEnvelopeReader(new CloudBlobContainerPool(Client), ContainerName);
        }
        
        [Test]
        public void IsCloudBlobMessageEnvelopeReader()
        {
            Assert.That(_sut, Is.InstanceOf<ICloudBlobMessageEnvelopeReader>());
        }

        [Test]
        public void ReadHasExpectedResult()
        {
            var writer = new CloudBlobMessageEnvelopeWriter(new CloudBlobContainerPool(Client), ContainerName);
            writer.Write(new CloudBlobMessageEnvelope().
                SetContent(new byte[] { 1, 2, 3 }).
                SetContentType("test").
                SetMessageId(Guid.Empty).
                SetBlobId(Guid.Empty).
                SetTime(DateTimeOffset.UtcNow));
            var blob = _sut.Read(Guid.Empty);
        }
    }
}
