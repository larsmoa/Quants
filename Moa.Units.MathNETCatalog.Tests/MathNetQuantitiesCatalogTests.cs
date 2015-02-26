using MathNet.Numerics.LinearAlgebra.Double;
using Moa.Units;
using Moa.Units.MathNETCatalog;
using Moa.Units.Quantities;
using Moq;
using NUnit.Framework;

namespace Tests.Moa.Units.MathNETCatalog
{
    [TestFixture]
    public class MathNetQuantitiesCatalogTests
    {
        [Test]
        public void Multiply_MatrixMatrix_ReturnsMatrix()
        {
            // Arrange
            MathNetQuantitiesCatalog.Setup();
            IUnit unit = new Mock<IUnit>().Object;
            Matrix leftMatrix = new DenseMatrix(10, 15);
            Matrix rightMatrix = new DenseMatrix(15, 5);
            Quantity<Matrix> left = new Quantity<Matrix>(leftMatrix, unit);
            Quantity<Matrix> right = new Quantity<Matrix>(rightMatrix, unit);

            // Act
            Quantity<Matrix> result = left*right as Quantity<Matrix>;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.AreEqual(10, result.Value.RowCount);
            Assert.AreEqual(5, result.Value.ColumnCount);
        }

        [Test]
        public void Multiply_MatrixVector_ReturnsVector()
        {
            // Arrange
            MathNetQuantitiesCatalog.Setup();
            IUnit unit = new Mock<IUnit>().Object;
            Matrix leftMatrix = new DenseMatrix(10, 15);
            Vector rightVector = new DenseVector(15);
            Quantity<Matrix> left = new Quantity<Matrix>(leftMatrix, unit);
            Quantity<Vector> right = new Quantity<Vector>(rightVector, unit);

            // Act
            Quantity<Vector> result = left * right as Quantity<Vector>;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.AreEqual(10, result.Value.Count);
        }

        [Test]
        public void Multiply_VectorMatrix_ReturnsVector()
        {
            // Arrange
            MathNetQuantitiesCatalog.Setup();
            IUnit unit = new Mock<IUnit>().Object;
            Vector leftVector = new DenseVector(10);
            Matrix rightMatrix = new DenseMatrix(10, 15);
            Quantity<Vector> left = new Quantity<Vector>(leftVector, unit);
            Quantity<Matrix> right = new Quantity<Matrix>(rightMatrix, unit);

            // Act
            Quantity<Vector> result = left * right as Quantity<Vector>;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.AreEqual(15, result.Value.Count);
        }

        [Test]
        public void Multiply_FloatMatrix_ReturnsMatrix()
        {
            // Arrange
            MathNetQuantitiesCatalog.Setup();
            IUnit unit = new Mock<IUnit>().Object;
            Matrix rightMatrix = new DenseMatrix(10, 15);
            Quantity<float> left = new Quantity<float>(2.0f, unit);
            Quantity<Matrix> right = new Quantity<Matrix>(rightMatrix, unit);

            // Act
            Quantity<Matrix> result = left * right as Quantity<Matrix>;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.AreEqual(rightMatrix.RowCount, result.Value.RowCount);
            Assert.AreEqual(rightMatrix.ColumnCount, result.Value.ColumnCount);
        }
    }
}
