using System.Diagnostics.Metrics;

namespace OpenTelemetry.Login
{
    public class DiagnosticsConfig
    {
        public const string ServiceName = "User Login";

        public static Meter Meter = new(ServiceName);

        public static Counter<int> LoginCounter = Meter.CreateCounter<int>("login.count");
    }
}
