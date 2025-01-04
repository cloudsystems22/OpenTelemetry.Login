using OpenTelemetry.Login.Database;
using OpenTelemetry.Logs;
using System.Diagnostics;

namespace OpenTelemetry.Login
{
    public class ActivityExporter : BaseExporter<Activity>
    {
        private readonly ApiContext _context;

        public ActivityExporter(ApiContext context)
        {
            _context = context;
        }

        public override ExportResult Export(in Batch<Activity> batch)
        {
            foreach (var activity in batch)
            {
                string pathUrl = string.Empty;
                string serverAddress = string.Empty;
                string statusCode = string.Empty;
                string userAgent = string.Empty;
                if (activity.TagObjects != null)
                {
                    foreach (var tags in activity.TagObjects)
                    {
                        if (tags.Key == "server.address")
                            serverAddress = tags.Value.ToString();

                        if (tags.Key == "url.path")
                            pathUrl = tags.Value.ToString();

                        if (tags.Key == "user_agent.original")
                            userAgent = tags.Value.ToString();

                        if (tags.Key == "http.response.status_code")
                            statusCode = tags.Value.ToString();
                    }
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
                    UserAgent = userAgent,
                    UserAgentVersion = "v1.0",
                    IPClient = "127.0.0.0",
                    CorrelationId = "99000AAAAAA000CC99",
                    Rawdata = ""
                };

                _context.TelemetryItems.Add(telemetryItem);
            }

            _context.SaveChanges();
            return ExportResult.Success;
        }
    }
}
