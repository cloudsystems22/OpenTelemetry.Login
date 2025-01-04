using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OpenTelemetry.Login.Database
{
    public class TelemetryItem
    {
        public int Id { get; set; }
        public string Uid { get; set; }
        public string Context { get; set; }
        public string Subcontext { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Username { get; set; }
        public string TraceId { get; set; }
        public string SpanId { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime Duration { get; set; }
        public string Status { get; set; }
        public string Brand { get; set; }
        public string Platform { get; set; }
        public string PlatformVersion { get; set; }
        public string Channel { get; set; }
        public string UserAgent { get; set; }
        public string UserAgentVersion { get; set; }
        public string IPClient { get; set; }
        public string CorrelationId { get; set; }
        public string Rawdata { get; set; }
    }
}
