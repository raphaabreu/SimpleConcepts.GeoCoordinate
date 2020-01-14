using System.Collections.Generic;

namespace SimpleConcepts.GeoCoordinates
{
    public static class GeoPointExtensions
    {
        /// <summary>
        /// Calculate the average heading for an object that would be moving through the specified <paramref name="path"/>.
        /// </summary>
        /// <param name="path">The path of motion</param>
        /// <returns>The average heading ranging from 0 to 360</returns>
        public static double AverageHeading(this IEnumerable<GeoPoint> path)
        {
            return 0;
        }


    }
}
