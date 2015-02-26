using Moa.Units;
using Moa.Units.Dimensions;
using Moa.Units.Units;
using Moq;
using NUnit.Framework;

namespace Tests.Moa.Units
{
    [TestFixture]
    public class UnitCombinerTests
    {
        [Test]
        public void Multiply_UnitlessArguments_ResultIsUnitless()
        {
            IUnit result = UnitCombiner.Multiply(Unitless.Instance, Unitless.Instance);
            Assert.IsInstanceOf(typeof (Unitless), result);
        }

        [Test]
        public void Multiply_OneArgumentIsUnitless_ResultIsOtherArgument()
        {
            // Arrange
            IUnit unit = CreateStubUnit();
            // Act
            IUnit leftresult = UnitCombiner.Multiply(Unitless.Instance, unit);
            IUnit rightresult = UnitCombiner.Multiply(Unitless.Instance, unit);
            // Assert
            Assert.AreSame(unit, leftresult);
            Assert.AreSame(unit, rightresult);
        }

        [Test]
        public void Multiply_NoUnitlessArguments_ReturnsCompoundUnit()
        {
            // Arrange
            IUnit unit = CreateStubUnit();
            // Act
            IUnit result = UnitCombiner.Multiply(unit, unit);
            // Assert
            Assert.IsInstanceOf(typeof (CompoundUnit), result);
        }

        [Test]
        public void Multiply_AxB_EqualsBxA()
        {
            // Arrange
            IUnit a = CreateStubUnit("a");
            IUnit b = CreateStubUnit("b");

            // Act
            IUnit result = UnitCombiner.Multiply(a, b);

            // Assert
            Assert.AreEqual(UnitCombiner.Multiply(b, a), result);

        }

        [Test]
        public void Divide_UnitlessArguments_ReturnsUnitless()
        {
            IUnit result = UnitCombiner.Divide(Unitless.Instance, Unitless.Instance);
            Assert.IsInstanceOf(typeof (Unitless), result);
        }

        [Test]
        public void Divide_LeftArgumentIsUnitless_ReturnsCompoundUnit()
        {
            // Arrange
            IUnit unit = CreateStubUnit();
            // Act
            IUnit result = UnitCombiner.Divide(Unitless.Instance, unit);
            // Assert
            Assert.IsInstanceOf(typeof (CompoundUnit), result);
        }

        [Test]
        public void Divide_RightArgumentIsUnitless_ReturnsLeftUnit()
        {
            // Arrange
            IUnit unit = CreateStubUnit();
            // Act
            IUnit result = UnitCombiner.Divide(unit, Unitless.Instance);
            // Assert
            Assert.AreSame(unit, result);
        }

        [Test]
        public void Divide_BothArgumentsAreSame_ReturnsUnitless()
        {
            // Arrange
            IUnit unit = CreateStubUnit();
            // Act
            IUnit result = UnitCombiner.Divide(unit, unit);
            // Assert
            Assert.IsInstanceOf(typeof (Unitless), result);
        }

        [Test]
        public void Divide_AxB_over_B_ResultIsA()
        {
            // Arrange
            IUnit a = CreateStubUnit("a");
            IUnit b = CreateStubUnit("b");
            IUnit axb = UnitCombiner.Multiply(a, b);

            // Act            
            IUnit result = UnitCombiner.Divide(axb, b);

            // Assert
            Assert.AreSame(a, result);
        }

        [Test]
        public void Divide_AxB_over_A_ResultIsB()
        {
            // Arrange
            IUnit a = CreateStubUnit("a");
            IUnit b = CreateStubUnit("b");
            IUnit axb = UnitCombiner.Multiply(a, b);

            // Act            
            IUnit result = UnitCombiner.Divide(axb, a);

            // Assert
            Assert.AreSame(b, result);
        }


        [Test]
        public void Divide_AxBxC_over_B_ResultIsAxC()
        {
            // Arrange
            IUnit a = CreateStubUnit("a");
            IUnit b = CreateStubUnit("b");
            IUnit c = CreateStubUnit("c");
            IUnit axb = UnitCombiner.Multiply(a, b);
            IUnit axbxc = UnitCombiner.Multiply(axb, c);
            IUnit expected = UnitCombiner.Multiply(a, c);

            // Act            
            IUnit result = UnitCombiner.Divide(axbxc, b);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Multiply_A_over_B_MultiplyWithB_ResultIsA()
        {
            // Arrange
            IUnit a = CreateStubUnit("a");
            IUnit b = CreateStubUnit("b");
            IUnit aoverb = UnitCombiner.Divide(a, b);

            // Act
            IUnit result = UnitCombiner.Multiply(aoverb, b);

            // Assert
            Assert.AreEqual(a, result);
        }

        [Test]
        public void Multiply_1_over_B_MultiplyWithA_ResultIsA_over_B()
        {
            // Arrange
            IUnit a = CreateStubUnit("a");
            IUnit b = CreateStubUnit("b");
            IUnit binv = UnitCombiner.Divide(Unitless.Instance, b);

            // Act
            IUnit result = UnitCombiner.Multiply(binv, a);

            // Assert
            Assert.AreEqual(UnitCombiner.Divide(a, b), result);
        }

        [Test]
        public void Divide_CompoundUnitOverSelf_ReturnsUnitless()
        {
            // Arrange
            IUnit a = CreateStubUnit("a");
            IUnit b = CreateStubUnit("b");
            IUnit axb = UnitCombiner.Multiply(a, b);

            // Act
            IUnit result = UnitCombiner.Divide(axb, axb);

            // Assert
            Assert.AreSame(Unitless.Instance, result);
        }

        [Test]
        public void Divide_CompoundUnitOverEqualCompoundUnit_ReturnsUnitless()
        {
            // Arrange
            IUnit a = CreateStubUnit("a");
            IUnit b = CreateStubUnit("b");
            IUnit axb1 = UnitCombiner.Multiply(a, b);
            IUnit axb2 = UnitCombiner.Multiply(a, b);

            // Act
            IUnit result = UnitCombiner.Divide(axb1, axb2);

            // Assert
            Assert.AreSame(Unitless.Instance, result);
        }

        [Test]
        public void Divide_AxBxC_Over_AxB_ReturnsC()
        {
            // Arrange
            IUnit a = CreateStubUnit("a");
            IUnit b = CreateStubUnit("b");
            IUnit c = CreateStubUnit("c");
            IUnit axb = UnitCombiner.Multiply(a, b);
            IUnit axbxc = UnitCombiner.Multiply(axb, c);

            // Act
            IUnit result = UnitCombiner.Divide(axbxc, axb);

            // Assert
            Assert.AreSame(c, result);
        }

        [Test]
        public void Multiply_AoverB_X_BoverA_ReturnsUnitless()
        {
            // Arrange
            IUnit a = CreateStubUnit("a");
            IUnit b = CreateStubUnit("b");
            IUnit aoverb = UnitCombiner.Divide(a, b);
            IUnit bovera = UnitCombiner.Divide(b, a);

            // Act
            IUnit result = UnitCombiner.Multiply(aoverb, bovera);

            // Assert
            Assert.AreSame(Unitless.Instance, result);
        }

        [Test]
        public void Multiply_AxB_X_BxA_EqualsAxAxBxB()
        {
            // Arrange
            IUnit a = CreateStubUnit("a");
            IUnit b = CreateStubUnit("b");
            IUnit axb = UnitCombiner.Multiply(a, b);
            IUnit bxa = UnitCombiner.Multiply(b, a);

            // Act
            IUnit result = UnitCombiner.Multiply(axb, bxa);

            // Assert
            Assert.AreEqual(UnitCombiner.Multiply(a, a, b, b), result);
        }

        [Test]
        public void Divide_AxB_Over_1OverBxA_EqualsAxAxBxB()
        {
            // Arrange
            IUnit a = CreateStubUnit("a");
            IUnit b = CreateStubUnit("b");
            IUnit axb = UnitCombiner.Multiply(a, b);
            IUnit bxa = UnitCombiner.Multiply(b, a);
            IUnit bxainv = UnitCombiner.Divide(Unitless.Instance, bxa);

            // Act
            IUnit result = UnitCombiner.Divide(axb, bxainv);

            // Assert
            Assert.AreEqual(UnitCombiner.Multiply(a, a, b, b), result);
        }

        [Test]
        public void MultiplyWithDimension_AmassxBlength_DimensionIsMassLength()
        {
            // Arrange
            IDimension mass = CreateStubDimension("M");
            IDimension length = CreateStubDimension("L");
            IUnit a = CreateStubUnit("a", mass);
            IUnit b = CreateStubUnit("b", length);

            // Act
            IUnit axb = UnitCombiner.Multiply(a, b);
            IDimension dimension = axb.Dimension;

            // Assert
            Assert.AreEqual(new CompoundDimension(mass).Multiply(length), dimension);
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
