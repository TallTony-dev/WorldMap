using DriverLib.Transport;
using System;
using System.Collections.Generic;
using System.Text;
using WorldMapLib;
using System.ComponentModel;
using Microsoft.SemanticKernel;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Agents.AI.Foundry;

namespace DriverLib
{
    public class Robot : IRobot
    {
        IMovement _movementDriver;
        ICamera _camera;
        IDeviceTransport _deviceTransport;
        WorldGraph _worldGraph;

        Kernel lightImageModelMapKernel;
        AIAgent navigationAgent;
        
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

            navigationAgent = lightImageModelMapKernel.GetRequiredService<IChatClient>().AsAIAgent
            (
                name: "Navigator",
                instructions: @"You are a navigation agent who is controlling a robot. You should reason about your environment and preform the assigned task in an effective manner.",
                //tools: FoundryAITool.Create
            );

        }
        

        public async Task<Image> GetImageFromDirection(Direction direction)
        {
            return await _camera.GetImageFromDirection(direction);
        }

        public async Task MoveInDirection(Direction movementDir, float durationSecs)
        {
            await _movementDriver.MoveInDirectionForDuration(movementDir, durationSecs);
        }


        public async Task MoveInDirectionUntilSenseForwards(Direction movementDir, float distanceFromObject)
        {
            await _movementDriver.MoveInDirectionUntilSenseForwards(movementDir, distanceFromObject);
        }


        public Direction GetDirectionOfFront()
        {
            throw new NotImplementedException();
        }
    }

}
