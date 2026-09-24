
/// <summary>
/// Represents a compass heading measured clockwise from north.
/// </summary>
public class Direction
{
    private const int DegreesWithinCardinal = 20; //degrees until called 'northeast' etc. 
    private float _degrees;


    public Direction(float degreesCWFromNorth)
    {
        SetDegrees(degreesCWFromNorth);
    }
    public Direction(EDirection direction)
    {
        SetDegrees((float)direction);
    }

    public void SetDegrees(float degreesCWFromNorth)
    {
        float trueDeg = degreesCWFromNorth % 360;
        _degrees = trueDeg < 0 ? trueDeg + 360 : trueDeg;
    }

    public void ChangeDegrees(float deltaDegreesCW)
    {
        SetDegrees(_degrees + deltaDegreesCW);
    }

    public float GetDegrees() 
    {
        return _degrees; 
    }

    /// <summary>
    /// Gets a compass-based string representation of the direction eg north, south, south-west.
    /// </summary>
    public string GetDescription()
    {
        switch(_degrees)
        {
            case (< 0): return "invalid";
            case (< DegreesWithinCardinal): return "north";
            case (< 90 - DegreesWithinCardinal): return "north-east";
            case (< 90 + DegreesWithinCardinal): return "east";
            case (< 180 - DegreesWithinCardinal): return "south-east";
            case (< 180 + DegreesWithinCardinal): return "south";
            case (< 270 - DegreesWithinCardinal): return "south-west";
            case (< 270 + DegreesWithinCardinal): return "west";
            case (< 360 - DegreesWithinCardinal): return "north-west";
            case (< 360): return "north";
            default:
                return "invalid";
        }
    }

    public string ToStringSemantic()
    {
        return GetDescription();
    }

    public override string ToString()
    {
        return _degrees.ToString();
    }

    public enum EDirection { North = 0, NorthEast = 45, East = 90, SouthEast = 135, South = 180, SouthWest = 225, West = 270, NorthWest = 315 }
}