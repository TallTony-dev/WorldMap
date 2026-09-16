using DriverLib;
using ModelContextProtocol.Server;
using System.ComponentModel;
using WorldMapLib;

namespace WorldMapServer;

[McpServerToolType]
public class WorldGraphTools(WorldGraph worldGraph, IRobot robot)
{
    private IRobot _robot = robot;
    private WorldGraph _worldGraph = worldGraph;


    [McpServerTool, Description("Adds a subarea to the current area in the graph")]
    public void AddSubAreaToCurrentArea(string areaName, string areaDescription)
    {
        _worldGraph.CurrentArea.AddSubArea(areaName, areaDescription, new GpsCoord());


    }

    [McpServerTool, Description("Adds an area next to the current area in the graph which is connected to the current room")]
    public void AddAreaNextToCurrentArea(string areaName, string areaDescription)
    {
        _worldGraph.CurrentArea.GetParent()?.AddSubArea(areaName, areaDescription, new GpsCoord());
    }


    [McpServerTool, Description("Echoes the message back to the client.")]
    public static string Echo(string message) => $"hello {message}";
}