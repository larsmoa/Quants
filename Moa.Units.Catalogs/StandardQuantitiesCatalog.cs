using Moa.Units.Quantities;
using Moa.Units.Units;

namespace Moa.Units.Catalogs
{
    /// <summary>
    /// Class that contains definitions of the default <see cref="Quantity{T}"/> and
    /// defines arithmetic operations for these.
    /// The types supported by this class is float, double, int and long.
    /// 
    /// If both quantities are the same the resulting quantity type (<see cref="QuantityBase.QuantityValueType"/>)
    /// will be unchanged, but if the types differ the results will be as follows:
    /// <list type="table">
    /// <listheader>
    ///     <term>Operation</term>
    ///     <description>Resulting quantity value type</description>
    /// </listheader>
    /// <item>
    ///     <term>double*float or float*double</term>
    ///     <description>double</description>
    /// </item>
    /// <item>
    ///     <term>double*int or int*double</term>
    ///     <description>double</description>
    /// </item>
    /// <item>
    ///     <term>double*long or long*double</term>
    ///     <description>double</description>
    /// </item>
    /// <item>
    ///     <term>float*int or int*float</term>
    ///     <description>float</description>
    /// </item>
    /// <item>
    ///     <term>float*long or long*float</term>
    ///     <description>float</description>
    /// </item>
    /// <item>
    ///     <term>int*long or long*int</term>
    ///     <description>long</description>
    /// </item>
    /// <item>
    ///     <term>double / float or float / double</term>
    ///     <description>double</description>
    /// </item>
    /// <item>
    ///     <term>double / int or int / double</term>
    ///     <description>double</description>
    /// </item>
    /// <item>
    ///     <term>double / long or long / double</term>
    ///     <description>double</description>
    /// </item>
    /// <item>
    ///     <term>int / long or long / int</term>
    ///     <description>long</description>
    /// </item>
    /// </list>
    /// </summary>
    public static class StandardQuantitiesCatalog
    {
        private static bool _hasRegisteredOperations = false;

        public static void Setup()
        {
            if (_hasRegisteredOperations) return;
            _hasRegisteredOperations = true;

            // Multiply operations
            ArithmeticsStore.Instance.RegisterMultiplyOperation<float, float>(MultiplyFloatFloat);
            ArithmeticsStore.Instance.RegisterMultiplyOperation<double, double>(MultiplyDoubleDouble);
            ArithmeticsStore.Instance.RegisterMultiplyOperation<int, int>(MultiplyIntInt);
            ArithmeticsStore.Instance.RegisterMultiplyOperation<long, long>(MultiplyLongLong);

            ArithmeticsStore.Instance.RegisterMultiplyOperation<double, float>(MultiplyDoubleFloat);
            ArithmeticsStore.Instance.RegisterMultiplyOperation<float, double>(MultiplyFloatDouble);
            ArithmeticsStore.Instance.RegisterMultiplyOperation<double, int>(MultiplyDoubleInt);
            ArithmeticsStore.Instance.RegisterMultiplyOperation<int, double>(MultiplyIntDouble);
            ArithmeticsStore.Instance.RegisterMultiplyOperation<double, long>(MultiplyDoubleLong);
            ArithmeticsStore.Instance.RegisterMultiplyOperation<long, double>(MultiplyLongDouble);

            ArithmeticsStore.Instance.RegisterMultiplyOperation<float, int>(MultiplyFloatInt);
            ArithmeticsStore.Instance.RegisterMultiplyOperation<int, float>(MultiplyIntFloat);
            ArithmeticsStore.Instance.RegisterMultiplyOperation<float, long>(MultiplyFloatLong);
            ArithmeticsStore.Instance.RegisterMultiplyOperation<long, float>(MultiplyLongFloat);

            ArithmeticsStore.Instance.RegisterMultiplyOperation<int, long>(MultiplyIntLong);
            ArithmeticsStore.Instance.RegisterMultiplyOperation<long, int>(MultiplyLongInt);

            // Divide operations
            ArithmeticsStore.Instance.RegisterDivideOperation<float, float>(DivideFloatFloat);
            ArithmeticsStore.Instance.RegisterDivideOperation<double, double>(DivideDoubleDouble);
            ArithmeticsStore.Instance.RegisterDivideOperation<int, int>(DivideIntInt);
            ArithmeticsStore.Instance.RegisterDivideOperation<long, long>(DivideLongLong);

            ArithmeticsStore.Instance.RegisterDivideOperation<double, float>(DivideDoubleFloat);
            ArithmeticsStore.Instance.RegisterDivideOperation<float, double>(DivideFloatDouble);
            ArithmeticsStore.Instance.RegisterDivideOperation<double, int>(DivideDoubleInt);
            ArithmeticsStore.Instance.RegisterDivideOperation<int, double>(DivideIntDouble);
            ArithmeticsStore.Instance.RegisterDivideOperation<double, long>(DivideDoubleLong);
            ArithmeticsStore.Instance.RegisterDivideOperation<long, double>(DivideLongDouble);

            ArithmeticsStore.Instance.RegisterDivideOperation<float, int>(DivideFloatInt);
            ArithmeticsStore.Instance.RegisterDivideOperation<int, float>(DivideIntFloat);
            ArithmeticsStore.Instance.RegisterDivideOperation<float, long>(DivideFloatLong);
            ArithmeticsStore.Instance.RegisterDivideOperation<long, float>(DivideLongFloat);

            ArithmeticsStore.Instance.RegisterDivideOperation<int, long>(DivideIntLong);
            ArithmeticsStore.Instance.RegisterDivideOperation<long, int>(DivideLongInt);

            // Add operations
            ArithmeticsStore.Instance.RegisterAddOperation<int>(AddInt);
            ArithmeticsStore.Instance.RegisterAddOperation<long>(AddLong);
            ArithmeticsStore.Instance.RegisterAddOperation<float>(AddFloat);
            ArithmeticsStore.Instance.RegisterAddOperation<double>(AddDouble);

            // Subtract operations
            ArithmeticsStore.Instance.RegisterSubtractOperation<int>(SubtractInt);
            ArithmeticsStore.Instance.RegisterSubtractOperation<long>(SubtractLong);
            ArithmeticsStore.Instance.RegisterSubtractOperation<float>(SubtractFloat);
            ArithmeticsStore.Instance.RegisterSubtractOperation<double>(SubtractDouble);
        }

        #region Multiply

        private static IUnit MultiplyUnits(QuantityBase left, QuantityBase right)
        {
            return new UnitCreator().Multiply(left.Unit, right.Unit).Create();
        }

        private static Quantity<float> MultiplyFloatFloat(QuantityBase left, QuantityBase right)
        {
            IUnit unit = MultiplyUnits(left, right);
            return new Quantity<float>(((Quantity<float>)left).Value * ((Quantity<float>)right).Value, unit);
        }

        private static Quantity<double> MultiplyDoubleDouble(QuantityBase left, QuantityBase right)
        {
            IUnit unit = MultiplyUnits(left, right);
            return new Quantity<double>(((Quantity<double>)left).Value * ((Quantity<double>)right).Value, unit);
        }

        private static Quantity<int> MultiplyIntInt(QuantityBase left, QuantityBase right)
        {
            IUnit unit = MultiplyUnits(left, right);
            return new Quantity<int>(((Quantity<int>)left).Value * ((Quantity<int>)right).Value, unit);
        }

        private static Quantity<long> MultiplyLongLong(QuantityBase left, QuantityBase right)
        {
            IUnit unit = MultiplyUnits(left, right);
            return new Quantity<long>(((Quantity<long>)left).Value * ((Quantity<long>)right).Value, unit);
        }

        private static Quantity<double> MultiplyFloatDouble(QuantityBase left, QuantityBase right)
        {
            IUnit unit = MultiplyUnits(left, right);
            return new Quantity<double>(((Quantity<float>)left).Value * ((Quantity<double>)right).Value, unit);
        }

        private static Quantity<double> MultiplyDoubleFloat(QuantityBase left, QuantityBase right)
        {
            return MultiplyFloatDouble(right, left);
        }

        private static Quantity<double> MultiplyIntDouble(QuantityBase left, QuantityBase right)
        {
            IUnit unit = MultiplyUnits(left, right);
            return new Quantity<double>(((Quantity<int>)left).Value * ((Quantity<double>)right).Value, unit);
        }

        private static Quantity<double> MultiplyDoubleInt(QuantityBase left, QuantityBase right)
        {
            return MultiplyIntDouble(right, left);
        }

        private static Quantity<double> MultiplyLongDouble(QuantityBase left, QuantityBase right)
        {
            IUnit unit = MultiplyUnits(left, right);
            return new Quantity<double>(((Quantity<long>)left).Value * ((Quantity<double>)right).Value, unit);
        }

        private static Quantity<double> MultiplyDoubleLong(QuantityBase left, QuantityBase right)
        {
            return MultiplyLongDouble(right, left);
        }

        private static Quantity<float> MultiplyIntFloat(QuantityBase left, QuantityBase right)
        {
            IUnit unit = MultiplyUnits(left, right);
            return new Quantity<float>(((Quantity<int>)left).Value * ((Quantity<float>)right).Value, unit);
        }

        private static Quantity<float> MultiplyFloatInt(QuantityBase left, QuantityBase right)
        {
            return MultiplyIntFloat(right, left);
        }

        private static Quantity<float> MultiplyLongFloat(QuantityBase left, QuantityBase right)
        {
            IUnit unit = MultiplyUnits(left, right);
            return new Quantity<float>(((Quantity<long>)left).Value * ((Quantity<float>)right).Value, unit);
        }

        private static Quantity<float> MultiplyFloatLong(QuantityBase left, QuantityBase right)
        {
            return MultiplyLongFloat(right, left);
        }

        private static Quantity<long> MultiplyLongInt(QuantityBase left, QuantityBase right)
        {
            IUnit unit = MultiplyUnits(left, right);
            return new Quantity<long>(((Quantity<long>)left).Value * ((Quantity<int>)right).Value, unit);
        }

        private static Quantity<long> MultiplyIntLong(QuantityBase left, QuantityBase right)
        {
            return MultiplyLongInt(right, left);
        }

        #endregion

        #region Divide

        private static IUnit DivideUnits(QuantityBase left, QuantityBase right)
        {
            return new UnitCreator(left.Unit).Divide(right.Unit).Create();
        }

        private static Quantity<float> DivideFloatFloat(QuantityBase left, QuantityBase right)
        {
            IUnit unit = DivideUnits(left, right);
            return new Quantity<float>(((Quantity<float>)left).Value / ((Quantity<float>)right).Value, unit);
        }

        private static Quantity<double> DivideDoubleDouble(QuantityBase left, QuantityBase right)
        {
            IUnit unit = DivideUnits(left, right);
            return new Quantity<double>(((Quantity<double>)left).Value / ((Quantity<double>)right).Value, unit);
        }

        private static Quantity<int> DivideIntInt(QuantityBase left, QuantityBase right)
        {
            IUnit unit = DivideUnits(left, right);
            return new Quantity<int>(((Quantity<int>)left).Value / ((Quantity<int>)right).Value, unit);
        }

        private static Quantity<long> DivideLongLong(QuantityBase left, QuantityBase right)
        {
            IUnit unit = DivideUnits(left, right);
            return new Quantity<long>(((Quantity<long>)left).Value / ((Quantity<long>)right).Value, unit);
        }

        private static Quantity<double> DivideFloatDouble(QuantityBase left, QuantityBase right)
        {
            IUnit unit = DivideUnits(left, right);
            return new Quantity<double>(((Quantity<float>)left).Value / ((Quantity<double>)right).Value, unit);
        }

        private static Quantity<double> DivideDoubleFloat(QuantityBase left, QuantityBase right)
        {
            return DivideFloatDouble(right, left);
        }

        private static Quantity<double> DivideIntDouble(QuantityBase left, QuantityBase right)
        {
            IUnit unit = DivideUnits(left, right);
            return new Quantity<double>(((Quantity<int>)left).Value / ((Quantity<double>)right).Value, unit);
        }

        private static Quantity<double> DivideDoubleInt(QuantityBase left, QuantityBase right)
        {
            return DivideIntDouble(right, left);
        }

        private static Quantity<double> DivideLongDouble(QuantityBase left, QuantityBase right)
        {
            IUnit unit = DivideUnits(left, right);
            return new Quantity<double>(((Quantity<long>)left).Value / ((Quantity<double>)right).Value, unit);
        }

        private static Quantity<double> DivideDoubleLong(QuantityBase left, QuantityBase right)
        {
            return DivideLongDouble(right, left);
        }

        private static Quantity<float> DivideIntFloat(QuantityBase left, QuantityBase right)
        {
            IUnit unit = DivideUnits(left, right);
            return new Quantity<float>(((Quantity<int>)left).Value / ((Quantity<float>)right).Value, unit);
        }

        private static Quantity<float> DivideFloatInt(QuantityBase left, QuantityBase right)
        {
            return DivideIntFloat(right, left);
        }

        private static Quantity<float> DivideLongFloat(QuantityBase left, QuantityBase right)
        {
            IUnit unit = DivideUnits(left, right);
            return new Quantity<float>(((Quantity<long>)left).Value / ((Quantity<float>)right).Value, unit);
        }

        private static Quantity<float> DivideFloatLong(QuantityBase left, QuantityBase right)
        {
            return DivideLongFloat(right, left);
        }

        private static Quantity<long> DivideLongInt(QuantityBase left, QuantityBase right)
        {
            IUnit unit = DivideUnits(left, right);
            return new Quantity<long>(((Quantity<long>)left).Value / ((Quantity<int>)right).Value, unit);
        }

        private static Quantity<long> DivideIntLong(QuantityBase left, QuantityBase right)
        {
            return DivideLongInt(right, left);
        }

        #endregion

        #region Add

        public static Quantity<float> AddFloat(QuantityBase left, QuantityBase right)
        {
            float value = ((Quantity<float>) left).Value + ((Quantity<float>) right).Value;
            return new Quantity<float>(value, left.Unit);
        }

        public static Quantity<double> AddDouble(QuantityBase left, QuantityBase right)
        {
            double value = ((Quantity<double>)left).Value + ((Quantity<double>)right).Value;
            return new Quantity<double>(value, left.Unit);
        }

        public static Quantity<int> AddInt(QuantityBase left, QuantityBase right)
        {
            int value = ((Quantity<int>)left).Value + ((Quantity<int>)right).Value;
            return new Quantity<int>(value, left.Unit);
        }

        public static Quantity<long> AddLong(QuantityBase left, QuantityBase right)
        {
            long value = ((Quantity<long>)left).Value + ((Quantity<long>)right).Value;
            return new Quantity<long>(value, left.Unit);
        }

        #endregion

        #region Subtract

        public static Quantity<float> SubtractFloat(QuantityBase left, QuantityBase right)
        {
            float value = ((Quantity<float>)left).Value - ((Quantity<float>)right).Value;
            return new Quantity<float>(value, left.Unit);
        }

        public static Quantity<double> SubtractDouble(QuantityBase left, QuantityBase right)
        {
            double value = ((Quantity<double>)left).Value - ((Quantity<double>)right).Value;
            return new Quantity<double>(value, left.Unit);
        }

        public static Quantity<int> SubtractInt(QuantityBase left, QuantityBase right)
        {
            int value = ((Quantity<int>)left).Value - ((Quantity<int>)right).Value;
            return new Quantity<int>(value, left.Unit);
        }

        public static Quantity<long> SubtractLong(QuantityBase left, QuantityBase right)
        {
            long value = ((Quantity<long>)left).Value - ((Quantity<long>)right).Value;
            return new Quantity<long>(value, left.Unit);
        }

        #endregion
    }
}
