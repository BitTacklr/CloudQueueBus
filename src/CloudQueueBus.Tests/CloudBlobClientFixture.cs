using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using NUnit.Framework;

namespace CloudQueueBus.Tests
{
    public abstract class CloudBlobClientFixture
    {
        private CloudBlobClient _client;
        private string _containerName;

        protected CloudBlobClient Client
        {
            get { return _client; }
        }

        protected string ContainerName
        {
            get { return _containerName; }
        }

        [SetUp]
        public virtual void SetUp()
        {
            _containerName = Guid.NewGuid().ToString("N");
            _client = CloudStorageAccount.DevelopmentStorageAccount.CreateCloudBlobClient();
            Client.GetContainerReference(ContainerName).CreateIfNotExists();
        }

        [TearDown]
        public virtual void TearDown()
        {
            Client.GetContainerReference(ContainerName).DeleteIfExists();
        }
    }
}