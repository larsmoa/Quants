using NUnit.Framework;
using Quants;
using Quants.Catalogs;
using Quants.Catalogs.SI;

namespace Tests.Quants.Catalogs.SI
{
    [TestFixture]
    public class SiUnitSystemFactoryTests
    {
        [Test]
        public void Create_DoesNotThrow()
        {
            // Arrange
            SiUnitSystemFactory factory = new SiUnitSystemFactory();
            // Act/Assert
            Assert.DoesNotThrow(() => factory.Create());
        }


        [Test]
        public void Create_ResultSupportsAllBaseSiDimensions()
        {
            // Arrange
            SiUnitSystemFactory factory = new SiUnitSystemFactory();
            
            // Act
            IUnitSystem system = factory.Create();
            
            // Assert
            Assert.True(system.SupportsDimension(StandardDimensionsCatalog.AmountOfSubstance));
            Assert.True(system.SupportsDimension(StandardDimensionsCatalog.ElectricCurrent));
            Assert.True(system.SupportsDimension(StandardDimensionsCatalog.Length));
            Assert.True(system.SupportsDimension(StandardDimensionsCatalog.LuminousIntensity));
            Assert.True(system.SupportsDimension(StandardDimensionsCatalog.Mass));
            Assert.True(system.SupportsDimension(StandardDimensionsCatalog.Time));
            Assert.True(system.SupportsDimension(StandardDimensionsCatalog.Temperature));
        }

        [Test]
        public void Create_ResultSupportsAllDerivedSiDimensions()
        {
            // Arrange
            SiUnitSystemFactory factory = new SiUnitSystemFactory();

            // Act
            IUnitSystem system = factory.Create();

            // Assert
            Assert.True(system.SupportsDimension(StandardDimensionsCatalog.Area));
            Assert.True(system.SupportsDimension(StandardDimensionsCatalog.Pressure));
            Assert.True(system.SupportsDimension(StandardDimensionsCatalog.Volume));
            Assert.True(system.SupportsDimension(StandardDimensionsCatalog.Speed));
            Assert.True(system.SupportsDimension(StandardDimensionsCatalog.Force));
            Assert.True(system.SupportsDimension(StandardDimensionsCatalog.Energy));
        }
    }
}
