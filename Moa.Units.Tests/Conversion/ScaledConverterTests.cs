using System;
using Moa.Units;
using Moa.Units.Conversion;
using Moq;
using NUnit.Framework;

namespace Tests.Moa.Units.Conversion
{
    [TestFixture]
    public class ScaledConverterTests
    {
        [Test]
        public void Convert_Identity_ReturnsEqual()
        {
            // Arrange
            IUnit source = new Mock<IUnit>().Object;
            IUnit target = new Mock<IUnit>().Object;
            ScaledConverter converter = new ScaledConverter(1.0, 0.0, source, target);
            // Act/Assert
            Random random = new Random(12939123);
            for (int i = 0; i < 100; ++i)
            {
                double value = random.NextDouble();
                Assert.AreEqual(value, converter.Convert(value));
            }
        }

        [Test]
        public void Convert_ScaleHalf_ResultIsHalf()
        {
            // Arrange
            IUnit source = new Mock<IUnit>().Object;
            IUnit target = new Mock<IUnit>().Object;
            ScaledConverter converter = new ScaledConverter(0.5, 0.0, source, target);
            // Act
            double result = converter.Convert(100.0);
            // Assert
            Assert.AreEqual(50.0, result);
        }

        [Test]
        public void Convert_CelciusToFahrenheit_ResultIsCorrect()
        {
            // Arrange
            IUnit source = new Mock<IUnit>().Object;
            IUnit target = new Mock<IUnit>().Object;
            // °F = (°C × 9/5) + 32
            ScaledConverter converter = new ScaledConverter(9.0 / 5.0, 32.0, source, target);            
            // Act/Assert
            Assert.AreEqual(32.0, converter.Convert(0.0));
            Assert.AreEqual(50.0, converter.Convert(10.0));
        }

        [Test]
        public void Convert_CelciusToFahrenheitInversed_ResultIsCorrect()
        {
            // Arrange
            IUnit source = new Mock<IUnit>().Object;
            IUnit target = new Mock<IUnit>().Object;
            // °F = (°C × 9/5) + 32 inversed is °C = (°F − 32) × 5⁄9
            ScaledConverter converter = new ScaledConverter(9.0 / 5.0, 32.0, source, target);
            // Act/Assert
            Assert.AreEqual(0.0, converter.Inversed().Convert(32.0));
            Assert.AreEqual(10.0, converter.Inversed().Convert(50.0));
        }
    }
}
