using Quants.Quantities;
using Quants.Units;

namespace Quants.MathNETCatalog
{
    public static class MathNetQuantitiesCatalog
    {
        private static bool _hasRegisteredOperations = false;

        public static void Setup()
        {
            if (_hasRegisteredOperations) return;
            _hasRegisteredOperations = true;
            
            // Multiplication
            ArithmeticsStore.Instance.RegisterMultiplyOperation<MathNet.Numerics.LinearAlgebra.Matrix<double>,
                                                                MathNet.Numerics.LinearAlgebra.Matrix<double>>(MultiplyMatrixMatrix);
            ArithmeticsStore.Instance.RegisterMultiplyOperation<MathNet.Numerics.LinearAlgebra.Matrix<double>,
                                                                MathNet.Numerics.LinearAlgebra.Vector<double>>(MultiplyMatrixVector);
            ArithmeticsStore.Instance.RegisterMultiplyOperation<MathNet.Numerics.LinearAlgebra.Vector<double>,
                                                                MathNet.Numerics.LinearAlgebra.Matrix<double>>(MultiplyVectorMatrix);
            ArithmeticsStore.Instance.RegisterMultiplyOperation<float, MathNet.Numerics.LinearAlgebra.Matrix<double>>(MultiplyFloatMatrix);
            ArithmeticsStore.Instance.RegisterMultiplyOperation<MathNet.Numerics.LinearAlgebra.Matrix<double>, float>(MultiplyMatrixFloat);
            ArithmeticsStore.Instance.RegisterMultiplyOperation<double, MathNet.Numerics.LinearAlgebra.Matrix<double>>(MultiplyDoubleMatrix);
            ArithmeticsStore.Instance.RegisterMultiplyOperation<MathNet.Numerics.LinearAlgebra.Matrix<double>, double>(MultiplyMatrixDouble);
            ArithmeticsStore.Instance.RegisterMultiplyOperation<float, MathNet.Numerics.LinearAlgebra.Vector<double>>(MultiplyFloatVector);
            ArithmeticsStore.Instance.RegisterMultiplyOperation<MathNet.Numerics.LinearAlgebra.Vector<double>, float>(MultiplyVectorFloat);
            ArithmeticsStore.Instance.RegisterMultiplyOperation<double, MathNet.Numerics.LinearAlgebra.Vector<double>>(MultiplyDoubleVector);
            ArithmeticsStore.Instance.RegisterMultiplyOperation<MathNet.Numerics.LinearAlgebra.Vector<double>, double>(MultiplyVectorDouble);
        }

        #region Multiply

        private static IUnit MultiplyUnits(QuantityBase left, QuantityBase right)
        {
            return new UnitCreator().Multiply(left.Unit, right.Unit).Create();
        }

        private static QuantityBase MultiplyMatrixMatrix(QuantityBase left, QuantityBase right)
        {
            Quantity<MathNet.Numerics.LinearAlgebra.Matrix<double>> leftQ = (Quantity<MathNet.Numerics.LinearAlgebra.Matrix<double>>) left;
            Quantity<MathNet.Numerics.LinearAlgebra.Matrix<double>> rightQ = (Quantity<MathNet.Numerics.LinearAlgebra.Matrix<double>>) right;
            
            IUnit unit = MultiplyUnits(left, right);
            MathNet.Numerics.LinearAlgebra.Matrix<double> matrix = leftQ.Value*rightQ.Value;
            return new Quantity<MathNet.Numerics.LinearAlgebra.Matrix<double>>(matrix, unit);
        }

        private static QuantityBase MultiplyMatrixVector(QuantityBase left, QuantityBase right)
        {
            Quantity<MathNet.Numerics.LinearAlgebra.Matrix<double>> leftQ = (Quantity<MathNet.Numerics.LinearAlgebra.Matrix<double>>)left;
            Quantity<MathNet.Numerics.LinearAlgebra.Vector<double>> rightQ = (Quantity<MathNet.Numerics.LinearAlgebra.Vector<double>>)right;

            IUnit unit = MultiplyUnits(left, right);
            MathNet.Numerics.LinearAlgebra.Vector<double> vector = leftQ.Value * rightQ.Value;
            return new Quantity<MathNet.Numerics.LinearAlgebra.Vector<double>>(vector, unit);
        }

        private static QuantityBase MultiplyVectorMatrix(QuantityBase left, QuantityBase right)
        {
            Quantity<MathNet.Numerics.LinearAlgebra.Vector<double>> leftQ = (Quantity<MathNet.Numerics.LinearAlgebra.Vector<double>>)left;
            Quantity<MathNet.Numerics.LinearAlgebra.Matrix<double>> rightQ = (Quantity<MathNet.Numerics.LinearAlgebra.Matrix<double>>)right;

            IUnit unit = MultiplyUnits(left, right);
            MathNet.Numerics.LinearAlgebra.Vector<double> vector = leftQ.Value * rightQ.Value;
            return new Quantity<MathNet.Numerics.LinearAlgebra.Vector<double>>(vector, unit);
        }

        private static QuantityBase MultiplyFloatMatrix(QuantityBase left, QuantityBase right)
        {
            Quantity<float> leftQ = (Quantity<float>) left;
            Quantity<MathNet.Numerics.LinearAlgebra.Matrix<double>> rightQ = (Quantity<MathNet.Numerics.LinearAlgebra.Matrix<double>>) right;

            IUnit unit = MultiplyUnits(left, right);
            MathNet.Numerics.LinearAlgebra.Matrix<double> matrix = leftQ.Value * rightQ.Value;
            return new Quantity<MathNet.Numerics.LinearAlgebra.Matrix<double>>(matrix, unit);
        }

        private static QuantityBase MultiplyMatrixFloat(QuantityBase left, QuantityBase right)
        {
            return MultiplyFloatMatrix(right, left);
        }

        private static QuantityBase MultiplyDoubleMatrix(QuantityBase left, QuantityBase right)
        {
            Quantity<double> leftQ = (Quantity<double>)left;
            Quantity<MathNet.Numerics.LinearAlgebra.Matrix<double>> rightQ = (Quantity<MathNet.Numerics.LinearAlgebra.Matrix<double>>)right;

            IUnit unit = MultiplyUnits(left, right);
            MathNet.Numerics.LinearAlgebra.Matrix<double> matrix = leftQ.Value * rightQ.Value;
            return new Quantity<MathNet.Numerics.LinearAlgebra.Matrix<double>>(matrix, unit);
        }

        private static QuantityBase MultiplyMatrixDouble(QuantityBase left, QuantityBase right)
        {
            return MultiplyDoubleMatrix(right, left);
        }

        private static QuantityBase MultiplyFloatVector(QuantityBase left, QuantityBase right)
        {
            Quantity<float> leftQ = (Quantity<float>)left;
            Quantity<MathNet.Numerics.LinearAlgebra.Vector<double>> rightQ = (Quantity<MathNet.Numerics.LinearAlgebra.Vector<double>>)right;

            IUnit unit = MultiplyUnits(left, right);
            MathNet.Numerics.LinearAlgebra.Vector<double> vector = leftQ.Value * rightQ.Value;
            return new Quantity<MathNet.Numerics.LinearAlgebra.Vector<double>>(vector, unit);
        }

        private static QuantityBase MultiplyVectorFloat(QuantityBase left, QuantityBase right)
        {
            return MultiplyFloatVector(right, left);
        }

        private static QuantityBase MultiplyDoubleVector(QuantityBase left, QuantityBase right)
        {
            Quantity<double> leftQ = (Quantity<double>)left;
            Quantity<MathNet.Numerics.LinearAlgebra.Vector<double>> rightQ = (Quantity<MathNet.Numerics.LinearAlgebra.Vector<double>>)right;

            IUnit unit = MultiplyUnits(left, right);
            MathNet.Numerics.LinearAlgebra.Vector<double> vector = leftQ.Value * rightQ.Value;
            return new Quantity<MathNet.Numerics.LinearAlgebra.Vector<double>>(vector, unit);
        }

        private static QuantityBase MultiplyVectorDouble(QuantityBase left, QuantityBase right)
        {
            return MultiplyDoubleVector(right, left);
        }

        #endregion
    }
}
