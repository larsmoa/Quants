using System;
using Moa.Units.Quantities;
using Moq;
using NUnit.Framework;

namespace Tests.Moa.Units.Quantities
{
    [TestFixture]
    public class QuantityBaseTests
    {
        [Test]
        public void ValueCastInt_ValueIsInt_DoesNotThrow()
        {
            // Arrange
            var mock = new Mock<QuantityBase>();
            mock.Setup(x => x.QuantityValueType).Returns(typeof (int));
            mock.Setup(x => x.QuantityValue).Returns(1);
            QuantityBase quantity = mock.Object;

            // Act/assert
            Assert.DoesNotThrow(() => quantity.ValueCast<int>());
        }

        [Test]
        public void ValueCastDouble_ValueIsInt_DoesNotThrow()
        {
            // Arrange
            var mock = new Mock<QuantityBase>();
            mock.Setup(x => x.QuantityValueType).Returns(typeof(int));
            mock.Setup(x => x.QuantityValue).Returns(1);
            QuantityBase quantity = mock.Object;

            // Act/assert
            Assert.DoesNotThrow(() => quantity.ValueCast<double>());
        }

        [Test]
        public void ValueCastNotRelated_ValueIsInt_ThrowsInvalidCast()
        {
            // Arrange
            var mock = new Mock<QuantityBase>();
            mock.Setup(x => x.QuantityValueType).Returns(typeof(int));
            mock.Setup(x => x.QuantityValue).Returns(1);
            QuantityBase quantity = mock.Object;

            
            // Act/assert
            Assert.Throws<InvalidCastException>(() => quantity.ValueCast<Array>());
        }
    }
}
