using System;
using Moq;
using NUnit.Framework;
using Quants;
using Quants.Catalogs;
using Quants.Quantities;

namespace Tests.Quants.Catalogs
{
    [TestFixture, Category(Categories.Integrationtest)]
    public class StandardQuantitiesCatalogTests
    {
        [Test]
        public void Setup_Multiply_AllAssosciativeCombinationsAreWorking()
        {
            // Arrange
            StandardQuantitiesCatalog.Setup();

            // Act/Assert (left input, right input, expected output)
            Assert.DoesNotThrow(DoAssosciativeQuantityMultiply<float, float, float>);
            Assert.DoesNotThrow(DoAssosciativeQuantityMultiply<double, double, double>);
            Assert.DoesNotThrow(DoAssosciativeQuantityMultiply<int, int, int>);
            Assert.DoesNotThrow(DoAssosciativeQuantityMultiply<long, long, long>);

            Assert.DoesNotThrow(DoAssosciativeQuantityMultiply<double, int, double>);
            Assert.DoesNotThrow(DoAssosciativeQuantityMultiply<double, long, double>);
            Assert.DoesNotThrow(DoAssosciativeQuantityMultiply<double, float, double>);

            Assert.DoesNotThrow(DoAssosciativeQuantityMultiply<float, int, float>);
            Assert.DoesNotThrow(DoAssosciativeQuantityMultiply<float, long, float>);

            Assert.DoesNotThrow(DoAssosciativeQuantityMultiply<int, long, long>);
        }

        [Test]
        public void Setup_Divide_AllAssosciativeCombinationsAreWorking()
        {
            // Arrange
            StandardQuantitiesCatalog.Setup();

            // Act/Assert (left input, right input, expected output)
            Assert.DoesNotThrow(() => DoAssosciativeQuantityDivide<float, float, float>(1, 1));
            Assert.DoesNotThrow(() => DoAssosciativeQuantityDivide<double, double, double>(1, 1));
            Assert.DoesNotThrow(() => DoAssosciativeQuantityDivide<int, int, int>(1, 1));
            Assert.DoesNotThrow(() => DoAssosciativeQuantityDivide<long, long, long>(1, 1));

            Assert.DoesNotThrow(() => DoAssosciativeQuantityDivide<double, int, double>(1, 1));
            Assert.DoesNotThrow(() => DoAssosciativeQuantityDivide<double, long, double>(1, 1));
            Assert.DoesNotThrow(() => DoAssosciativeQuantityDivide<double, float, double>(1, 1));

            Assert.DoesNotThrow(() => DoAssosciativeQuantityDivide<float, int, float>(1, 1));
            Assert.DoesNotThrow(() => DoAssosciativeQuantityDivide<float, long, float>(1, 1));

            Assert.DoesNotThrow(() => DoAssosciativeQuantityDivide<int, long, long>(1, 1));
        }

        [Test]
        public void Setup_Add_SameValueTypeIsWorking()
        {
            // Arrange
            StandardQuantitiesCatalog.Setup();

            // Act/Assert (left input, right input, expected output)
            Assert.DoesNotThrow(DoQuantityAdd<float, float, float>);
            Assert.DoesNotThrow(DoQuantityAdd<double, double, double>);
            Assert.DoesNotThrow(DoQuantityAdd<int, int, int>);
            Assert.DoesNotThrow(DoQuantityAdd<long, long, long>);
        }

        [Test]
        public void Setup_Add_CombinationsThrowsInvalidOperationException()
        {
            // Arrange
            StandardQuantitiesCatalog.Setup();

            // Act/Assert (left input, right input, expected output)
            Assert.Throws<InvalidOperationException>(DoQuantityAdd<float, double, double>);
            Assert.Throws<InvalidOperationException>(DoQuantityAdd<double, float, double>);
            Assert.Throws<InvalidOperationException>(DoQuantityAdd<int, long, long>);
            Assert.Throws<InvalidOperationException>(DoQuantityAdd<long, int, int>);
            Assert.Throws<InvalidOperationException>(DoQuantityAdd<float, int, float>);
            Assert.Throws<InvalidOperationException>(DoQuantityAdd<int, float, float>);
            Assert.Throws<InvalidOperationException>(DoQuantityAdd<float, long, float>);
            Assert.Throws<InvalidOperationException>(DoQuantityAdd<long, float, float>);
            Assert.Throws<InvalidOperationException>(DoQuantityAdd<double, int, double>);
            Assert.Throws<InvalidOperationException>(DoQuantityAdd<int, double, double>);
            Assert.Throws<InvalidOperationException>(DoQuantityAdd<double, long, double>);
            Assert.Throws<InvalidOperationException>(DoQuantityAdd<long, double, double>);
        }

        [Test]
        public void Setup_Subtract_SameValueTypeIsWorking()
        {
            // Arrange
            StandardQuantitiesCatalog.Setup();

            // Act/Assert (left input, right input, expected output)
            Assert.DoesNotThrow(DoQuantitySubtract<float, float, float>);
            Assert.DoesNotThrow(DoQuantitySubtract<double, double, double>);
            Assert.DoesNotThrow(DoQuantitySubtract<int, int, int>);
            Assert.DoesNotThrow(DoQuantitySubtract<long, long, long>);
        }

        [Test]
        public void Setup_Subtract_CombinationsThrowsInvalidOperationException()
        {
            // Arrange
            StandardQuantitiesCatalog.Setup();

            // Act/Assert (left input, right input, expected output)
            Assert.Throws<InvalidOperationException>(DoQuantitySubtract<float, double, double>);
            Assert.Throws<InvalidOperationException>(DoQuantitySubtract<double, float, double>);
            Assert.Throws<InvalidOperationException>(DoQuantitySubtract<int, long, long>);
            Assert.Throws<InvalidOperationException>(DoQuantitySubtract<long, int, int>);
            Assert.Throws<InvalidOperationException>(DoQuantitySubtract<float, int, float>);
            Assert.Throws<InvalidOperationException>(DoQuantitySubtract<int, float, float>);
            Assert.Throws<InvalidOperationException>(DoQuantitySubtract<float, long, float>);
            Assert.Throws<InvalidOperationException>(DoQuantitySubtract<long, float, float>);
            Assert.Throws<InvalidOperationException>(DoQuantitySubtract<double, int, double>);
            Assert.Throws<InvalidOperationException>(DoQuantitySubtract<int, double, double>);
            Assert.Throws<InvalidOperationException>(DoQuantitySubtract<double, long, double>);
            Assert.Throws<InvalidOperationException>(DoQuantitySubtract<long, double, double>);
        }

        private static void DoAssosciativeQuantityMultiply<TLeft, TRight, TExpectedResult>()
        {
            IUnit unit = new Mock<IUnit>().Object;
            Quantity<TLeft> left = new Quantity<TLeft>(default(TLeft), unit);
            Quantity<TRight> right = new Quantity<TRight>(default(TRight), unit);
            QuantityBase lxrResult = left*right;
            QuantityBase rxlResult = right*left;
            if (lxrResult == null)
                throw new NullReferenceException("Result of left*right was null.");
            if (rxlResult == null)
                throw new NullReferenceException("Result of right*left was null.");

            if (!(rxlResult is Quantity<TExpectedResult>))
                throw new NotSupportedException(string.Format("Expected result type {0}, but got {1}.",
                                                              typeof(Quantity<TExpectedResult>), lxrResult.GetType()));
            if (!(rxlResult is Quantity<TExpectedResult>))
                throw new NotSupportedException(string.Format("Expected result type {0}, but got {1}.",
                                                              typeof(Quantity<TExpectedResult>), rxlResult.GetType()));
        }

        private static void DoAssosciativeQuantityDivide<TDividend, TDivisor, TExpectedResult>(TDividend nonZeroDividend, TDivisor nonZeroDivisor)
        {
            IUnit unit = new Mock<IUnit>().Object;
            Quantity<TDividend> dividend = new Quantity<TDividend>(nonZeroDividend, unit);
            Quantity<TDivisor> divisor = new Quantity<TDivisor>(nonZeroDivisor, unit);
            QuantityBase ldrResult = dividend / divisor;
            QuantityBase rdlResult = divisor / dividend;
            if (ldrResult == null)
                throw new NullReferenceException("Result of left / right was null.");
            if (rdlResult == null)
                throw new NullReferenceException("Result of right / left was null.");

            if (!(ldrResult is Quantity<TExpectedResult>))
                throw new NotSupportedException(string.Format("Expected result type {0}, but got {1}.",
                                                              typeof(Quantity<TExpectedResult>), ldrResult.GetType()));
            if (!(rdlResult is Quantity<TExpectedResult>))
                throw new NotSupportedException(string.Format("Expected result type {0}, but got {1}.",
                                                              typeof(Quantity<TExpectedResult>), rdlResult.GetType()));
        }

        private static void DoQuantityAdd<TDividend, TDivisor, TExpectedResult>()
        {
            IUnit unit = new Mock<IUnit>().Object;
            Quantity<TDividend> dividend = new Quantity<TDividend>(default(TDividend), unit);
            Quantity<TDivisor> divisor = new Quantity<TDivisor>(default(TDivisor), unit);
            QuantityBase ldrResult = dividend + divisor;
            QuantityBase rdlResult = divisor + dividend;
            if (ldrResult == null)
                throw new NullReferenceException("Result of left + right was null.");
            if (rdlResult == null)
                throw new NullReferenceException("Result of right + left was null.");

            if (!(ldrResult is Quantity<TExpectedResult>))
                throw new NotSupportedException(string.Format("Expected result type {0}, but got {1}.",
                                                              typeof(Quantity<TExpectedResult>), ldrResult.GetType()));
            if (!(rdlResult is Quantity<TExpectedResult>))
                throw new NotSupportedException(string.Format("Expected result type {0}, but got {1}.",
                                                              typeof(Quantity<TExpectedResult>), rdlResult.GetType()));
        }

        private static void DoQuantitySubtract<TDividend, TDivisor, TExpectedResult>()
        {
            IUnit unit = new Mock<IUnit>().Object;
            Quantity<TDividend> dividend = new Quantity<TDividend>(default(TDividend), unit);
            Quantity<TDivisor> divisor = new Quantity<TDivisor>(default(TDivisor), unit);
            QuantityBase ldrResult = dividend - divisor;
            QuantityBase rdlResult = divisor - dividend;
            if (ldrResult == null)
                throw new NullReferenceException("Result of left - right was null.");
            if (rdlResult == null)
                throw new NullReferenceException("Result of right - left was null.");

            if (!(ldrResult is Quantity<TExpectedResult>))
                throw new NotSupportedException(string.Format("Expected result type {0}, but got {1}.",
                                                              typeof(Quantity<TExpectedResult>), ldrResult.GetType()));
            if (!(rdlResult is Quantity<TExpectedResult>))
                throw new NotSupportedException(string.Format("Expected result type {0}, but got {1}.",
                                                              typeof(Quantity<TExpectedResult>), rdlResult.GetType()));
        }
    }
}
