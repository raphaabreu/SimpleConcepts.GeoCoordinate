using System;
using System.Diagnostics;

namespace SimpleConcepts.GeoCoordinates
{
    [DebuggerDisplay("{Distance} meters on {Heading}° N")]
    public struct GeoShift : IEquatable<GeoShift>
    {
        public double Distance { get; }
        public double Heading { get; }

        public static GeoShift Zero { get; } = new GeoShift(0, 0);

        public GeoShift(double distance, double heading)
        {
            Distance = distance;
            Heading = heading;
        }

        public bool Equals(GeoShift other)
        {
            return Distance.Equals(other.Distance) && Heading.Equals(other.Heading);
        }

        public override bool Equals(object obj)
        {
            return obj is GeoShift other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Distance.GetHashCode() * 397) ^ Heading.GetHashCode();
            }
        }

        public static bool operator ==(GeoShift left, GeoShift right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GeoShift left, GeoShift right)
        {
            return !(left == right);
        }

        public static GeoShift operator *(GeoShift left, double right)
        {
            return new GeoShift(left.Distance * right, left.Heading);
        }

        public static GeoShift operator /(GeoShift left, double right)
        {
            return new GeoShift(left.Distance / right, left.Heading);
        }

        public static GeoShift operator +(GeoShift left, GeoShift right)
        {
            return left;
        }

        public static GeoShift operator -(GeoShift left, GeoShift right)
        {
            return left + right * -1;
        }
    }
}
