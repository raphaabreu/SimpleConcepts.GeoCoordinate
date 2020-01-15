using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SimpleConcepts.GeoCoordinates
{
    [DebuggerDisplay("Vertices = {_shape.Length - 1}, Area = {Area} m²")]
    public struct GeoRegion : IEquatable<GeoRegion>
    {
        private readonly GeoPoint[] _shape;

        public IEnumerable<GeoPoint> Shape => _shape;

        public static GeoRegion Empty => new GeoRegion();

        public GeoRegion(IEnumerable<GeoPoint> shape)
        {
            if (shape is null)
            {
                throw new ArgumentNullException(nameof(shape));
            }

            var points = Array.AsReadOnly(shape.ToArray()).ToArray();

            if (points.Length < 3)
            {
                throw new ArgumentException("Value must contain at least 3 points.", nameof(shape));
            }

            if (points.First() != points.Last())
            {
                throw new ArgumentException("The shape must start and finish on the same point.", nameof(shape));
            }

            _shape = points;
        }

        public GeoRegion(params GeoPoint[] shape) : this(shape.AsEnumerable())
        {

        }

        public double Area
        {
            get
            {
                double area = 0;

                for (var i = 0; i < _shape.Length - 1; i++)
                {
                    var p1 = _shape[i];
                    var p2 = _shape[i + 1];

                    area += Util.ToRadians(p2.Longitude - p1.Longitude) * (2 + Math.Sin(Util.ToRadians(p1.Latitude)) + Math.Sin(Util.ToRadians(p2.Latitude)));
                }

                area = area * Math.Pow(Util.EQUATORIAL_RADIUS, 2) / 2;

                return Math.Abs(area);
            }
        }

        public GeoPoint Center
        {
            get
            {
                var points = _shape.ToArray();

                var minLat = points.Min(p => p.Latitude);
                var minLon = points.Min(p => p.Longitude);
                var maxLat = points.Max(p => p.Latitude);
                var maxLon = points.Max(p => p.Longitude);

                return new GeoPoint((maxLon - minLon) / 2 + minLon, (maxLat - minLat) / 2 + minLat);
            }
        }

        public GeoPoint Centroid => GeoPoint.Zero;

        public bool Contains(GeoPoint point)
        {
            //TODO

            return false;
        }

        public bool Contains(GeoRegion region)
        {
            return region.Shape.All(Contains);
        }

        public bool Intersects(GeoRegion region)
        {
            //TODO
            return false;
        }

        public bool Equals(GeoRegion other)
        {
            if (other._shape is null)
            {
                return _shape is null;
            }

            if (other._shape.Length != _shape.Length)
            {
                return false;
            }

            return !_shape.Where((t, i) => other._shape[i] != t).Any();
        }

        public override bool Equals(object obj)
        {
            return obj is GeoRegion other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var code = 0;

                foreach (var point in _shape)
                {
                    code = (code.GetHashCode() * 397) ^ point.GetHashCode();
                }

                return code;
            }
        }

        public static bool operator ==(GeoRegion left, GeoRegion right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GeoRegion left, GeoRegion right)
        {
            return !(left == right);
        }
    }
}
