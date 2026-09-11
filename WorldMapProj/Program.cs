using System.Text.Json;
using WorldMapLib;

internal class Program
{
    public static void Main()
    {
        WorldGraph graph = new();


        //graph.GetBaseArea().AddObjectsUnsafe(new List<WorldObject>
        //{
        //    new WorldObject("mreeeow", "a cat likely", Volatility.High, new GpsCoord(0, 0))
        //});

        graph.GetBaseArea().AddSubArea("kitchen", "a kitchen that contains many cooking utensils and similar", new GpsCoord(0,0), new List<WorldObject>
        {
            new WorldObject("black cat", "mreooww", Volatility.High, new GpsCoord(0, 0))
            , new WorldObject("black garbage can", "full of trash", Volatility.Low, new GpsCoord(0, 0))
            , new WorldObject("Phone with black case", "Leather case on small phone", Volatility.High, new GpsCoord(0, 0))
            , new WorldObject("Tissue box", "sunflower pattern on the outside", Volatility.Medium, new GpsCoord(0, 0))
            , new WorldObject("Brown box of cables", "contains assorted cables", Volatility.Medium, new GpsCoord(0, 0))
        });

        string json = graph.SerializeToJsonString(true);
        Console.WriteLine(json);


        //string objectPrompt = "Tissues";
        //while ((objectPrompt = Console.ReadLine()!) != "exit")
        //{
        //    Console.WriteLine($"Object found: {graph.GetBaseArea().SearchForObjectSemantic(objectPrompt)}");
        //}


        WorldGraph rerun = new WorldGraph(json);
        string rerunStr = rerun.SerializeToJsonString(true);
        Console.WriteLine((rerunStr == json ? "Successful" : "Failed") + " round trip.");

        rerun.GetBaseArea().TryAddObjects(
            """
                  [
              {
                "Name": "black cat",
                "Description": "mreooww",
                "ApproxCoordinates": {},
                "ObjectVolatility": 2
              },
              {
                "Name": "black garbage can",
                "Description": "full of trash",
                "ApproxCoordinates": {},
                "ObjectVolatility": 0
              },
              {
                "Name": "Phone with black case",
                "Description": "Leather case on small phone",
                "ApproxCoordinates": {},
                "ObjectVolatility": 2
              },
              {
                "Name": "Tissue box",
                "Description": "sunflower pattern on the outside",
                "ApproxCoordinates": {},
                "ObjectVolatility": 1
              },
              {
                "Name": "Brown box of cables",
                "Description": "contains assorted cables",
                "ApproxCoordinates": {},
                "ObjectVolatility": 1
              }
            ]
            """
            );
        Console.WriteLine(rerun.SerializeToJsonString(true));
    }
}