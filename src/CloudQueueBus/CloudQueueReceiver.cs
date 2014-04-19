using System;
using System.Threading;
using System.Threading.Tasks;
using CloudQueueBus.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus
{
    public class CloudQueueReceiver : ICloudQueueReceiver
    {
        private readonly ICloudQueuePool _pool;
        private readonly IObserver<CloudQueueMessage> _observer;
        private readonly ICloudQueueReceiverConfiguration _configuration;
        private readonly CancellationTokenSource _stopSource;
        private Task _task;

        public CloudQueueReceiver(
            ICloudQueuePool pool, 
            IObserver<CloudQueueMessage> observer,
            ICloudQueueReceiverConfiguration configuration)
        {
            if (pool == null) throw new ArgumentNullException("pool");
            if (observer == null) throw new ArgumentNullException("observer");
            if (configuration == null) throw new ArgumentNullException("configuration");
            _pool = pool;
            _observer = observer;
            _configuration = configuration;
            _stopSource = new CancellationTokenSource();
        }

        public ICloudQueueReceiverConfiguration Configuration
        {
            get { return _configuration; }
        }

        public ICloudQueuePool Pool
        {
            get { return _pool; }
        }

        public IObserver<CloudQueueMessage> Observer
        {
            get { return _observer; }
        }

        public void Start()
        {
            _task = Task.Run(new Func<Task>(Run), _stopSource.Token);
        }

        private async Task Run()
        {
            var queue = Pool.Take(Configuration.ReceiveAddress);
            try
            {
                while (!_stopSource.IsCancellationRequested)
                {
                    try
                    {
                        var messages = await queue.GetMessagesAsync(
                            Configuration.BatchCount,
                            Configuration.VisibilityTimeout,
                            Configuration.QueueRequestOptions.Clone(),
                            null,
                            _stopSource.Token);

                        var receivedAnyMessages = false;
                        foreach (var message in messages)
                        {
                            receivedAnyMessages = true;
                            Observer.OnNext(message);
                            await queue.DeleteMessageAsync(message);
                        }
                        if (!receivedAnyMessages)
                        {
                            await Task.Delay(Configuration.DelayBetweenIdleReceives, _stopSource.Token);
                        }
                    }
                    catch (StorageException exception)
                    {
                        Observer.OnError(exception);
                    }
                    catch (Exception exception)
                    {
                       Observer.OnError(exception);
                    }
                }
                Observer.OnCompleted();
            }
            finally
            {
                Pool.Return(queue);
            }
        }

        public void Dispose()
        {
            if (!_stopSource.IsCancellationRequested)
            {
                _stopSource.Cancel();
            }
            if (_task != null)
            {
                _task.Wait();
                _task.Dispose();
                _task = null;
            }
            _stopSource.Dispose();
        }
    }
}
