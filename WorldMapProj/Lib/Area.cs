
namespace WorldMap;


public class Area
{
    public string Description { get; private set; }
    public GpsCoord Coordinates { get; private set; }
    public List<Area> SubAreas { get; private set; }
    public List<WorldObject> Objects { get; private set; }
}