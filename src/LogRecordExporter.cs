using OpenTelemetry.Login.Database;
using OpenTelemetry.Logs;
using System.Text.Json;

namespace OpenTelemetry.Login
{
    public class LogRecordExporter : BaseExporter<LogRecord>
    {
        private readonly ApiContext _context;

        public LogRecordExporter(ApiContext context)
        {
            _context = context;
        }

        public override ExportResult Export(in Batch<LogRecord> batch)
        {
            foreach (var logRecord in batch)
            {

                var scopeValues = new List<string>();
                var rawData = string.Empty;
                logRecord.ForEachScope<object>((scope, state) =>
                {
                   foreach (var item in scope)
                   {
                       scopeValues.Add($"{item.Key}:{item.Value}");
                        if (item.Key == "Rawdata")
                            rawData = item.Value.ToString();
                   }

                    //scopeValues.Add(scope.ToString());
                }, state: null);
                //var scopeValuesJson = rawData != null ? JsonSerializer.Serialize(rawData) : string.Empty;

                string userName = string.Empty;
                if(logRecord.Attributes != null)
                {
                    foreach(var attribute in logRecord.Attributes)
                    {
                        if (attribute.Key == "@User")
                            userName = attribute.Value.ToString();
                    }
                }

                var logRecordItem = new LogRecordItem
                {
                    Uid = Guid.NewGuid().ToString(),
                    Context = "Login",
                    Subcontext = logRecord.CategoryName,
                    Description = logRecord.Body,
                    Username = userName,
                    TraceId = logRecord.TraceId.ToString(),
                    SpanId = logRecord.SpanId.ToString(),
                    LogLevel = logRecord.LogLevel.ToString(),
                    Message = logRecord.FormattedMessage,
                    Timestamp = logRecord.Timestamp.ToUniversalTime(),
                    Rawdata = rawData
                };

                _context.LogRecords.Add(logRecordItem);
            }

            _context.SaveChanges();
            return ExportResult.Success;
        }
    }
}
