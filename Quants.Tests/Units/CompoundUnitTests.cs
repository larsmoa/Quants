using Moq;
using NUnit.Framework;
using Quants;
using Quants.Units;

namespace Tests.Quants.Units
{
    [TestFixture]
    public class CompoundUnitTests
    {
        [Test]
        public void Inverse_A_Returns1OverA()
        {
            // Arrange
            IUnit a = CreateStubUnit("a");
            CompoundUnit unit = new CompoundUnit(a);
            // Act
            IUnit result = unit.Inverse();
            // Assert
            Assert.AreEqual(new CompoundUnit(Unitless.Instance, a), result);
        }

        [Test]
        public void Inverse_1overA_ReturnsA()
        {
            // Arrange
            IUnit a = CreateStubUnit("a");
            CompoundUnit unit = new CompoundUnit(Unitless.Instance, a);
            // Act
            IUnit result = unit.Inverse();
            // Assert
            Assert.AreSame(a, result);
        }

        [Test]
        public void Inverse_AoverB_ReturnsBoverA()
        {
            // Arrange
            IUnit a = CreateStubUnit("a");
            IUnit b = CreateStubUnit("b");
            CompoundUnit unit = new CompoundUnit(a, b);
            // Act
            IUnit result = unit.Inverse();
            // Assert
            Assert.AreEqual(new CompoundUnit(b, a), result);
        }


        [Test]
        public void Multiply_AoverBx1_ReturnsSelf()
        {
            // Arrange
            IUnit a = CreateStubUnit("a");
            IUnit b = CreateStubUnit("b");
            CompoundUnit unit = new CompoundUnit(a, b);

            // Act
            IUnit result = unit.Multiply(Unitless.Instance);

            // Assert
            Assert.AreSame(unit, result);
        }

        [Test]
        public void Multiply_AxB_ReturnsCompoundUnit()
        {
            // Arrange
            IUnit a = CreateStubUnit("a");
            IUnit b = CreateStubUnit("b");
            CompoundUnit unit = new CompoundUnit(a);

            // Act
            IUnit result = unit.Multiply(b);

            // Assert
            Assert.IsInstanceOf(typeof(CompoundUnit), result);
        }

        [Test]
        public void Divide_Aover1_ReturnsA()
        {
            // Arrange
            IUnit a = CreateStubUnit("a");
            CompoundUnit unit = new CompoundUnit(a);

            // Act
            IUnit result = unit.Divide(Unitless.Instance);

            // Assert
            Assert.AreSame(a, result);
        }

        [Test]
        public void Divide_AoverB_ReturnsCompoundUnit()
        {
            // Arrange
            IUnit a = CreateStubUnit("a");
            IUnit b = CreateStubUnit("b");
            CompoundUnit unit = new CompoundUnit(a);

            // Act
            IUnit result = unit.Divide(b);

            // Assert
            Assert.IsInstanceOf(typeof(CompoundUnit), result);
        }

        [Test]
        public void Divide_AxBoverB_ReturnsA()
        {
            // Arrange
            IUnit a = CreateStubUnit("a");
            IUnit b = CreateStubUnit("b");
            CompoundUnit unit = new CompoundUnit(a);
            unit = (CompoundUnit) unit.Multiply(b);

            // Act
            IUnit result = unit.Divide(b);

            // Assert
            Assert.AreSame(a, result);
        }

        [Test]
        public void Multiply_AoverBxB_ReturnsA()
        {
            // Arrange
            IUnit a = CreateStubUnit("a");
            IUnit b = CreateStubUnit("b");
            CompoundUnit unit = new CompoundUnit(a);
            unit = (CompoundUnit)unit.Divide(b);

            // Act
            IUnit result = unit.Multiply(b);

            // Assert
            Assert.AreSame(a, result);
        }

        [Test]
        public void Multiply_AxBsquared_EqualsAxAxBxB()
        {
            // Arrange
            IUnit a = CreateStubUnit("a");
            IUnit b = CreateStubUnit("b");
            CompoundUnit unit = new CompoundUnit(a);
            CompoundUnit axb = (CompoundUnit) unit.Multiply(b);

            // Act
            IUnit axbsquared = axb.Multiply(axb);
            IUnit axaxbxb = unit.Multiply(a, b, b);

            // Assert
            Assert.AreEqual(axaxbxb, axbsquared);

        }

        private static IUnit CreateStubUnit(string unit = "")
        {
            var stub = new Mock<IUnit>();
            stub.Setup(x => x.Unit).Returns(unit);
            stub.Setup(x => x.GetHashCode()).Returns(unit.GetHashCode());
            stub.Setup(x => x.Equals(stub.Object)).Returns(true);
            return stub.Object;
        }
    }
}
