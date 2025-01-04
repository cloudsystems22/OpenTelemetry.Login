namespace OpenTelemetry.Login.Database
{
    public class LogRecordItem
    {
        public int Id { get; set; }
        public string Uid { get; set; }
        public string? Context { get; set; }
        public string? Subcontext { get; set; }
        public string? Action { get; set; }
        public string? Description { get; set; }
        public string? Source { get; set; }
        public string? Username { get; set; }
        public string? TraceId { get; set; }
        public string? SpanId { get; set; }
        public string? LogLevel { get; set; }
        public string? Message { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Brand { get; set; }
        public string? Platform { get; set; }
        public string? PlatformVersion { get; set; }
        public string? Channel { get; set; }
        public string? UserAgent { get; set; }
        public string? UserAgentVersion { get; set; }
        public string? IPClient { get; set; }
        public string? CorrelationId { get; set; }
        public string? Rawdata { get; set; }
    }
}
