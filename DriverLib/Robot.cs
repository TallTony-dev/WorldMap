using DriverLib.Transport;
using System;
using System.Collections.Generic;
using System.Text.Json;
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

        AIAgent navigationAgent;
        AIAgent graphManagerAgent;
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
            
            Kernel lightImageModelMapKernel = builder.Build();
            lightImageModelMapKernel.Plugins.AddFromType<WorldGraphTools>("World graph tools");

            navigationAgent = lightImageModelMapKernel.GetRequiredService<IChatClient>().AsAIAgent
            (
                name: "Navigator",
                instructions: @"You are a navigation agent who is controlling a robot. You should reason about your environment and use the given tools to complete the assigned task.
                Another agent is handling the management of the world graph tool given, so you shouldn't need to write to it, only query it.",
                tools:  
                [
                    AIFunctionFactory.Create(this.GetImageFromDirection),
                    AIFunctionFactory.Create(this.MoveInDirection),
                    AIFunctionFactory.Create(this.MoveInDirectionUntilSenseForwards),
                ]
            );

            graphManagerAgent = lightImageModelMapKernel.GetRequiredService<IChatClient>().AsAIAgent
            (
                name: "Graph Manager",
                instructions: @"You a a graph managing agent who ensures the accuracy and maintains the quality of the world map given in your tools. Another agent controls movement,
                so you should rely on the given imaging tool to find what is around you.",
                tools:
                [
                    AIFunctionFactory.Create(this.GetImageFromDirection)
                ]
            );

        }

        public void NavigateToAreaByName(string areaName)
        {

        }

        public async Task GraphSurroundings()
        {
            Console.WriteLine("graph surroundings agent pt 1: " + graphManagerAgent.RunAsync(@"Use the provided tools to first query your current position on the graph, 
                then compare it to the true world around you using the camera tool provided. Rectify any differences in your current position and the position identified on the graph."));
            Console.WriteLine("graph surroundings agent pt 2: " + graphManagerAgent.RunAsync(@"Use the provided tools to ensure that the surroundings you see are sufficiently modelled
                in the world graph given. Add and remove duplicate areas and unmapped areas, and connect areas as needed to make the model match the world."));
        }


        [Description("Gets an image from the passed in direction, where north is treated as forwards.")]
        public async Task<Image> GetImageFromDirection(Direction direction)
        {
            return await _camera.GetImageFromDirection(direction);
        }

        [Description("Moves the robot forward for a specified duration.")]
        public async Task MoveInDirection(Direction movementDir, float durationSecs)
        {
            await _movementDriver.MoveInDirectionForDuration(movementDir, durationSecs);
        }

        [Description("Moves the robot forward until an object is sensed a number of meters away.")]
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
