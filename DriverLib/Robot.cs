using DriverLib.Transport;
using System;
using System.Collections.Generic;
using System.Text;
using static DriverLib.Direction;
using WorldMapLib;
using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace DriverLib
{
    public class Robot : IRobot
    {
        IMovement _movementDriver;
        ICamera _camera;
        IDeviceTransport _deviceTransport;
        WorldGraph _worldGraph;

        Kernel lightImageModelMapKernel;

        
        public Robot(string targetIp, string graphSaveName)
        {
            _deviceTransport = new HttpDevice(targetIp);
            _worldGraph = new WorldGraph(graphSaveName);

            _movementDriver = new Movement(_deviceTransport);
            _camera = new Camera(_deviceTransport);

            string modelId = "gemma4:e2b";
            string endpoint = "http://localhost:11434";

            var builder = Kernel.CreateBuilder();
            builder.AddOllamaChatCompletion(
                modelId: modelId,
                endpoint: new Uri(endpoint)
            );
            
            lightImageModelMapKernel = builder.Build();
            lightImageModelMapKernel.Plugins.AddFromType<WorldGraphTools>("World graph tools");
        }

        public async Task<Image> GetImageFromDirection(Direction direction)
        {
            return await _camera.GetImageFromDirection(direction);
        }

        public async Task MoveInDirection(Direction movementDir, float durationSecs)
        {
            await _movementDriver.MoveInDirectionForDuration(movementDir, durationSecs);
        }


        public async Task MoveInDirectionUntilSenseForwards(Direction movementDir, int distanceFromObject)
        {
            
        }


        public Direction GetDirectionOfFront()
        {
            throw new NotImplementedException();
        }
    }

}
