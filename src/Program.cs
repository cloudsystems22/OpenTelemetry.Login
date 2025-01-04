using OpenTelemetry.Login.Database;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Login;
using Microsoft.Extensions.Options;
using OpenTelemetry.Login.Services;
using OpenTelemetry;
using Google.Protobuf.WellKnownTypes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApiContext>(options => options.UseInMemoryDatabase("UserDb"));
//builder.Services.AddSingleton<TelemetryService>();

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(DiagnosticsConfig.ServiceName))
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation();

        metrics.AddMeter(DiagnosticsConfig.Meter.Name)
               .AddMeter("Microsoft.AspNetCore.Hosting")
               .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
               .AddMeter("Microsoft.EntityFrameworkCore")
               .AddMeter("System.Net.Http");

        metrics.AddOtlpExporter();
        //metrics.AddOtlpExporter(options => options.Endpoint = new Uri("http://userlogin.dashboard:18889"));
    })
    .WithTracing(tracing =>
    {
        tracing
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Tracing.NET"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddEntityFrameworkCoreInstrumentation(options =>
            {
                options.EnrichWithIDbCommand = (activity, command) =>
                {
                    activity.SetTag("UserDb", command.CommandText);
                };
            });

        tracing
            .AddProcessor(new SimpleActivityExportProcessor(
                new ActivityExporter(builder.Services.BuildServiceProvider()
                                                           .GetRequiredService<ApiContext>())));

            tracing.AddOtlpExporter();
            tracing.AddConsoleExporter();
    })
    .WithLogging(loggingBuilder =>
    {
        loggingBuilder.AddProcessor(new SimpleLogRecordExportProcessor(new LogRecordExporter(
                 builder.Services.BuildServiceProvider().GetRequiredService<ApiContext>())));
    });

builder.Logging
    .ClearProviders()
    .AddConsole()
    .AddDebug()
    .AddOpenTelemetry(logging =>
    {
        logging.AddOtlpExporter()
               .SetResourceBuilder(ResourceBuilder.CreateDefault()
                                                  .AddService("Logging.Aspire.UserLogin"));
    })
    .AddOpenTelemetry(logging =>
    {
        logging.AddConsoleExporter()
              .SetResourceBuilder(ResourceBuilder.CreateDefault()
                                                 .AddService("Logging.Console.UserLogin"))
              .IncludeScopes = true;

    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
