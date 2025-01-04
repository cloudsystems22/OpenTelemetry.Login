using OpenTelemetry.Login.Database;
using System.Diagnostics;

namespace OpenTelemetry.Login.Services
{
    public class TelemetryService
    {
        private readonly ApiContext _context;

        public TelemetryService(ApiContext context)
        {
            _context = context;
        }

        public void SaveTelemetry(Activity activity)
        {
            string pathUrl = string.Empty;
            string serverAddress = string.Empty;
            string statusCode = string.Empty;
            foreach (var tags in activity.Parent.TagObjects)
            {
                if (tags.Key == "server.address")
                    serverAddress = tags.Value.ToString();

                if (tags.Key == "url.path")
                    pathUrl = tags.Value.ToString();

                if (tags.Key == "http.response.status_code")
                    statusCode = tags.Value.ToString();
            }

            var telemetryItem = new TelemetryItem
            {
                Uid = Guid.NewGuid().ToString(),
                Context = "Login",
                Subcontext = "Cadatro Usuario",
                Action = activity.DisplayName,
                Description = $"{activity.DisplayName} - {statusCode}",
                Username = "",
                Source = $"{serverAddress}{pathUrl}",
                TraceId = activity.TraceId.ToString(),
                SpanId = activity.SpanId.ToString(),
                Name = activity.DisplayName,
                StartTime = activity.StartTimeUtc,
                Duration = activity.Duration == TimeSpan.Zero ? DateTime.UtcNow : activity.StartTimeUtc + activity.Duration,
                Status = activity.Status.ToString(),
                Brand = "XP",
                Platform = "IOS",
                PlatformVersion = "v1.0",
                Channel = "HUB APP",
                UserAgent = "",
                UserAgentVersion = "v1.0",
                IPClient = "127.0.0.0",
                CorrelationId = "99000AAAAAA000CC99",
                Rawdata = ""
            };

            _context.TelemetryItems.Add(telemetryItem);
            _context.SaveChanges();
        }
    }
}
