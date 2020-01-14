using System;
using System.Diagnostics;
using System.Globalization;

namespace SimpleConcepts.GeoCoordinates
{
    /// <summary>
    /// Represents a point on Earth's surface using the WGS 84 reference frame.
    /// </summary>
    [DebuggerDisplay("λ = {Longitude}, φ = {Latitude}")]
    public struct GeoPoint : IEquatable<GeoPoint>
    {
        /// <summary>
        /// Latitude in decimal form ranging from -90 to 90.
        /// </summary>
        public double Latitude { get; }

        /// <summary>
        /// Longitude in decimal form ranging from -180 to 180.
        /// </summary>
        public double Longitude { get; }

        /// <summary>
        /// Represents the 0,0 coordinate.
        /// </summary>
        public static GeoPoint Zero { get; } = new GeoPoint(0, 0);

        /// <summary>
        /// Creates a new GeoPoint.
        /// </summary>
        /// <param name="longitude">Longitude in decimal form ranging from -180 to 180.</param>
        /// <param name="latitude">Latitude in decimal form ranging from -90 to 90.</param>
        public GeoPoint(double longitude, double latitude)
        {
            if (latitude > 90 || latitude < -90)
            {
                throw new ArgumentOutOfRangeException(nameof(latitude), latitude, "Value must be between -90 and +90.");
            }
            if (longitude > 180 || longitude < -180)
            {
                throw new ArgumentOutOfRangeException(nameof(longitude), longitude, "Value must be between -180 and +180.");
            }

            Longitude = longitude;
            Latitude = latitude;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is GeoPoint other && Equals(other);
        }

        /// <inheritdoc />
        public bool Equals(GeoPoint other)
        {
            return this == other;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (Latitude.GetHashCode() * 397) ^ Longitude.GetHashCode();
            }
        }

        /// <summary>
        /// Indicates if both coordinates represent the same point on Earth.
        /// </summary>
        /// <param name="left">The left <see cref="GeoPoint"/> to compare</param>
        /// <param name="right">The right <see cref="GeoPoint"/> to compare</param>
        /// <returns>True if they represent the same right on Earth, false otherwise.</returns>
        public static bool operator ==(GeoPoint left, GeoPoint right)
        {
            return Math.Abs(left.Latitude - right.Latitude) < double.Epsilon
                   && Math.Abs(left.Longitude - right.Longitude) < double.Epsilon;
        }

        /// <summary>
        /// Indicates if coordinates represent different points on Earth.
        /// </summary>
        /// <param name="left">The left <see cref="GeoPoint"/> to compare</param>
        /// <param name="right">The right <see cref="GeoPoint"/> to compare</param>
        /// <returns>True if they represent different points on Earth, false otherwise.</returns>
        public static bool operator !=(GeoPoint left, GeoPoint right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Calculates the great-circle distance between both points on Earth.
        /// </summary>
        /// <param name="left">The left <see cref="GeoPoint"/> reference point</param>
        /// <param name="right">The right <see cref="GeoPoint"/> reference point</param>
        /// <returns>The distance between the <paramref name="left"/> and <paramref name="right"/> points measured in meters.</returns>
        public static double operator -(GeoPoint left, GeoPoint right)
        {
            var a = (left.Latitude - right.Latitude) * (Math.PI / 180.0);
            var b = (left.Longitude - right.Longitude) * (Math.PI / 180.0);
            var c = right.Latitude * (Math.PI / 180.0);
            var d = left.Latitude * (Math.PI / 180.0);
            var e = Math.Sin(a / 2.0) * Math.Sin(a / 2.0) + Math.Sin(b / 2.0) * Math.Sin(b / 2.0) * Math.Cos(c) * Math.Cos(d);
            return 6371000.0 * (2.0 * Math.Atan2(Math.Sqrt(e), Math.Sqrt(1.0 - e)));
        }

        /// <summary>
        /// Converts the given <see cref="GeoPoint"/> to <c>double[2]</c> where the first element is the longitude and the second is the latitude.
        /// </summary>
        /// <param name="point">The <see cref="GeoPoint"/> to convert</param>
        public static implicit operator double[](GeoPoint point)
        {
            return new[] { point.Longitude, point.Latitude };
        }

        /// <summary>
        /// Converts the given <c>double[2]</c> into a <see cref="GeoPoint"/> where the first element is the longitude and the second is the latitude.
        /// </summary>
        /// <param name="coordinates">The array with the coordinates to convert</param>
        public static implicit operator GeoPoint(double[] coordinates)
        {
            if (coordinates == null)
            {
                throw new ArgumentNullException(nameof(coordinates));
            }
            if (coordinates.Length < 2)
            {
                throw new ArgumentException("Array must contain two elements", nameof(coordinates));
            }

            return new GeoPoint(coordinates[0], coordinates[1]);
        }

        /// <summary>
        /// Returns the coordinates represented by this instance.
        /// </summary>
        public override string ToString()
        {
            return ToString(7);
        }

        /// <summary>
        /// Returns the coordinates represented by this instance in the specified format.
        /// </summary>
        /// <param name="decimalDigits">The number of decimal digits to format</param>
        public string ToString(ushort decimalDigits)
        {
            return $"{Longitude.ToString($"F{decimalDigits}", CultureInfo.InvariantCulture)},{Latitude.ToString($"F{decimalDigits}", CultureInfo.InvariantCulture)}";
        }
    }
}
