using System.Collections.Generic;
using StatsdClient.Statistic;
using StatsdClient.Utils;

namespace StatsdClient.Aggregator
{
    /// <summary>
    /// Aggregate `StatsMetric` instances of type `Set` by keeping the unique values.
    /// </summary>
    internal class SetAggregator
    {
        private readonly AggregatorFlusher<StatsMetricSet> _aggregator;
        private readonly Pool<StatsMetricSet> _pool;
        private readonly Telemetry _optionalTelemetry;

        public SetAggregator(MetricAggregatorParameters parameters, Telemetry optionalTelemetry)
        {
            _aggregator = new AggregatorFlusher<StatsMetricSet>(parameters, MetricType.Set);
            _pool = new Pool<StatsMetricSet>(pool => new StatsMetricSet(pool), 2 * parameters.MaxUniqueStatsBeforeFlush);
            _optionalTelemetry = optionalTelemetry;
        }

        public void OnNewValue(ref StatsMetric metric)
        {
            string value = metric.StringValue;
            var key = _aggregator.CreateKey(metric);
            if (_aggregator.TryGetValue(ref key, out var v))
            {
                v.Values.Add(value);
                _aggregator.Update(ref key, v);
            }
            else
            {
                if (_pool.TryDequeue(out var metricFieldsMetricSet))
                {
                    metricFieldsMetricSet.StatsMetric = metric;
                    metricFieldsMetricSet.Values.Add(value);
                    _aggregator.Add(ref key, metricFieldsMetricSet);
                }
                else
                {
                    _optionalTelemetry?.OnPacketsDroppedQueue();
                }
            }

            this.TryFlush();
        }

        public void TryFlush(bool force = false)
        {
            _aggregator.TryFlush(
                dictionary =>
                {
                    foreach (var keyValue in dictionary)
                    {
                        using (var statsMetricSet = keyValue.Value)
                        {
                            var metric = statsMetricSet.StatsMetric;
                            foreach (var v in statsMetricSet.Values)
                            {
                                metric.StringValue = v;
                                _aggregator.FlushStatsMetric(metric);
                            }
                        }
                    }
                },
                force);
        }

        private class StatsMetricSet : AbstractPoolObject
        {
            public StatsMetricSet(IPool pool)
            : base(pool)
            {
            }

            public StatsMetric StatsMetric { get; set; }

            public HashSet<string> Values { get; } = new HashSet<string>();

            protected override void DoReset()
            {
                Values.Clear();
            }
        }
    }
}