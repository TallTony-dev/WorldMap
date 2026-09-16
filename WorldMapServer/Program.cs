using DriverLib;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System.ComponentModel;
using WorldMapLib;
using WorldMapServer;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddConsole(consoleLogOptions =>
{
    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
});
builder.Services.AddSingleton(new WorldGraph("testing"));
builder.Services.AddSingleton(new Robot());
builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithTools<WorldGraphTools>();
builder.Services.AddHostedService<GraphSurroudingsPeriodicService>();


await builder.Build().RunAsync();
