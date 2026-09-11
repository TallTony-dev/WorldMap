using System;
using System.Collections;
using System.Text.Json.Serialization;

namespace WorldMapLib
{
    public struct GpsCoord
    {
        public double X { get; set; }
        public double Y { get; set; }

        [JsonConstructor]
        public GpsCoord(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"<{X},{Y}>";
        }
    }
}
