using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System.ComponentModel;
using WorldMapLib;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddConsole(consoleLogOptions =>
{
    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
});
builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithTools<WorldGraphTools>();
builder.Services.AddHostedService<GraphSurroudingsPeriodicService>();
builder.Services.AddSingleton<WorldGraph>(new WorldGraph("testing"));

await builder.Build().RunAsync();


public class GraphSurroudingsPeriodicService(ILogger<GraphSurroudingsPeriodicService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(30));

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                logger.LogInformation("Executing surroundings graphing");
                


                

            }
        }
        catch (Exception ex)
        {
            logger.LogInformation($"Graph surroundings periodic task failed with exception {ex.Message}");
        }
    }
}
