using System;
using System.Diagnostics;

namespace SimpleConcepts.GeoCoordinates
{
    [DebuggerDisplay("{Distance} meters on {Heading}° N")]
    public struct GeoVector : IEquatable<GeoVector>
    {
        public double Distance { get; }
        public double Heading { get; }

        public static GeoVector Zero { get; } = new GeoVector(0, 0);

        public GeoVector(double distance, double heading)
        {
            Distance = distance;
            Heading = heading;
        }

        public bool Equals(GeoVector other)
        {
            return Distance.Equals(other.Distance) && Heading.Equals(other.Heading);
        }

        public override bool Equals(object obj)
        {
            return obj is GeoVector other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Distance.GetHashCode() * 397) ^ Heading.GetHashCode();
            }
        }

        public static bool operator ==(GeoVector left, GeoVector right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GeoVector left, GeoVector right)
        {
            return !(left == right);
        }

        public static GeoVector operator *(GeoVector left, double right)
        {
            return new GeoVector(left.Distance * right, left.Heading);
        }

        public static GeoVector operator /(GeoVector left, double right)
        {
            return new GeoVector(left.Distance / right, left.Heading);
        }

        public static GeoVector operator +(GeoVector left, GeoVector right)
        {
            return left;
        }

        public static GeoVector operator -(GeoVector left, GeoVector right)
        {
            return left + right * -1;
        }
    }
}
