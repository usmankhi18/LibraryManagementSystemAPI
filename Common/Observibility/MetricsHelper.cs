using Prometheus;

namespace Common.Observibility
{
    public static class MetricsHelper
    {
        #region Counter


        public static Counter CreateCounter(string name, string help, params string[] labelNames)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new Exception("Counter name can not be empty.");
            }

            return Metrics.CreateCounter(name, help, labelNames);

        }


        #endregion


        #region Gauge

        public static Gauge CreateGauge(string name, string help, GaugeConfiguration configuration = null)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new Exception("gauge name can not be empty.");
            }
            return Metrics.CreateGauge(name, help, configuration);

        }


        public static Gauge CreateGauge(string name, string help, params string[] labelNames)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new Exception("gauge name can not be empty.");
            }

            return Metrics.CreateGauge(name, help, labelNames);

        }

        public static double GetValue(this Gauge gauge)
        {
            return gauge.Value;
        }

        public static void Increment(this Gauge gauge, double increment = 1)
        {
            gauge.Inc();
        }
        public static void IncrementTo(this Gauge gauge, double targetValue)
        {

            gauge.IncTo(targetValue);
        }
        public static void Decrement(this Gauge gauge, double increment = 1)
        {
            gauge.Dec();
        }
        public static void DecrementTo(this Gauge gauge, double targetValue)
        {

            gauge.DecTo(targetValue);
        }

        public static void Set(this Gauge gauge, double val)
        {
            gauge.Set(val);
        }

        #endregion

        #region Histrogram

        public static Histogram CreateHistogram(string name, string help, HistogramConfiguration configuration = null)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new Exception("Histogram name can not be empty.");
            }
            return Metrics.CreateHistogram(name, help, configuration);

        }



        public static void SetLabelsWithElapsedTime(this Histogram Histogram, double targetVal, params string[] labelValues)
        {
            Histogram.Labels(labelValues).Observe(targetVal);
        }

        #endregion

    }
}
