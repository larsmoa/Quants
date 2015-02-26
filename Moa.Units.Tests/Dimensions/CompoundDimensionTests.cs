using Moa.Units;
using Moa.Units.Dimensions;
using Moq;
using NUnit.Framework;

namespace Tests.Moa.Units.Dimensions
{
    [TestFixture]
    public class CompoundDimensionTests
    {
        [Test]
        public void Inverse_A_Returns1OverA()
        {
            // Arrange
            IDimension a = CreateStubDimension("a");
            CompoundDimension dimension = new CompoundDimension(a);
            // Act
            IDimension result = dimension.Inverse();
            // Assert
            Assert.AreEqual(new CompoundDimension(Dimsensionless.Instance, a), result);
        }

        [Test]
        public void Inverse_1overA_ReturnsA()
        {
            // Arrange
            IDimension a = CreateStubDimension("a");
            CompoundDimension dimension = new CompoundDimension(Dimsensionless.Instance, a);
            // Act
            IDimension result = dimension.Inverse();
            // Assert
            Assert.AreSame(a, result);
        }

        [Test]
        public void Inverse_AoverB_ReturnsBoverA()
        {
            // Arrange
            IDimension a = CreateStubDimension("a");
            IDimension b = CreateStubDimension("b");
            CompoundDimension dimension = new CompoundDimension(a, b);
            // Act
            IDimension result = dimension.Inverse();
            // Assert
            Assert.AreEqual(new CompoundDimension(b, a), result);
        }


        [Test]
        public void Multiply_AoverBx1_ReturnsSelf()
        {
            // Arrange
            IDimension a = CreateStubDimension("a");
            IDimension b = CreateStubDimension("b");
            CompoundDimension dimension = new CompoundDimension(a, b);

            // Act
            IDimension result = dimension.Multiply(Dimsensionless.Instance);

            // Assert
            Assert.AreSame(dimension, result);
        }

        [Test]
        public void Multiply_AxB_ReturnsCompoundUnit()
        {
            // Arrange
            IDimension a = CreateStubDimension("a");
            IDimension b = CreateStubDimension("b");
            CompoundDimension unit = new CompoundDimension(a);

            // Act
            IDimension result = unit.Multiply(b);

            // Assert
            Assert.IsInstanceOf(typeof(CompoundDimension), result);
        }

        [Test]
        public void Divide_Aover1_ReturnsA()
        {
            // Arrange
            IDimension a = CreateStubDimension("a");
            CompoundDimension dimension = new CompoundDimension(a);

            // Act
            IDimension result = dimension.Divide(Dimsensionless.Instance);

            // Assert
            Assert.AreSame(a, result);
        }

        [Test]
        public void Divide_AoverB_ReturnsCompoundUnit()
        {
            // Arrange
            IDimension a = CreateStubDimension("a");
            IDimension b = CreateStubDimension("b");
            CompoundDimension dimension = new CompoundDimension(a);

            // Act
            IDimension result = dimension.Divide(b);

            // Assert
            Assert.IsInstanceOf(typeof(CompoundDimension), result);
        }

        [Test]
        public void Divide_AxBoverB_ReturnsA()
        {
            // Arrange
            IDimension a = CreateStubDimension("a");
            IDimension b = CreateStubDimension("b");
            CompoundDimension unit = new CompoundDimension(a);
            unit = (CompoundDimension) unit.Multiply(b);

            // Act
            IDimension result = unit.Divide(b);

            // Assert
            Assert.AreSame(a, result);
        }

        [Test]
        public void Multiply_AoverBxB_ReturnsA()
        {
            // Arrange
            IDimension a = CreateStubDimension("a");
            IDimension b = CreateStubDimension("b");
            CompoundDimension unit = new CompoundDimension(a);
            unit = (CompoundDimension) unit.Divide(b);

            // Act
            IDimension result = unit.Multiply(b);

            // Assert
            Assert.AreSame(a, result);
        }

        [Test]
        public void Multiply_AxBsquared_EqualsAxAxBxB()
        {
            // Arrange
            IDimension a = CreateStubDimension("a");
            IDimension b = CreateStubDimension("b");
            CompoundDimension unit = new CompoundDimension(a);
            CompoundDimension axb = (CompoundDimension) unit.Multiply(b);

            // Act
            IDimension axbsquared = axb.Multiply(axb);
            IDimension axaxbxb = unit.Multiply(a, b, b);

            // Assert
            Assert.AreEqual(axaxbxb, axbsquared);

        }

        private static IDimension CreateStubDimension(string name = "")
        {
            var stub = new Mock<IDimension>();
            stub.Setup(x => x.Symbol).Returns(name);
            stub.Setup(x => x.GetHashCode()).Returns(name.GetHashCode());
            stub.Setup(x => x.Equals(stub.Object)).Returns(true);
            return stub.Object;
        }
    }
}
