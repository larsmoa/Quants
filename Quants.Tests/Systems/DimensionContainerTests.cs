using System;
using Moq;
using NUnit.Framework;
using Quants;
using Quants.Systems;

namespace Tests.Quants.Systems
{
    // Moq doesn't support mocking delegates, but creating an interface is a good workaround
    public interface ICreateAdjacentValueConverterDelegateFunctor
    {
        // Fulfills DimensionContainer.CreateAdjacentValueConverterDelegate
        IValueConverter Create(IUnit source, IUnit target);
    }

    [TestFixture]
    public class DimensionContainerTests
    {
        [Test]
        public void AddUnit_BaseunitNotRegistred_ThrowsNullReferenceException()
        {
            // Arrange
            var mockDelegate = CreateMockDelegate();
            IDimension dimension = CreateStubDimension("M");
            IUnit baseunit = CreateStubUnit("g", dimension);
            IUnit scaledunit = CreateStubUnit("kg", dimension);
            DimensionContainer container = new DimensionContainer(dimension, mockDelegate.Object.Create);

            // Act/Assert
            Assert.Throws<NullReferenceException>(() => container.AddUnit(baseunit, scaledunit));
        }

        [Test]
        public void AddUnit_DimensionMismatch_ThrowsArgumentException()
        {
            // Arrange
            var mockDelegate = new Mock<ICreateAdjacentValueConverterDelegateFunctor>();
            IDimension mass = CreateStubDimension("M");
            IDimension length = CreateStubDimension("L");
            IUnit kg = CreateStubUnit("kg", mass);
            IUnit m = CreateStubUnit("m", length);
            DimensionContainer container = new DimensionContainer(mass, mockDelegate.Object.Create);
            container.SetBaseUnit(kg);

            // Act/Assert
            Assert.Throws<ArgumentException>(() => container.AddUnit(kg, m));            
        }

        [Test]
        public void CreateConverter_BaseUnitToDirectAnchestor_CallsCreateConverterOnce()
        {
            // Arrange
            var mockDelegate = CreateMockDelegate();
            IDimension mass = CreateStubDimension("M");
            IUnit kg = CreateStubUnit("kg", mass);
            IUnit lbs = CreateStubUnit("lbs", mass);

            DimensionContainer container = new DimensionContainer(mass, mockDelegate.Object.Create);
            container.SetBaseUnit(kg);
            container.AddUnit(kg, lbs);

            // Act
            IValueConverter converter = container.CreateConverter(kg, lbs);

            // Assert
            mockDelegate.Verify(x => x.Create(kg, lbs), Times.Once());
            Assert.NotNull(converter);
        }

        [Test]
        public void CreateConverter_BaseUnitToIndirectAnchestor_CallsCreateConverterTwice()
        {
            // Arrange
            var mockDelegate = CreateMockDelegate();
            IDimension mass = CreateStubDimension("M");
            IUnit g = CreateStubUnit("g", mass);
            IUnit kg = CreateStubUnit("kg", mass);
            IUnit lbs = CreateStubUnit("lbs", mass);

            DimensionContainer container = new DimensionContainer(mass, mockDelegate.Object.Create);
            container.SetBaseUnit(g);
            container.AddUnit(g, kg);
            container.AddUnit(kg, lbs);

            // Act
            IValueConverter converter = container.CreateConverter(g, lbs);

            // Assert
            mockDelegate.Verify(x => x.Create(g, kg), Times.Once());
            mockDelegate.Verify(x => x.Create(kg, lbs), Times.Once());
            Assert.NotNull(converter);
        }

        [Test]
        public void CreateConverter_UnitToAnchestor_CallsCreateConverterTwice()
        {
            // Arrange
            var mockDelegate = CreateMockDelegate();
            IDimension mass = CreateStubDimension("M");
            IUnit g = CreateStubUnit("g", mass);
            IUnit kg = CreateStubUnit("kg", mass);
            IUnit lbs = CreateStubUnit("lbs", mass);

            DimensionContainer container = new DimensionContainer(mass, mockDelegate.Object.Create);
            container.SetBaseUnit(g);
            container.AddUnit(g, kg);
            container.AddUnit(kg, lbs);

            // Act
            IValueConverter converter = container.CreateConverter(kg, lbs);

            // Assert
            mockDelegate.Verify(x => x.Create(It.IsAny<IUnit>(), It.IsAny<IUnit>()), Times.Exactly(1));
            Assert.NotNull(converter);
        }

        [Test]
        public void CreateConverter_UnitsShareParent_CallsCreateConverterTwice()
        {
            // Arrange
            var mockDelegate = CreateMockDelegate();
            IDimension mass = CreateStubDimension("M");
            IUnit g = CreateStubUnit("g", mass);
            IUnit kg = CreateStubUnit("kg", mass);
            IUnit lbs = CreateStubUnit("lbs", mass);

            DimensionContainer container = new DimensionContainer(mass, mockDelegate.Object.Create);
            container.SetBaseUnit(g);
            container.AddUnit(g, kg);
            container.AddUnit(g, lbs);

            // Act
            IValueConverter converter = container.CreateConverter(kg, lbs);

            // Assert
            mockDelegate.Verify(x => x.Create(kg, g), Times.Once());
            mockDelegate.Verify(x => x.Create(g, lbs), Times.Once());
            Assert.NotNull(converter);
        }

        [Test]
        public void Clone_EmptyContainer_ReturnsEmptyContainer()
        {
            // Arrange
            var mockDelegate = CreateMockDelegate();
            IDimension dimension = CreateStubDimension();
            DimensionContainer original = new DimensionContainer(dimension, mockDelegate.Object.Create);

            // Act
            DimensionContainer cloned = (DimensionContainer)original.Clone();

            // Assert
            Assert.NotNull(cloned);
            Assert.AreSame(original.Dimension, cloned.Dimension);
        }

        [Test]
        public void Clone_NonEmptyContainer_ReturnsContainerWithSameUnits()
        {
            // Arrange
            var mockDelegate = CreateMockDelegate();
            IDimension mass = CreateStubDimension("M");
            IUnit g = CreateStubUnit("g", mass);
            IUnit kg = CreateStubUnit("kg", mass);
            IUnit lbs = CreateStubUnit("lbs", mass);

            DimensionContainer original = new DimensionContainer(mass, mockDelegate.Object.Create);
            original.SetBaseUnit(g);
            original.AddUnit(g, kg);
            original.AddUnit(g, lbs);

            // Act
            DimensionContainer cloned = (DimensionContainer) original.Clone();

            // Assert
            Assert.AreSame(original.BaseUnit, cloned.BaseUnit);
            CollectionAssert.AreEquivalent(original.Units, cloned.Units);
            Assert.True(cloned.CanConvert(g, kg));
            Assert.True(cloned.CanConvert(g, lbs));
            Assert.True(cloned.CanConvert(kg, lbs));

        }

        private static Mock<ICreateAdjacentValueConverterDelegateFunctor> CreateMockDelegate()
        {
            Func<IUnit, IUnit, IValueConverter> factory = 
                (source, target) =>
                {
                    var mockConverter = new Mock<IValueConverter>();
                    mockConverter.Setup(x => x.Source).Returns(source);
                    mockConverter.Setup(x => x.Target).Returns(target);
                    return mockConverter.Object;
                };
            var mockDelegate = new Mock<ICreateAdjacentValueConverterDelegateFunctor>();
            mockDelegate.Setup(x => x.Create(It.IsAny<IUnit>(), It.IsAny<IUnit>())).Returns(factory);
            return mockDelegate;
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

    }
}
