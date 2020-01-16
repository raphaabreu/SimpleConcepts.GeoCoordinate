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
        /// Latitude (φ) in degrees form ranging from -90 to 90.
        /// </summary>
        public double Latitude { get; }

        /// <summary>
        /// Longitude (λ) in degrees form ranging from -180 to 180.
        /// </summary>
        public double Longitude { get; }

        /// <summary>
        /// Represents the 0,0 coordinate.
        /// </summary>
        public static GeoPoint Zero { get; } = new GeoPoint(0, 0);

        /// <summary>
        /// Creates a new GeoPoint.
        /// </summary>
        /// <param name="longitude">Longitude (φ) in degrees form ranging from -180 to 180.</param>
        /// <param name="latitude">Latitude (λ) in degrees form ranging from -90 to 90.</param>
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
        /// Calculates the <see cref="GeoVector"/> from the <paramref name="left"/> to the <paramref name="right"/>
        /// </summary>
        /// <param name="left">The left <see cref="GeoPoint"/> reference point</param>
        /// <param name="right">The right <see cref="GeoPoint"/> reference point</param>
        /// <returns>The <see cref="GeoVector"/> between both points.</returns>
        public static GeoVector operator -(GeoPoint left, GeoPoint right)
        {
            return new GeoVector(CalculateDistance(left, right), CalculateHeading(left, right));
        }

        /// <summary>
        /// Adds the given <see cref="GeoVector"/> to the current point returning the final point.
        /// </summary>
        /// <param name="left">The origin <see cref="GeoPoint"/></param>
        /// <param name="right">The <see cref="GeoVector"/> to add</param>
        /// <returns>The final <see cref="GeoPoint"/></returns>
        public static GeoPoint operator +(GeoPoint left, GeoVector right)
        {
            var φ1 = Util.ToRadians(left.Latitude);
            var λ1 = Util.ToRadians(left.Longitude);
            var δ = right.Distance / Util.EQUATORIAL_RADIUS;
            var θ = Util.ToRadians(right.Heading);

            var φ2 = Math.Asin(Math.Sin(φ1) * Math.Cos(δ) +
                               Math.Cos(φ1) * Math.Sin(δ) * Math.Cos(θ));
            var λ2 = λ1 + Math.Atan2(Math.Sin(θ) * Math.Sin(δ) * Math.Cos(φ1),
                         Math.Cos(δ) - Math.Sin(φ1) * Math.Sin(φ2));

            return new GeoPoint(Util.ToDegrees(λ2), Util.ToDegrees(φ2));
        }

        /// <summary>
        /// Adds the given <see cref="GeoVector"/> to the current point returning the final point.
        /// </summary>
        /// <param name="left">The <see cref="GeoVector"/> to add</param>
        /// <param name="right">The origin <see cref="GeoPoint"/></param>
        /// <returns>The final <see cref="GeoPoint"/></returns>
        public static GeoPoint operator +(GeoVector left, GeoPoint right)
        {
            return right + left;
        }

        /// <summary>
        /// Calculates the heading from <paramref name="pointA"/> to <paramref name="pointB"/>.
        /// </summary>
        /// <returns>The heading in degrees</returns>
        private static double CalculateHeading(GeoPoint pointA, GeoPoint pointB)
        {
            var dLon = Util.ToRadians(pointB.Longitude - pointA.Longitude);
            var dPhi = Math.Log(Math.Tan(Util.ToRadians(pointB.Latitude) / 2 + Math.PI / 4) / Math.Tan(Util.ToRadians(pointA.Latitude) / 2 + Math.PI / 4));
            if (Math.Abs(dLon) > Math.PI)
            {
                dLon = dLon > 0 ? -(2 * Math.PI - dLon) : (2 * Math.PI + dLon);
            }

            return (Util.ToDegrees(Math.Atan2(dLon, dPhi)) + 360) % 360;
        }

        /// <summary>
        /// Calculates the distance from <paramref name="pointA"/> to <paramref name="pointB"/>.
        /// </summary>
        /// <returns>The distance in meters</returns>
        private static double CalculateDistance(GeoPoint pointA, GeoPoint pointB)
        {
            var φ1 = Util.ToRadians(pointA.Latitude);
            var φ2 = Util.ToRadians(pointB.Latitude);
            var Δφ = Util.ToRadians(pointB.Latitude - pointA.Latitude);
            var Δλ = Util.ToRadians(pointB.Longitude - pointA.Longitude);

            var a = Math.Sin(Δφ / 2.0) * Math.Sin(Δφ / 2.0) +
                    Math.Sin(Δλ / 2.0) * Math.Sin(Δλ / 2.0) * Math.Cos(φ1) * Math.Cos(φ2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return Util.EQUATORIAL_RADIUS * c;
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
                throw new ArgumentException("Value must contain two elements.", nameof(coordinates));
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
