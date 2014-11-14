using System;
using NUnit.Framework;

namespace CloudQueueBus.Tests
{
    [TestFixture]
    public class CloudBlobMessageEnvelopeWriterTests : CloudBlobClientFixture
    {
        private CloudBlobMessageEnvelopeWriter _sut;

        public override void SetUp()
        {
            base.SetUp();
            _sut = new CloudBlobMessageEnvelopeWriter(new CloudBlobContainerPool(Client), ContainerName);
        }

        [Test]
        public void IsCloudBlobMessageEnvelopeWriter()
        {
            Assert.That(_sut, Is.InstanceOf<ICloudBlobMessageEnvelopeWriter>());
        }

        [Test]
        public void WriteHasExpectedResult()
        {
            _sut.Write(new CloudBlobMessageEnvelope().
                SetContent(new byte[] { 1, 2, 3 }).
                SetContentType("test").
                SetMessageId(Guid.Empty).
                SetBlobId(Guid.Empty).
                SetTime(DateTimeOffset.UtcNow));
        }
    }
}