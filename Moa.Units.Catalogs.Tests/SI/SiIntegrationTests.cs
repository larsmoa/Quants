using NUnit.Framework;
using Quants;
using Quants.Catalogs;
using Quants.Catalogs.SI;
using Quants.Quantities;

namespace Tests.Quants.Catalogs.SI
{
    [TestFixture, Category(Categories.Integrationtest)]
    public class SiIntegrationTests
    {       
        [Test]
        public void LengthTimesLength_Returns_AreaQuantity()
        {
            // Arrange
            SiUnitSystemFactory siFactory = new SiUnitSystemFactory();
            IUnitSystem siSystem = siFactory.Create();
            StandardQuantitiesCatalog.Setup();

            // Act
            Quantity<float> width = new Quantity<float>(2.0f, StandardUnitsCatalog.Meter);
            Quantity<float> height = new Quantity<float>(4.0f, StandardUnitsCatalog.Meter);
            Quantity<float> resultm2 = (Quantity<float>) (width * height);

            IValueConverter converter = siSystem.CreateConverter(resultm2.Unit, StandardUnitsCatalog.SquareCentimeter);
            Assert.NotNull(converter);

            Quantity<float> resultcm2 = new Quantity<float>(converter.Convert(resultm2.Value), converter.Target);

            // Assert           
            Assert.AreEqual(8.0f, resultm2.Value);
            Assert.AreEqual(StandardUnitsCatalog.SquareMeter, resultm2.Unit);
            Assert.AreEqual(StandardDimensionsCatalog.Area, resultm2.Dimension);
            Assert.AreEqual(StandardDimensionsCatalog.Area, resultcm2.Dimension);
            Assert.AreEqual(resultm2.Value*100.0f*100.0f, resultcm2.Value);
        }
    }
}
