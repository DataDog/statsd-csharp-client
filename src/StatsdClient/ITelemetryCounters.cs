namespace StatsdClient
{
    /// <summary>
    /// ITelemetryCounters contains the telemetry counters.
    /// </summary>
    public interface ITelemetryCounters
    {
        /// <summary>
        /// Gets the number of metrics sent.
        /// </summary>
        /// <value>The number of metrics sent.</value>
        int MetricsSent { get; }

        /// <summary>
        /// Gets the number of events sent.
        /// </summary>
        /// <value>The number of events sent.</value>
        int EventsSent { get; }

        /// <summary>
        /// Gets the number of service checks sent.
        /// </summary>
        /// <value>The number of service checks sent.</value>
        int ServiceChecksSent { get; }

        /// <summary>
        /// Gets the total number of bytes sent.
        /// </summary>
        /// <value>The total number of bytes sent.</value>
        int BytesSent { get; }

        /// <summary>
        /// Gets the total number of bytes dropped.
        /// </summary>
        /// <value>The total number of bytes dropped.</value>
        int BytesDropped { get; }

        /// <summary>
        /// Gets the number of packets (UDP or UDS) sent.
        /// </summary>
        /// <value>The number of packets (UDP or UDS) sent.</value>
        int PacketsSent { get; }

        /// <summary>
        /// Gets the number of packets (UDP or UDS) dropped.
        /// </summary>
        /// <value>The number of packets (UDP or UDS) dropped.</value>
        int PacketsDropped { get; }

        /// <summary>
        /// Gets the number of packets (UDP or UDS) dropped because the queue is full.
        /// </summary>
        /// <value>The number of packets (UDP or UDS) dropped because the queue is full.</value>
        int PacketsDroppedQueue { get; }
    }
}