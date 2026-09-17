using Microsoft.SemanticKernel;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace WorldMapLib;


public class WorldGraphTools(WorldGraph worldGraph)
{
    private WorldGraph _worldGraph = worldGraph;


    [KernelFunction, McpServerTool, Description("Adds a subarea to the current area in the graph")]
    public void AddSubAreaToCurrentArea(string areaName, string areaDescription)
    {
        _worldGraph.CurrentArea.AddSubArea(areaName, areaDescription, new GpsCoord());

    }

    [KernelFunction, McpServerTool, Description("Adds an area next to the current area in the graph which is connected to the current room")]
    public void AddAreaNextToCurrentArea(string areaName, string areaDescription)
    {
        _worldGraph.CurrentArea.GetParent()?.AddSubArea(areaName, areaDescription, new GpsCoord());
    }
}