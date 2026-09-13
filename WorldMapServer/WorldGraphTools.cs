using ModelContextProtocol.Server;
using System.ComponentModel;
using WorldMapLib;

[McpServerToolType]
public class WorldGraphTools
{

    private WorldGraph _worldGraph;
    private Area _currentArea;

    public WorldGraphTools(WorldGraph worldGraph)
    {
        _worldGraph = worldGraph;
        _currentArea = worldGraph.GetBaseArea();
    }


    [McpServerTool, Description("Adds an area to the current area in the graph")]
    public void AddAreaToCurrentArea(string areaName, string areaDescription, 
    int latitude, int longitude, string[] adjacentRooms)
    {
        


    }


    [McpServerTool, Description("Echoes the message back to the client.")]
    public static string Echo(string message) => $"hello {message}";
}