using System;
using Moq;
using NUnit.Framework;
using Quants;
using Quants.Quantities;

namespace Tests.Quants.Quantities
{
    [TestFixture]
    public class ArithmeticsStoreTests
    {
        [Test]
        public void Add_UnregisteredType_ThrowsInvalidOperationException()
        {
            // Arrange
            IUnit unit = new Mock<IUnit>().Object;
            ArithmeticsStore store = CreateStore();
            // Act/assert
            Assert.Throws<InvalidOperationException>(() => store.Add(new Quantity<float>(0.0f, unit), new Quantity<float>(0.0f, unit)));
        }

        [Test]
        public void Add_RegisteredType_InvokesOperation()
        {
            // Arrange
            var mockDelegate = new Mock<IOperationFunction>();
            IUnit unit = new Mock<IUnit>().Object;            
            ArithmeticsStore store = CreateStore();
            store.RegisterAddOperation<float>(mockDelegate.Object.Operation);
            // Act/assert
            Assert.DoesNotThrow(() => store.Add(new Quantity<float>(0.0f, unit), new Quantity<float>(0.0f, unit)));            
            mockDelegate.Verify(x => x.Operation(It.IsAny<QuantityBase>(), It.IsAny<QuantityBase>()), Times.Once());
        }

        [Test]
        public void Add_RegisteredsTypeButNotMatchingUnit_ThrowsArgumentException()
        {
            // Arrange
            var mockDelegate = new Mock<IOperationFunction>();
            IUnit unit1 = new Mock<IUnit>().Object;
            IUnit unit2 = new Mock<IUnit>().Object;
            ArithmeticsStore store = CreateStore();
            store.RegisterAddOperation<float>(mockDelegate.Object.Operation);
            // Act/assert
            Assert.Throws<ArgumentException>(() => store.Add(new Quantity<float>(0.0f, unit1), new Quantity<float>(0.0f, unit2)));
        }

        [Test]
        public void Subtract_UnregisteredType_ThrowsInvalidOperationException()
        {
            // Arrange
            IUnit unit = new Mock<IUnit>().Object;
            ArithmeticsStore store = CreateStore();
            // Act/assert
            Assert.Throws<InvalidOperationException>(() => store.Subtract(new Quantity<float>(0.0f, unit), new Quantity<float>(0.0f, unit)));
        }

        [Test]
        public void Subtract_RegisteredType_InvokesOperation()
        {
            // Arrange
            var mockDelegate = new Mock<IOperationFunction>();
            IUnit unit = new Mock<IUnit>().Object;
            ArithmeticsStore store = CreateStore();
            store.RegisterSubtractOperation<float>(mockDelegate.Object.Operation);
            // Act/assert
            Assert.DoesNotThrow(() => store.Subtract(new Quantity<float>(0.0f, unit), new Quantity<float>(0.0f, unit)));
        }

        [Test]
        public void Subtract_RegisteredButTypeNotMatchingUnits_ThrowsArgumentException()
        {
            // Arrange
            var mockDelegate = new Mock<IOperationFunction>();
            IUnit unit1 = new Mock<IUnit>().Object;
            IUnit unit2 = new Mock<IUnit>().Object;
            ArithmeticsStore store = CreateStore();
            store.RegisterSubtractOperation<float>(mockDelegate.Object.Operation);
            // Act/assert
            Assert.Throws<ArgumentException>(() => store.Subtract(new Quantity<float>(0.0f, unit1), new Quantity<float>(0.0f, unit2)));
        }

        [Test]
        public void Multiply_UnregisteredType_ThrowsInvalidOperationException()
        {
            // Arrange
            IUnit unit = new Mock<IUnit>().Object;
            ArithmeticsStore store = CreateStore();
            // Act/assert
            Assert.Throws<InvalidOperationException>(() => store.Multiply(new Quantity<float>(0.0f, unit), new Quantity<float>(0.0f, unit)));
        }

        [Test]
        public void Multiply_RegisteredType_InvokesOperation()
        {
            // Arrange
            var mockDelegate = new Mock<IOperationFunction>();
            IUnit unit = new Mock<IUnit>().Object;
            ArithmeticsStore store = CreateStore();
            store.RegisterMultiplyOperation<float, float>(mockDelegate.Object.Operation);
            // Act/assert
            Assert.DoesNotThrow(() => store.Multiply(new Quantity<float>(0.0f, unit), new Quantity<float>(0.0f, unit)));
            mockDelegate.Verify(x => x.Operation(It.IsAny<QuantityBase>(), It.IsAny<QuantityBase>()), Times.Once());
        }

        [Test]
        public void Multiply_RegisteredTypeButNotMatchingDimension_InvokesOperation()
        {
            // Arrange
            var mockDelegate = new Mock<IOperationFunction>();
            var stubUnit1 = new Mock<IUnit>();
            stubUnit1.Setup(x => x.Dimension).Returns(new Mock<IDimension>().Object);
            var stubUnit2 = new Mock<IUnit>();
            stubUnit2.Setup(x => x.Dimension).Returns(new Mock<IDimension>().Object);
            ArithmeticsStore store = CreateStore();
            store.RegisterMultiplyOperation<float, float>(mockDelegate.Object.Operation);
            // Act/assert
            Assert.DoesNotThrow(() => store.Multiply(new Quantity<float>(0.0f, stubUnit1.Object),
                                                     new Quantity<float>(0.0f, stubUnit2.Object)));
            mockDelegate.Verify(x => x.Operation(It.IsAny<QuantityBase>(), It.IsAny<QuantityBase>()), Times.Once());
        }

        [Test]
        public void Divide_UnregisteredType_ThrowsInvalidOperationException()
        {
            // Arrange
            IUnit unit = new Mock<IUnit>().Object;
            ArithmeticsStore store = CreateStore();
            // Act/assert
            Assert.Throws<InvalidOperationException>(() => store.Divide(new Quantity<float>(1.0f, unit), new Quantity<float>(1.0f, unit)));
        }

        [Test]
        public void Divide_RegisteredType_InvokesOperation()
        {
            // Arrange
            var mockDelegate = new Mock<IOperationFunction>();
            IUnit unit = new Mock<IUnit>().Object;
            ArithmeticsStore store = CreateStore();
            store.RegisterDivideOperation<float, float>(mockDelegate.Object.Operation);
            // Act/assert
            Assert.DoesNotThrow(() => store.Divide(new Quantity<float>(1.0f, unit), new Quantity<float>(1.0f, unit)));
            mockDelegate.Verify(x => x.Operation(It.IsAny<QuantityBase>(), It.IsAny<QuantityBase>()), Times.Once());
        }

        [Test]
        public void Divide_RegisteredTypeButNotMatchingDimension_InvokesOperation()
        {
            // Arrange
            var mockDelegate = new Mock<IOperationFunction>();
            var stubUnit1 = new Mock<IUnit>();
            stubUnit1.Setup(x => x.Dimension).Returns(new Mock<IDimension>().Object);
            var stubUnit2 = new Mock<IUnit>();
            stubUnit2.Setup(x => x.Dimension).Returns(new Mock<IDimension>().Object);
            ArithmeticsStore store = CreateStore();
            store.RegisterDivideOperation<float, float>(mockDelegate.Object.Operation);
            // Act/assert
            Assert.DoesNotThrow(() => store.Divide(new Quantity<float>(1.0f, stubUnit1.Object),
                                                   new Quantity<float>(1.0f, stubUnit2.Object)));
            mockDelegate.Verify(x => x.Operation(It.IsAny<QuantityBase>(), It.IsAny<QuantityBase>()), Times.Once());
        }

        private static ArithmeticsStore CreateStore()
        {
            // Inaccessible constructor
            return new Mock<ArithmeticsStore>().Object;
        }
    }
}
