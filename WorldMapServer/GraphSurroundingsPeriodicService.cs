using WorldMapLib;
using LLMLib;
using DriverLib;

namespace WorldMapServer
{
    public class GraphSurroudingsPeriodicService(ILogger<GraphSurroudingsPeriodicService> logger, WorldGraph worldGraph, IRobot robot) : BackgroundService
    {
        private WorldGraph _worldGraph = worldGraph;
        private IRobot _robot = robot;



        private void GraphSurroundings()
        {
            Image[] images = new Image[2];
            images[0] = _robot.GetImageFromDirection(new Direction(Direction.EDirection.North));
            images[1] = _robot.GetImageFromDirection(new Direction(Direction.EDirection.South));

            LLMProcessing.AddApplicableAreasFromImageAsync(images, _worldGraph.CurrentArea).Wait();
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var timer = new PeriodicTimer(TimeSpan.FromSeconds(30));

            try
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    logger.LogInformation("Executing surroundings graphing");
                    GraphSurroundings();

                }
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Graph surroundings periodic task failed with exception {ex.Message}");
            }
        }
    }
}
