using System;

namespace StatsdClient
{
    /// <summary>
    /// The advanced configuration options for DogStatsdService.
    /// </summary>
    public class AdvancedStatsConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedStatsConfig"/> class.
        /// </summary>
        public AdvancedStatsConfig()
        {
            DurationBeforeSendingNotFullBuffer = TimeSpan.FromMilliseconds(100);
        }

        /// <summary>
        /// Gets or sets a value defining the maximum number of metrics in the queue
        /// (Metrics are sent asynchronously using a queue).
        /// A small value reduces memory usage whereas an higher value reduces
        /// latency (When <see cref="MaxBlockDuration"/> is null) or the number of messages
        /// dropped (When <see cref="MaxBlockDuration"/> is not null).
        /// </summary>
        public int MaxMetricsInAsyncQueue { get; set; } = 100 * 1000;

        /// <summary>
        /// Gets or sets a value defining the maximum duration a call can block.
        /// If there are more metrics than `MaxMetricsInAsyncQueue` waiting to be sent:
        ///     - if <see cref="MaxBlockDuration"/> is null, the metric send by a call to a
        ///       <see cref="DogStatsd"/> or <see cref="DogStatsdService"/> method will be dropped.
        ///     - If <see cref="MaxBlockDuration"/> is not null, the metric send by a call to a
        ///       <see cref="DogStatsd"/> or <see cref="DogStatsdService"/> method will block for at most
        ///       <see cref="MaxBlockDuration"/> duration.
        /// </summary>
        public TimeSpan? MaxBlockDuration { get; set; } = null;

        /// <summary>
        /// Gets or sets a value defining how long
        /// DogStatsD waits before sending a not full buffer (Metrics are buffered before sent).
        /// </summary>
        public TimeSpan DurationBeforeSendingNotFullBuffer { get; set; }

        /// <summary>
        /// Gets or sets a value defining how long to wait when the UDS buffer is full
        /// (SocketError.NoBufferSpaceAvailable). A null value results in
        /// dropping the metric.
        /// </summary>
        public TimeSpan? UDSBufferFullBlockDuration { get; set; } = null;

        /// <summary>
        /// Gets or sets a value defining the duration between two telemetry flushes.
        /// When this value is set at null, telemetry is disabled.
        /// </summary>
        public TimeSpan? TelemetryFlushInterval { get; set; } = TimeSpan.FromSeconds(10);
    }
}