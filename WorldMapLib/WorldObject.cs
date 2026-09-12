
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
    public GpsCoord ApproxGpsCoordinates { get; private set; } = new(); //might not need or want, could be unneeded context
    public Volatility MovementLikelihood { get; private set; }

    [JsonConstructor]
    public WorldObject(string name, string description, Volatility movementLikelihood, GpsCoord approxGpsCoordinates)
    {
        Name = name;
        Description = description;
        ApproxGpsCoordinates = approxGpsCoordinates;
        MovementLikelihood = movementLikelihood;
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
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Volatility { Low, Medium, High }
