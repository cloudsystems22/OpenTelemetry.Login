﻿using OpenTelemetry.Logs;
using System.Diagnostics;

namespace OpenTelemetry.Login;

public class ActivityEventLogProcessor : BaseProcessor<LogRecord>
{
    public override void OnEnd(LogRecord data)
    {
        base.OnEnd(data);
        var currentActivity = Activity.Current;
        currentActivity?.AddEvent(new ActivityEvent(data.Attributes.ToString()));
    }
}
