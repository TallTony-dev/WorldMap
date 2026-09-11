
using System.Text.Json.Serialization;

namespace WorldMapLib;

/// <summary>
/// Describes an object in an area
/// </summary>
public class WorldObject
{
    /// <summary>
    /// Name should be verbose enough to identify the object based solely off of it.
    /// Unique identifiers are useful, for example the brand of a coffee machine, 
    /// </summary>
    public string Name { get; private set; }
    public string Description { get; private set; } = "";
    public GpsCoord ApproxCoordinates { get; private set; } //might not need or want, could be unneeded context
    public Volatility ObjectVolatility { get; private set; }

    [JsonConstructor]
    public WorldObject(string name, string description, Volatility objectVolatility, GpsCoord approxCoordinates)
    {
        Name = name;
        Description = description;
        ApproxCoordinates = approxCoordinates;
        ObjectVolatility = objectVolatility;
    }

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

}

/// <summary>
/// Describes how likely an item is to change place or be destroyed,
/// For example a burger or paper coffee cup is highly volatile, a cutting board or coffee machine is medium, a fridge is low
/// </summary>
public enum Volatility { Low, Medium, High }
