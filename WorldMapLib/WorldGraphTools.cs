using Microsoft.SemanticKernel;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace WorldMapLib;


public class WorldGraphTools(WorldGraph worldGraph)
{
    private WorldGraph _worldGraph = worldGraph;


    [KernelFunction, Description("Adds a subarea to the current area in the graph")]
    public void AddSubAreaToCurrentArea(string areaName, string areaDescription)
    {
        _worldGraph.CurrentArea.AddSubArea(areaName, areaDescription, new GpsCoord());
    }

    [KernelFunction, Description("Adds an area next to the current area in the graph which is connected to the current room")]
    public void AddAreaNextToCurrentArea(string areaName, string areaDescription)
    {
        _worldGraph.CurrentArea.MakeNewAdjacentArea(areaName, areaDescription, new GpsCoord(), new());
    }

    public void SetCurrentArea(Area area)
    {
        _worldGraph.CurrentArea = area;
    }

    public Area GetCurrentArea()
    {
        return _worldGraph.CurrentArea;
    }

    public Area SearchForAreaSemantic(string areaName)
    {
        return _worldGraph.SearchForAreaSemantically(areaName);
    }

    public void ConnectAreaToCurrentArea(string semanticAreaName)
    {
        _worldGraph.CurrentArea.MakeAreaAdjacent(_worldGraph.CurrentArea.GetParent()?.SearchForSubAreaSemantic(semanticAreaName).area!);
    }

    public string GetPathToArea(Area area)
    {
        //trying out spans here cause seems like the best way to make performant even though not really needed :P
        ReadOnlySpan<char> path1 = area.AreaPath.AsSpan();
        ReadOnlySpan<char> path2 = _worldGraph.CurrentArea.AreaPath.AsSpan();
        int minlen = Math.Min(path1.Length, path2.Length);
        int compInd = 0;
        while (compInd < minlen && path1[compInd] == path2[compInd])
        {
            compInd++;
        }

        int current = path1.IndexOf('.') + 1;
        Area curArea = _worldGraph.GetBaseArea();
        while (current < compInd)
        {
            var slice = path1.Slice(current);
            current = slice.IndexOf('.') + 1;
            curArea = curArea.GetSubArea(slice);
        }
        //now at the deepest common area (curArea)
        return "";
        //TODO: assemble path between adjacent and other areas or throw if no connection found, maybe use djikstras or dfs or similar
    }
}