using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus
{
    internal static class QueueRequestOptionsExtensions
    {
        public static QueueRequestOptions Clone(this QueueRequestOptions options)
        {
            return new QueueRequestOptions
            {
                LocationMode = options.LocationMode,
                MaximumExecutionTime = options.MaximumExecutionTime,
                RetryPolicy = options.RetryPolicy,
                ServerTimeout = options.ServerTimeout
            };
        }
    }
}