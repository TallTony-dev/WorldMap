
using System.Text.Json;

namespace WorldMapLib;


public class WorldGraph
{
    public Area CurrentArea { get; set; }

    private Area _baseArea;
    private string _saveName;

    private string SavePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
        , "WorldGraphData", $"{_saveName}/{_saveName}.json");

    internal const float ObjectDuplicateStrictness = 0.5f;


    /// <summary>
    /// Uses an existing save with the name if one exists, otherwise creates a new save.
    /// </summary>
    /// <param name="saveName">The name of the save, store in LocalApplicationData/WorldGraphData</param>
    public WorldGraph(string saveName)
    {
        _saveName = saveName;
        string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
        , "WorldGraphData");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        if (!Directory.Exists(Path.Combine(folderPath, saveName)))
        {
            Directory.CreateDirectory(Path.Combine(folderPath, saveName));
        }

        if (File.Exists(SavePath))
        {
            _baseArea = JsonSerializer.Deserialize<Area>(File.ReadAllText(SavePath))!;
        }
        else
        {
            _baseArea = new Area("Earth", "Base area object, should contain all subareas", new(0, 0));
        }
        CurrentArea = _baseArea;
    }

    /// <summary>
    /// Saves data to LocalApplicationData/WorldGraphData/{saveName passed in constructor}
    /// </summary>
    public void SaveToFile()
    {
        File.Create(SavePath + ".new");
        using var stream = File.OpenWrite(SavePath + ".new");
        JsonSerializer.Serialize(stream, _baseArea);
        stream.Close();
        File.Move(SavePath + ".new", SavePath, true);
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