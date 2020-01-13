using System;
using System.Diagnostics;
using System.Globalization;

namespace SimpleConcepts.GeoCoordinates
{
    [DebuggerDisplay("{Longitude},{Latitude}")]
    public struct GeoPoint
    {
        public double Latitude { get; }
        public double Longitude { get; }

        public static GeoPoint Zero { get; } = new GeoPoint(0, 0);

        public GeoPoint(double longitude, double latitude)
        {
            if (latitude > 90 || latitude < -90)
            {
                throw new ArgumentOutOfRangeException(nameof(latitude), latitude, "Latitude must be between -90 and +90.");
            }
            if (longitude > 180 || longitude < -180)
            {
                throw new ArgumentOutOfRangeException(nameof(longitude), longitude, "Longitude must be between -180 and +180.");
            }

            Longitude = longitude;
            Latitude = latitude;
        }

        public double DistanceTo(GeoPoint other)
        {
            var a = (other.Latitude - Latitude) * (Math.PI / 180.0);
            var b = (other.Longitude - Longitude) * (Math.PI / 180.0);
            var c = Latitude * (Math.PI / 180.0);
            var d = other.Latitude * (Math.PI / 180.0);
            var e = Math.Sin(a / 2.0) * Math.Sin(a / 2.0) + Math.Sin(b / 2.0) * Math.Sin(b / 2.0) * Math.Cos(c) * Math.Cos(d);
            return 6371000.0 * (2.0 * Math.Atan2(Math.Sqrt(e), Math.Sqrt(1.0 - e)));
        }

        public override bool Equals(object obj)
        {
            if (obj is GeoPoint point)
            {
                return this == point;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Latitude.GetHashCode() + Longitude.GetHashCode();
        }

        public static bool operator ==(GeoPoint left, GeoPoint right)
        {
            return Math.Abs(left.Latitude - right.Latitude) < double.Epsilon
                   && Math.Abs(left.Longitude - right.Longitude) < double.Epsilon;
        }

        public static bool operator !=(GeoPoint left, GeoPoint right)
        {
            return !(left == right);
        }

        public static double operator -(GeoPoint left, GeoPoint right)
        {
            return left.DistanceTo(right);
        }

        public override string ToString()
        {
            return $"{Longitude.ToString("F5", CultureInfo.InvariantCulture)},{Latitude.ToString("F5", CultureInfo.InvariantCulture)}";
        }
    }
}
