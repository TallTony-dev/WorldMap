
using System.Text.Json;

namespace WorldMapLib;


public class WorldGraph
{
    private Area _baseArea;

    internal static float ObjectDuplicateStrictness = 0.8f;

    public WorldGraph()
    {
        _baseArea = new Area("Earth", "Base area object, should contain all subareas", new(0, 0));
    }
    public WorldGraph(string jsonString)
    {
        _baseArea = JsonSerializer.Deserialize<Area>(jsonString)!;
    }

    /// <summary>
    /// Overwrites the file at <paramref name="filePath"/> to contain json rep of the world graph.
    /// </summary>
    public void SaveToFile(string filePath)
    {
        File.Delete(filePath);
        var stream = File.OpenWrite(filePath);
        JsonSerializer.Serialize(stream, _baseArea);
    }
    
    public string SerializeToJsonString(bool pretty = false)
    {
        if (!pretty)
        {
            return JsonSerializer.Serialize(_baseArea);
        }
        else
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            return JsonSerializer.Serialize(_baseArea, options);
        }
    }



    public Area GetBaseArea()
    {
        return _baseArea;
    }

    public Area SearchForAreaSemantically(string name)
    {
        return _baseArea.SearchForSubAreaSemantic(name).area;
    }

    public Area? SearchForAreaExactly(string name)
    {
        return _baseArea.SearchForSubAreaExact(name);
    }
}