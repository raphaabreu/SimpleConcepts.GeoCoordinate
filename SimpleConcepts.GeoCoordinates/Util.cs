using System;

namespace SimpleConcepts.GeoCoordinates
{
    internal static class Util
    {
        public const double EQUATORIAL_RADIUS = 6371000;

        public static double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        public static double ToDegrees(double radians)
        {
            return radians * 180 / Math.PI;
        }
    }
}
