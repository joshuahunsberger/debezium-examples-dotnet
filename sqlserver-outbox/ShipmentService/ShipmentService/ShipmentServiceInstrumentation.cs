using System.Diagnostics;

namespace ShipmentService
{
    public class ShipmentServiceInstrumentation : IDisposable
    {
        internal const string ActivitySourceName = "ShipmentService";

        public ShipmentServiceInstrumentation()
        {
            var version = typeof(ShipmentServiceInstrumentation).Assembly.GetName().Version?.ToString();
            ShipmentServiceActivitySource = new ActivitySource(ActivitySourceName, version);
        }

        public ActivitySource ShipmentServiceActivitySource { get; }

        public void Dispose()
        {
            ShipmentServiceActivitySource.Dispose();
        }
    }
}

