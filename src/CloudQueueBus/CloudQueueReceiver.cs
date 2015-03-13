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
        private readonly ICloudQueueReceiverConfiguration _receiverConfiguration;
        private readonly ICloudQueueErrorConfiguration _errorConfiguration;
        private readonly CancellationTokenSource _stopSource;
        private Task _task;

        public CloudQueueReceiver(
            ICloudQueuePool pool, 
            IObserver<CloudQueueMessage> observer,
            ICloudQueueReceiverConfiguration receiverConfiguration,
            ICloudQueueErrorConfiguration errorConfiguration)
        {
            if (pool == null) throw new ArgumentNullException("pool");
            if (observer == null) throw new ArgumentNullException("observer");
            if (receiverConfiguration == null) throw new ArgumentNullException("receiverConfiguration");
            if (errorConfiguration == null) throw new ArgumentNullException("errorConfiguration");
            _pool = pool;
            _observer = observer;
            _receiverConfiguration = receiverConfiguration;
            _errorConfiguration = errorConfiguration;
            _stopSource = new CancellationTokenSource();
        }

        public ICloudQueueReceiverConfiguration ReceiverConfiguration
        {
            get { return _receiverConfiguration; }
        }

        public ICloudQueuePool Pool
        {
            get { return _pool; }
        }

        public IObserver<CloudQueueMessage> Observer
        {
            get { return _observer; }
        }

        public ICloudQueueErrorConfiguration ErrorConfiguration
        {
            get { return _errorConfiguration; }
        }

        public void Start()
        {
            _task = Task.Run(new Func<Task>(Run), _stopSource.Token);
        }

        private async Task Run()
        {
            var receiveQueue = Pool.Take(ReceiverConfiguration.ReceiveQueue);
            var errorQueue = Pool.Take(ErrorConfiguration.ErrorQueue);
            try
            {
                while (!_stopSource.IsCancellationRequested)
                {
                    try
                    {
                        var message = await receiveQueue.GetMessageAsync(
                            ReceiverConfiguration.VisibilityTimeout,
                            ReceiverConfiguration.QueueRequestOptions.Clone(),
                            null,
                            _stopSource.Token);

                        if (message != null)
                        {
                            if (message.DequeueCount > ErrorConfiguration.DequeueCountThreshold)
                            {
                                await errorQueue.AddMessageAsync(message, ErrorConfiguration.TimeToLive, ErrorConfiguration.InitialVisibilityDelay, ErrorConfiguration.QueueRequestOptions, null, _stopSource.Token);
                            }
                            else
                            {
                                Observer.OnNext(message);
                            }
                            await receiveQueue.DeleteMessageAsync(message, ReceiverConfiguration.QueueRequestOptions, null, _stopSource.Token);
                        }
                        else
                        {
                            await Task.Delay(ReceiverConfiguration.DelayBetweenIdleReceives, _stopSource.Token);
                        }
                    }
                    catch (OperationCanceledException) { }
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
                Pool.Return(receiveQueue);
                Pool.Return(errorQueue);
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
