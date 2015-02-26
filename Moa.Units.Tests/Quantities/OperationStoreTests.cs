using System;
using Moq;
using NUnit.Framework;
using Quants;
using Quants.Quantities;

namespace Tests.Quants.Quantities
{
    [TestFixture]
    public class OperationStoreTests
    {
        [Test]
        public void AddOperationTemplated_ValidTypes_DoesNotThrow()
        {
            // Arrange
            OperationStore store = new OperationStore();
            Func<QuantityBase, QuantityBase, QuantityBase> operation = CreateMockOperation().Object.Operation;
            // Act/assert
            Assert.DoesNotThrow(() => store.AddOperation<Quantity<float>, Quantity<double>>(operation));
        }

        [Test]
        public void AddOperationTemplated_ExistingType_ThrowsArgumentException()
        {
            // Arrange
            OperationStore store = new OperationStore();
            Func<QuantityBase, QuantityBase, QuantityBase> operation = CreateMockOperation().Object.Operation;
            store.AddOperation<Quantity<float>, Quantity<double>>(operation);
            // Act/assert
            Assert.Throws<ArgumentException>(() => store.AddOperation<Quantity<float>, Quantity<double>>(operation));
        }

        [Test]
        public void AddOperation_ValidTypes_DoesNotThrow()
        {
            // Arrange
            OperationStore store = new OperationStore();
            Func<QuantityBase, QuantityBase, QuantityBase> operation = CreateMockOperation().Object.Operation;            
            // Act/assert
            Assert.DoesNotThrow(() => store.AddOperation(typeof(Quantity<float>), typeof(Quantity<float>), operation));
        }

        [Test]
        public void AddOperation_TypeDoesNotInheritdQuantityBase_ThrowsInvalidCastException()
        {
            // Arrange
            OperationStore store = new OperationStore();
            Func<QuantityBase, QuantityBase, QuantityBase> operation = CreateMockOperation().Object.Operation;
            // Act/assert
            Assert.Throws<InvalidCastException>(() => store.AddOperation(typeof(string), typeof(int), operation));
        }

        [Test]
        public void PerformOperation_RegisteredTypes_InvokesOperation()
        {
            // Arrange
            OperationStore store = new OperationStore();
            var mockOperation = CreateMockOperation();
            store.AddOperation<int, int>(mockOperation.Object.Operation);
            // Act
            store.PerformOperation(CreateStubQuantity<int>(), CreateStubQuantity<int>());
            // Assert
            mockOperation.Verify(x => x.Operation(It.IsAny<QuantityBase>(), It.IsAny<QuantityBase>()), Times.Once());
        }

        [Test]
        public void PerformOperation_UnregisteredTypes_ThrowsInvalidOperationException()
        {
            // Arrange
            OperationStore store = new OperationStore();
            var mockOperation = CreateMockOperation();
            store.AddOperation<int, int>(mockOperation.Object.Operation);
            // Act/Assert
            Assert.Throws<InvalidOperationException>(() => store.PerformOperation(CreateStubQuantity<int>(), 
                                                                                  CreateStubQuantity<float>()));
        }

        [Test]
        public void PerformOperation_CommutativeTypes_ThrowsInvalidOperationException()
        {
            // Arrange
            OperationStore store = new OperationStore();
            var mockOperation = CreateMockOperation();
            store.AddOperation<int, float>(mockOperation.Object.Operation);
            // Act/Assert
            Assert.Throws<InvalidOperationException>(() => store.PerformOperation(CreateStubQuantity<float>(),
                                                                                  CreateStubQuantity<int>()));
        }

        private static Mock<IOperationFunction> CreateMockOperation()
        {
            Func<QuantityBase, QuantityBase, QuantityBase> factory =
                (source, target) =>
                {
                    var mockConverter = new Mock<QuantityBase>();
                    return mockConverter.Object;
                };
            var mockDelegate = new Mock<IOperationFunction>();
            mockDelegate.Setup(x => x.Operation(It.IsAny<QuantityBase>(), It.IsAny<QuantityBase>())).Returns(factory);
            return mockDelegate;
        }

        private static Quantity<T> CreateStubQuantity<T>()
        {
            IUnit unit = new Mock<IUnit>().Object;
            return new Quantity<T>(default(T), unit);
        }
    }


}
