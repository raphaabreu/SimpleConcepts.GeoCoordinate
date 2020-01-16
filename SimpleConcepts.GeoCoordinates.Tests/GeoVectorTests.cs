using Xunit;

namespace SimpleConcepts.GeoCoordinates.Tests
{
    public class GeoVectorTests
    {
        [Fact]
        public void MultiplicationOperator_WithInput_ReturnsExpectedValue()
        {
            // Arrange
            var vector = new GeoVector(100, 42);
            var factor = 12.4;

            // Act
            var result = vector * factor;

            // Assert
            Assert.Equal(1240, result.Distance);
            Assert.Equal(42, result.Heading);
        }
    }
}
