using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using Quants;
using Quants.Systems;

namespace Tests.Quants.Systems
{
    [TestFixture]
    public class UnitSystemTests
    {
        [Test]
        public void AddDimension_ValidDimension_DoesNotThrow()
        {
            // Arrange
            UnitSystem system = new UnitSystem();
            IDimension dimension = CreateStubDimension("M");            
            // Act/assert
            Assert.DoesNotThrow(() => system.AddDimension(dimension));
        }

        [Test]
        public void AddDimension_AllreadyAddedDimension_ThrowsArgumentException()
        {
            // Arrange
            UnitSystem system = new UnitSystem();
            IDimension dimension = CreateStubDimension("M");
            system.AddDimension(dimension);
            // Act/assert
            Assert.Throws<ArgumentException>(() => system.AddDimension(dimension));
        }

        [Test]
        public void CreateConverter_AnchestorUnits_CanCreateConverters()
        {
            // Arrange
            var mockConverter = new Mock<IValueConverter>();
            UnitSystem system = new UnitSystem();
            IDimension dimension = CreateStubDimension("M");
            IUnit baseunit = CreateStubUnit("base", dimension);
            IUnit scaledunit = CreateStubUnit("scaled", dimension);
            system.AddDimension(dimension);
            system.AddBaseUnit(baseunit);
            system.AddScaledUnit(baseunit, scaledunit, mockConverter.Object);

            // Act
            IValueConverter toScaledConverter = system.CreateConverter(baseunit, scaledunit);
            IValueConverter fromScaledConverter = system.CreateConverter(scaledunit, baseunit);

            // Assert
            Assert.NotNull(toScaledConverter);
            Assert.NotNull(fromScaledConverter);
        }

        [Test]
        public void CreateConverter_IndirectlyRelatedUnits_CanCreateConverters()
        {
            // Arrange
            UnitSystem system = new UnitSystem();
            IDimension dimension = CreateStubDimension("M");
            IUnit baseunit = CreateStubUnit("base", dimension);
            IUnit scaledunit1 = CreateStubUnit("scaled1", dimension);
            IUnit scaledunit2 = CreateStubUnit("scaled2", dimension);
            system.AddDimension(dimension);
            system.AddBaseUnit(baseunit);
            // Conversion between scaleduni1 and scaledunit2 through baseunit
            system.AddScaledUnit(baseunit, scaledunit1, CreateStubConverter(baseunit, scaledunit1));
            system.AddScaledUnit(baseunit, scaledunit2, CreateStubConverter(baseunit, scaledunit2));

            // Act
            IValueConverter converter = system.CreateConverter(scaledunit1, scaledunit2);

            // Assert
            Assert.NotNull(converter);
        }

        [Test]
        public void Clone_EmptySystem_ReturnsEmptySystem()
        {
            // Arrange
            UnitSystem system = new UnitSystem();

            // Act
            UnitSystem clone = (UnitSystem) system.Clone();

            // Assert
            Assert.NotNull(clone);
            CollectionAssert.IsEmpty(clone.Dimensions);
        }

        [Test]
        public void Clone_SingleDimensionSystem_ResultHasSameDimension()
        {
            // Arrange
            var mockConverter = new Mock<IValueConverter>();
            UnitSystem system = new UnitSystem();
            IDimension dimension = CreateStubDimension("M");
            IUnit baseunit = CreateStubUnit("base", dimension);
            IUnit scaledunit = CreateStubUnit("scaled", dimension);
            system.AddDimension(dimension);
            system.AddBaseUnit(baseunit);
            system.AddScaledUnit(baseunit, scaledunit, mockConverter.Object);

            // Act
            UnitSystem clone = (UnitSystem) system.Clone();

            // Assert
            CollectionAssert.AreEquivalent(system.Dimensions, clone.Dimensions);
        }

        [Test]
        public void CreateFrom_IUnitSystem_AccessesDimensionsAndUnits()
        {
            // Arrange
            const int dimensions = 3;
            const int unitsperdimension = 3;

            IDimension[] dimensionArray = new []
                                              {
                                                  CreateStubDimension(),
                                                  CreateStubDimension(),
                                                  CreateStubDimension()
                                              };

            var mockSystem = new Mock<IUnitSystem>();
            mockSystem.Setup(x => x.Dimensions)
                .Returns(dimensionArray);
            mockSystem.Setup(x => x.GetBaseUnit(It.IsAny<IDimension>()))
                .Returns<IDimension>(x => CreateStubUnit("", x));
            mockSystem.Setup(x => x.GetSupportedUnits(It.IsAny<IDimension>()))
                .Returns<IDimension>(x => Enumerable.Repeat(CreateStubUnit("", x), unitsperdimension));
            
            // Act
            UnitSystem system = UnitSystem.CreateFrom(mockSystem.Object);

            // Assert
            Assert.NotNull(system);
            mockSystem.Verify(x => x.Dimensions, Times.AtLeastOnce());
            mockSystem.Verify(x => x.GetBaseUnit(It.IsAny<IDimension>()), Times.AtLeast(dimensions));
            mockSystem.Verify(x => x.GetSupportedUnits(It.IsAny<IDimension>()), Times.AtLeast(dimensions));
        }

        private static IDimension CreateStubDimension(string name = "")
        {
            var stubDimension = new Mock<IDimension>();
            stubDimension.Setup(x => x.Symbol).Returns(name);
            stubDimension.Setup(x => x.GetHashCode()).Returns(name.GetHashCode());
            stubDimension.Setup(x => x.Equals(stubDimension.Object)).Returns(true);
            stubDimension.Setup(x => x.ToString()).Returns(name);
            return stubDimension.Object;
        }

        private static IUnit CreateStubUnit(string unit = "", IDimension dimension = null)
        {
            if (dimension == null)
                dimension = Dimsensionless.Instance;
            var stubUnit = new Mock<IUnit>();
            stubUnit.Setup(x => x.Unit).Returns(unit);
            stubUnit.Setup(x => x.ToString()).Returns(unit);
            stubUnit.Setup(x => x.Equals(stubUnit.Object)).Returns(true);
            stubUnit.Setup(x => x.GetHashCode()).Returns(unit.GetHashCode());
            stubUnit.Setup(x => x.Dimension).Returns(dimension);
            return stubUnit.Object;
        }

        private static IValueConverter CreateStubConverter(IUnit source, IUnit target)
        {
            var mockConverter = new Mock<IValueConverter>();
            mockConverter.Setup(x => x.Source).Returns(source);
            mockConverter.Setup(x => x.Target).Returns(target);
            mockConverter.Setup(x => x.Inversed()).Returns(() => CreateStubConverter(target, source));
            return mockConverter.Object;
        }
    }
}
