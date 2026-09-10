
using System.Text.Json.Serialization;
using Kjarni;


namespace WorldMapLib;


public class Area
{
    public string Name { get; private set; } = "";
    public string Description { get; private set; } = "";
    /// <summary>
    /// Does not have to be exact, approximate coordinates of the area.
    /// </summary>
    public GpsCoord Coordinates { get; private set; }

    [JsonIgnore]
    public string AreaPath
    {
        get
        {
            if (_parent == null) { return ""; }
            return _parent.AreaPath + Name;
        }
    }

    /// <summary>
    /// Objects contained in the area, 
    /// </summary>
    [JsonInclude]
    private List<WorldObject> Objects = new();

    /// <summary>
    /// Subareas contained within an area, for example Earth contains Waterloo contains University of Waterloo contains MC Building contains Classroom 2054
    /// Areas should not be too specific, at minimum should define a space, for example Apartment 3024 contains Kitchen, Bedroom, Bathroom, and Living Room
    /// </summary>
    [JsonInclude]
    private List<Area> SubAreas = new();


    private Area? _parent;


    [JsonConstructor]
    public Area(string name, string description, GpsCoord coordinates, List<WorldObject> objects, List<Area> subAreas)
    {
        Name = name;
        Description = description;
        Coordinates = coordinates;
        SubAreas = subAreas;
        foreach (var area in SubAreas)
        {
            area._parent = this;
        }
        Objects = objects;
    }

    public Area(string name, string description, GpsCoord approxCoords, List<WorldObject> objects) :
        this(name, description, approxCoords, objects, new()) { }
    public Area(string name, string description, GpsCoord approxCoords) :
        this(name, description, approxCoords, new()) { }


    public string ToString(bool withDescription = false)
    {
        if (withDescription)
        {
            return $"Name: {Name}; Description: {Description}";
        }
        else
        {
            return $"Name: {Name}";
        }
    }


    public void AddSubArea(string name, string description, GpsCoord approxCoords, List<WorldObject> Objects)
    {
        SubAreas.Add(new Area(name, description, approxCoords, Objects));
    }
    public void AddSubArea(string name, string description, GpsCoord approxCoords)
    {
        if (SubAreas.Any(t => t.Name == name))
        {
            Console.WriteLine("Warning: added subarea with duplicate name");
        }
        SubAreas.Add(new Area(name, description, approxCoords));
    }

    internal string GetSubAreasAsString(bool verbose = false)
    {
        return string.Join(',', SubAreas.Select(t => t.ToString(verbose)));
    }
    internal Area GetSubArea(string name)
    {
        return SubAreas.First(t => t.Name == name);
    }
    internal Area? GetParent()
    {
        return _parent;
    }

    /// <summary>
    /// Recursively searches for an area in itself and all subareas.
    /// Can use semantic search to find best possible option rather than exact.
    /// </summary>
    /// <param name="name">Identifying name of area</param>
    /// <returns>Area if found or null otherwise, not null on sematic search ever</returns>
    public Area? SearchForSubAreaExact(string name)
    {
        int ind;
        if ((ind = SubAreas.FindIndex(t => t.Name == name)) != -1)
        {
            return SubAreas[ind];
        }
        foreach (Area a in SubAreas)
        {
            Area? area;
            if ((area = a.SearchForSubAreaExact(name)) != null)
            {
                return area;
            }
        }
        return null;
    }
    
    /// <summary>
    /// Searches through all subareas below this one recursively to find the closest semantic match to the given name 
    /// </summary>
    public (Area area, float score) SearchForSubAreaSemantic(string name)
    {
        using var embedder = new Embedder("minilm-l6-v2");
        return SearchForSubAreaSemantic(name, embedder);
    }
    private (Area area, float score) SearchForSubAreaSemantic(string name, Embedder embedder)
    {
        (Area area, float score)[] arr = new (Area area, float score)[(SubAreas.Count)];
        for (int i = 0; i < SubAreas.Count; i++)
        {
            arr[i] = SubAreas[i].SearchForSubAreaSemantic(name, embedder);
        }
        (Area area, float score) bestBelow = arr.MaxBy(t => t.score);
        (Area area, float score) thisScore = (this, embedder.Similarity(name, Name));

        return thisScore.score > bestBelow.score ? thisScore : bestBelow;
    }


    public void AddObjects(List<WorldObject> worldObjects)
    {
        Objects.AddRange(worldObjects);
    }
    public void AddObject(WorldObject worldObject)
    {
        Objects.Add(worldObject);
    }

    internal string GetObjectsAsString(bool verbose = false)
    {
        return string.Join(",", Objects.Select(t => t.ToString(verbose)));
    }
    internal WorldObject GetObject(string name)
    {
        return Objects.First(t => t.Name == name);
    }

    /// <summary>
    /// Recursively searches for an object in itself and all subareas.
    /// </summary>
    /// <param name="name">Identifying name of object</param>
    /// <returns>Object if found or null otherwise, not null on sematic search ever</returns>
    public WorldObject? SearchForObjectExact(string name)
    {
        int areaInd = 0;
        int objectInd = 0;
        if ((areaInd = SubAreas.FindIndex(t => ((objectInd = t.Objects.FindIndex(t => t.Name == name)) != -1))) != -1)
        {
            return SubAreas[areaInd].Objects[objectInd];
        }
        foreach (Area a in SubAreas)
        {
            WorldObject? obj;
            if ((obj = a.SearchForObjectExact(name)) != null)
            {
                return obj;
            }
        }
        return null;
    }

    /// <summary>
    /// Searches for an object in this area and any subareas based off semantics
    /// </summary>
    public (WorldObject obj, float score, string areaPath) SearchForObjectSemantic(string name)
    {
        using var embedder = new Embedder("minilm-l6-v2");
        return SearchForObjectSemantic(name, embedder);
    }
    private (WorldObject obj, float score, string areaPath) SearchForObjectSemantic(string name, Embedder embedder)
    {
        (WorldObject obj, float score)[] arr = new (WorldObject obj, float score)[(Objects.Count)];

        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = (Objects[i], embedder.Similarity(name, Objects[i].Name));
        }

        (WorldObject obj, float score, string areaPath)[] arr2 = new (WorldObject obj, float score, string areaPath)[(SubAreas.Count)];

        for (int i = 0; i < arr2.Length; i++)
        {
            arr2[i] = SubAreas[i].SearchForObjectSemantic(name, embedder);
        }

        var b = arr.MaxBy(t => t.score);
        (WorldObject obj, float score, string areaPath) bestIn = (b.obj, b.score, AreaPath);
        var bestUnder = arr2.Length > 0 ? arr2.MaxBy(t => t.score) : bestIn;

        return bestIn.score > bestUnder.score ? bestIn : bestUnder;
    }
}