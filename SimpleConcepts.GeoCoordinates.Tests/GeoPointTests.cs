using System;
using Xunit;

namespace SimpleConcepts.GeoCoordinates.Tests
{
    public class GeoPointTests
    {
        [Fact]
        public void Zero_HasLatitudeZeroAndLongitudeZero()
        {
            // Arrange

            // Act
            var result = GeoPoint.Zero;

            // Assert
            Assert.Equal(0, result.Longitude);
            Assert.Equal(0, result.Latitude);
        }

        [Fact]
        public void Constructor_WithValidCoordinates_CreatesInstance()
        {
            // Arrange

            // Act
            var result = new GeoPoint(-10, 20);

            // Assert
            Assert.Equal(-10, result.Longitude);
            Assert.Equal(20, result.Latitude);
        }

        [Theory]
        [InlineData(-91)]
        [InlineData(91)]
        public void Constructor_WithInvalidLatitude_Throws(double value)
        {
            // Arrange

            // Act
            void act()
            {
                var x = new GeoPoint(10, value);
            }

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(act);
        }

        [Theory]
        [InlineData(-181)]
        [InlineData(181)]
        public void Constructor_WithInvalidLongitude_Throws(double value)
        {
            // Arrange

            // Act
            void act()
            {
                var x = new GeoPoint(value, 10);
            }

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(act);
        }

        [Fact]
        public void ImplicitCastToDoubleArray_WithInput_ReturnsExpectedValue()
        {
            // Arrange
            var input = new GeoPoint(-43.2126812, -22.951911);

            // Act
            double[] result = input;

            // Assert
            Assert.Equal(new[] { -43.2126812, -22.951911 }, result);
        }

        [Fact]
        public void ImplicitCastFromDoubleArray_WithInput_ReturnsExpectedValue()
        {
            // Arrange
            var input = new[] { -43.2126812, -22.951911 };

            // Act
            GeoPoint result = input;

            // Assert
            Assert.Equal(-43.2126812, result.Longitude);
            Assert.Equal(-22.951911, result.Latitude);
        }

        [Fact]
        public void SubtractionOperator_WithInput_ReturnsExpectedValue()
        {
            // Arrange
            var point1 = new GeoPoint(-43.2126812, -22.951911);
            var point2 = new GeoPoint(-43.1589638, -22.9492483);

            var x = new GeoRegion(new GeoPoint(10, 10), new GeoPoint(10.1, 10.1), new GeoPoint(10.2, 10.2), new GeoPoint(10.0, 10.0));

            // Act
            var shift = point2 - point1;

            // Assert
            Assert.Equal(5508.24386676248, shift.Distance);
        }

        [Fact]
        public void Equals_WithEqualValues_ReturnsFalse()
        {
            // Arrange
            var point1 = new GeoPoint(-43.2126812, -22.951911);
            var point2 = new GeoPoint(-43.2126812, -22.951911);

            // Act
            var result = point1.Equals((object)point2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Equals_WithDifferentValues_ReturnsFalse()
        {
            // Arrange
            var point1 = new GeoPoint(-43.2126812, -22.951911);
            var point2 = new GeoPoint(-43.1589638, -22.9492483);

            // Act
            var result = point1.Equals((object)point2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ToString_WithNoCustomDecimalDigits_ReturnsExpectedValue()
        {
            // Arrange
            var input = new GeoPoint(-43.212681212, -22.951911);

            // Act
            var result = input.ToString();

            // Assert
            Assert.Equal("-43.2126812,-22.9519110", result);
        }

        [Fact]
        public void ToString_WithCustomDecimalDigits_ReturnsExpectedValue()
        {
            // Arrange
            var input = new GeoPoint(-43.2126812, -22.951911);

            // Act
            var result = input.ToString(3);

            // Assert
            Assert.Equal("-43.213,-22.952", result);
        }
    }
}
