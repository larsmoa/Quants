using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Moq;
using NUnit.Framework;
using Quants;
using Quants.MathNETCatalog;
using Quants.Quantities;

namespace Tests.Quantss.MathNETCatalog
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
            Matrix<double> leftMatrix = new DenseMatrix(10, 15);
            Matrix<double> rightMatrix = new DenseMatrix(15, 5);
            Quantity<Matrix<double>> left = new Quantity<Matrix<double>>(leftMatrix, unit);
            Quantity<Matrix<double>> right = new Quantity<Matrix<double>>(rightMatrix, unit);

            // Act
            Quantity<Matrix<double>> result = left * right as Quantity<Matrix<double>>;

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
            Matrix<double> leftMatrix = new DenseMatrix(10, 15);
            Vector rightVector = new DenseVector(15);
            Quantity<Matrix<double>> left = new Quantity<Matrix<double>>(leftMatrix, unit);
            Quantity<Vector<double>> right = new Quantity<Vector<double>>(rightVector, unit);

            // Act
            Quantity<Vector<double>> result = left * right as Quantity<Vector<double>>;

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
            Vector<double> leftVector = new DenseVector(10);
            Matrix<double> rightMatrix = new DenseMatrix(10, 15);
            Quantity<Vector<double>> left = new Quantity<Vector<double>>(leftVector, unit);
            Quantity<Matrix<double>> right = new Quantity<Matrix<double>>(rightMatrix, unit);

            // Act
            Quantity<Vector<double>> result = left * right as Quantity<Vector<double>>;

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
            Matrix<double> rightMatrix = new DenseMatrix(10, 15);
            Quantity<float> left = new Quantity<float>(2.0f, unit);
            Quantity<Matrix<double>> right = new Quantity<Matrix<double>>(rightMatrix, unit);

            // Act
            Quantity<Matrix<double>> result = left * right as Quantity<Matrix<double>>;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.AreEqual(rightMatrix.RowCount, result.Value.RowCount);
            Assert.AreEqual(rightMatrix.ColumnCount, result.Value.ColumnCount);
        }
    }
}
