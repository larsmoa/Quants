using Moa.Units.Quantities;
using Moa.Units.Units;

namespace Moa.Units.MathNETCatalog
{
    public static class MathNetQuantitiesCatalog
    {
        private static bool _hasRegisteredOperations = false;

        public static void Setup()
        {
            if (_hasRegisteredOperations) return;
            _hasRegisteredOperations = true;
            
            // Multiplication
            ArithmeticsStore.Instance.RegisterMultiplyOperation<MathNet.Numerics.LinearAlgebra.Double.Matrix,
                                                                MathNet.Numerics.LinearAlgebra.Double.Matrix>(MultiplyMatrixMatrix);
            ArithmeticsStore.Instance.RegisterMultiplyOperation<MathNet.Numerics.LinearAlgebra.Double.Matrix,
                                                                MathNet.Numerics.LinearAlgebra.Double.Vector>(MultiplyMatrixVector);
            ArithmeticsStore.Instance.RegisterMultiplyOperation<MathNet.Numerics.LinearAlgebra.Double.Vector,
                                                                MathNet.Numerics.LinearAlgebra.Double.Matrix>(MultiplyVectorMatrix);
            ArithmeticsStore.Instance.RegisterMultiplyOperation<float, MathNet.Numerics.LinearAlgebra.Double.Matrix>(MultiplyFloatMatrix);
            ArithmeticsStore.Instance.RegisterMultiplyOperation<MathNet.Numerics.LinearAlgebra.Double.Matrix, float>(MultiplyMatrixFloat);
            ArithmeticsStore.Instance.RegisterMultiplyOperation<double, MathNet.Numerics.LinearAlgebra.Double.Matrix>(MultiplyDoubleMatrix);
            ArithmeticsStore.Instance.RegisterMultiplyOperation<MathNet.Numerics.LinearAlgebra.Double.Matrix, double>(MultiplyMatrixDouble);
            ArithmeticsStore.Instance.RegisterMultiplyOperation<float, MathNet.Numerics.LinearAlgebra.Double.Vector>(MultiplyFloatVector);
            ArithmeticsStore.Instance.RegisterMultiplyOperation<MathNet.Numerics.LinearAlgebra.Double.Vector, float>(MultiplyVectorFloat);
            ArithmeticsStore.Instance.RegisterMultiplyOperation<double, MathNet.Numerics.LinearAlgebra.Double.Vector>(MultiplyDoubleVector);
            ArithmeticsStore.Instance.RegisterMultiplyOperation<MathNet.Numerics.LinearAlgebra.Double.Vector, double>(MultiplyVectorDouble);
        }

        #region Multiply

        private static IUnit MultiplyUnits(QuantityBase left, QuantityBase right)
        {
            return new UnitCreator().Multiply(left.Unit, right.Unit).Create();
        }

        private static QuantityBase MultiplyMatrixMatrix(QuantityBase left, QuantityBase right)
        {
            Quantity<MathNet.Numerics.LinearAlgebra.Double.Matrix> leftQ = (Quantity<MathNet.Numerics.LinearAlgebra.Double.Matrix>) left;
            Quantity<MathNet.Numerics.LinearAlgebra.Double.Matrix> rightQ = (Quantity<MathNet.Numerics.LinearAlgebra.Double.Matrix>) right;
            
            IUnit unit = MultiplyUnits(left, right);
            MathNet.Numerics.LinearAlgebra.Double.Matrix matrix = leftQ.Value*rightQ.Value;
            return new Quantity<MathNet.Numerics.LinearAlgebra.Double.Matrix>(matrix, unit);
        }

        private static QuantityBase MultiplyMatrixVector(QuantityBase left, QuantityBase right)
        {
            Quantity<MathNet.Numerics.LinearAlgebra.Double.Matrix> leftQ = (Quantity<MathNet.Numerics.LinearAlgebra.Double.Matrix>)left;
            Quantity<MathNet.Numerics.LinearAlgebra.Double.Vector> rightQ = (Quantity<MathNet.Numerics.LinearAlgebra.Double.Vector>)right;

            IUnit unit = MultiplyUnits(left, right);
            MathNet.Numerics.LinearAlgebra.Double.Vector vector = leftQ.Value * rightQ.Value;
            return new Quantity<MathNet.Numerics.LinearAlgebra.Double.Vector>(vector, unit);
        }

        private static QuantityBase MultiplyVectorMatrix(QuantityBase left, QuantityBase right)
        {
            Quantity<MathNet.Numerics.LinearAlgebra.Double.Vector> leftQ = (Quantity<MathNet.Numerics.LinearAlgebra.Double.Vector>)left;
            Quantity<MathNet.Numerics.LinearAlgebra.Double.Matrix> rightQ = (Quantity<MathNet.Numerics.LinearAlgebra.Double.Matrix>)right;

            IUnit unit = MultiplyUnits(left, right);
            MathNet.Numerics.LinearAlgebra.Double.Vector vector = leftQ.Value * rightQ.Value;
            return new Quantity<MathNet.Numerics.LinearAlgebra.Double.Vector>(vector, unit);
        }

        private static QuantityBase MultiplyFloatMatrix(QuantityBase left, QuantityBase right)
        {
            Quantity<float> leftQ = (Quantity<float>) left;
            Quantity<MathNet.Numerics.LinearAlgebra.Double.Matrix> rightQ = (Quantity<MathNet.Numerics.LinearAlgebra.Double.Matrix>) right;

            IUnit unit = MultiplyUnits(left, right);
            MathNet.Numerics.LinearAlgebra.Double.Matrix matrix = leftQ.Value * rightQ.Value;
            return new Quantity<MathNet.Numerics.LinearAlgebra.Double.Matrix>(matrix, unit);
        }

        private static QuantityBase MultiplyMatrixFloat(QuantityBase left, QuantityBase right)
        {
            return MultiplyFloatMatrix(right, left);
        }

        private static QuantityBase MultiplyDoubleMatrix(QuantityBase left, QuantityBase right)
        {
            Quantity<double> leftQ = (Quantity<double>)left;
            Quantity<MathNet.Numerics.LinearAlgebra.Double.Matrix> rightQ = (Quantity<MathNet.Numerics.LinearAlgebra.Double.Matrix>)right;

            IUnit unit = MultiplyUnits(left, right);
            MathNet.Numerics.LinearAlgebra.Double.Matrix matrix = leftQ.Value * rightQ.Value;
            return new Quantity<MathNet.Numerics.LinearAlgebra.Double.Matrix>(matrix, unit);
        }

        private static QuantityBase MultiplyMatrixDouble(QuantityBase left, QuantityBase right)
        {
            return MultiplyDoubleMatrix(right, left);
        }

        private static QuantityBase MultiplyFloatVector(QuantityBase left, QuantityBase right)
        {
            Quantity<float> leftQ = (Quantity<float>)left;
            Quantity<MathNet.Numerics.LinearAlgebra.Double.Vector> rightQ = (Quantity<MathNet.Numerics.LinearAlgebra.Double.Vector>)right;

            IUnit unit = MultiplyUnits(left, right);
            MathNet.Numerics.LinearAlgebra.Double.Vector vector = leftQ.Value * rightQ.Value;
            return new Quantity<MathNet.Numerics.LinearAlgebra.Double.Vector>(vector, unit);
        }

        private static QuantityBase MultiplyVectorFloat(QuantityBase left, QuantityBase right)
        {
            return MultiplyFloatVector(right, left);
        }

        private static QuantityBase MultiplyDoubleVector(QuantityBase left, QuantityBase right)
        {
            Quantity<double> leftQ = (Quantity<double>)left;
            Quantity<MathNet.Numerics.LinearAlgebra.Double.Vector> rightQ = (Quantity<MathNet.Numerics.LinearAlgebra.Double.Vector>)right;

            IUnit unit = MultiplyUnits(left, right);
            MathNet.Numerics.LinearAlgebra.Double.Vector vector = leftQ.Value * rightQ.Value;
            return new Quantity<MathNet.Numerics.LinearAlgebra.Double.Vector>(vector, unit);
        }

        private static QuantityBase MultiplyVectorDouble(QuantityBase left, QuantityBase right)
        {
            return MultiplyDoubleVector(right, left);
        }

        #endregion
    }
}
