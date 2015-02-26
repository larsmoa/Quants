using System;
using Moa.Units;
using Moa.Units.Conversion;
using Moq;
using NUnit.Framework;

namespace Tests.Moa.Units.Conversion
{
    [TestFixture]
    public class CompositeConverterTests
    {
        [Test]
        public void AddConverter_MatchingUnits_DoesNotThrow()
        {
            // Arrange
            var atob = CreateMockConverter("a", "b");
            var btoc = CreateMockConverter("b", "c");
            // Act/Assert
            CompositeConverter converter = new CompositeConverter();
            converter.AddConverter(atob.Object);
            Assert.DoesNotThrow(() => converter.AddConverter(btoc.Object));
        }

        [Test]
        public void AddConverter_MismatchingUnits_ThrowsArgumentException()
        {
            // Arrange
            var atob = CreateMockConverter("a", "b");
            var ctod = CreateMockConverter("c", "d");
            // Act/Assert
            CompositeConverter converter = new CompositeConverter();
            converter.AddConverter(atob.Object);
            Assert.Throws<ArgumentException>(() => converter.AddConverter(ctod.Object));
        }

        [Test]
        public void Convert_SingleStep_InvokesConvert()
        {
            // Arrange
            var mockConverter = CreateMockConverter("a", "b");
            CompositeConverter converter = new CompositeConverter();
            converter.AddConverter(mockConverter.Object);
            // Act
            converter.Convert(1.0);
            // Assert
            mockConverter.Verify(x => x.Convert(It.IsAny<double>()), Times.Once());
        }


        [Test]
        public void Convert_TwoSteps_InvokesConvert()
        {
            // Arrange
            var mockAtoB = CreateMockConverter("a", "b");
            var mockBtoC = CreateMockConverter("b", "c");
            CompositeConverter converter = new CompositeConverter();
            converter.AddConverter(mockAtoB.Object);
            converter.AddConverter(mockBtoC.Object);
            // Act
            converter.Convert(1.0);
            // Assert
            mockAtoB.Verify(x => x.Convert(It.IsAny<double>()), Times.Once());
            mockBtoC.Verify(x => x.Convert(It.IsAny<double>()), Times.Once());
        }

        private static Mock<IValueConverter> CreateMockConverter(string source, string target)
        {
            IUnit sourceUnit = CreateStubUnit(source);
            IUnit targetUnit = CreateStubUnit(target);

            var converter = new Mock<IValueConverter>();
            converter.Setup(x => x.Source).Returns(sourceUnit);
            converter.Setup(x => x.Target).Returns(targetUnit);
            converter.Setup(x => x.Inversed()).Returns(() => CreateMockConverter(target, source).Object);
            return converter;
        }

        private static IUnit CreateStubUnit(string unit)
        {
            var stub = new Mock<IUnit>();
            stub.Setup(x => x.Unit).Returns(unit);
            stub.Setup(x => x.GetHashCode()).Returns(unit.GetHashCode());
            stub.Setup(x => x.ToString()).Returns(unit);
            // Equal if unit-string is equal
            stub.Setup(x => x.Equals(It.IsAny<object>()))
                .Returns<object>((x) => (x is IUnit) ? (((IUnit) x).Unit == unit) : false);
            stub.Setup(x => x.Equals(It.IsAny<IUnit>()))
                .Returns<IUnit>((x) => (x != null) ? (x.Unit == unit) : false);
            return stub.Object;
        }
    }
}
