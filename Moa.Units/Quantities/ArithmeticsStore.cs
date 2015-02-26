using System;

namespace Quants.Quantities
{
    /// <summary>
    /// Singleton class responsible for maintaining callbacks for performing arithmetics on
    /// quantities.
    /// </summary>
    public class ArithmeticsStore
    {
        private static ArithmeticsStore _instance;

        /// <summary>
        /// Returns the singleton instance.
        /// </summary>
        public static ArithmeticsStore Instance
        {
            get 
            { 
                if (_instance == null)
                    _instance = new ArithmeticsStore();
                return _instance;
            }
        }

        private readonly OperationStore _multiplicationStore;
        private readonly OperationStore _divisionStore;
        private readonly OperationStore _additionStore;
        private readonly OperationStore _substractionStore;

        protected ArithmeticsStore()
        {
            _multiplicationStore = new OperationStore();
            _divisionStore = new OperationStore();
            _additionStore = new OperationStore();
            _substractionStore = new OperationStore();
        }

        /// <summary>
        /// Registers a multiplication operation Quantity&lt;TLeft&gt; * Quantity&lt;TRight&gt;. The
        /// multiplication is not commutative, Quantity&lt;TLeft&gt; * Quantity&lt;TRight&gt; is not
        /// necessarily equals to Quantity&lt;TRight&gt; * Quantity&lt;TLeft&gt;. Therefore the
        /// store distinguishes between these.
        /// </summary>
        /// <typeparam name="TLeft">Data value type for the left Quantity.</typeparam>
        /// <typeparam name="TRight">Data value type for the right Quantity.</typeparam>
        /// <param name="operation"></param>
        public void RegisterMultiplyOperation<TLeft, TRight>(Func<QuantityBase, QuantityBase, QuantityBase> operation)
        {
            _multiplicationStore.AddOperation<TLeft, TRight>(operation);
        }

        /// <summary>
        /// Registers a multiplication operation leftType * rightType. The
        /// multiplication is not commutative, leftType * rightType; is not
        /// necessarily equals to rightType * leftType. Therefore the
        /// store distinguishes between these.
        /// </summary>
        /// <param name="leftType">Quantity type for the left Quantity. Must be subtype of QuantityBase.</param>
        /// <param name="rightType">Quantity type for the right Quantity. Must be subtype of QuantityBase.</param>
        /// <param name="operation"></param>
        public void RegisterMultiplyOperation(Type leftType, Type rightType, Func<QuantityBase, QuantityBase, QuantityBase> operation)
        {
            _multiplicationStore.AddOperation(leftType, rightType, operation);
        }

        /// <summary>
        /// Registers a division operation Quantity&lt;TLeft&gt; / Quantity&lt;TRight&gt;.
        /// </summary>
        /// <typeparam name="TLeft">Data value type for the left Quantity.</typeparam>
        /// <typeparam name="TRight">Data value type for the right Quantity.</typeparam>
        /// <param name="operation"></param>
        public void RegisterDivideOperation<TLeft, TRight>(Func<QuantityBase, QuantityBase, QuantityBase> operation)
        {
            _divisionStore.AddOperation<TLeft, TRight>(operation);
        }

        /// <summary>
        /// Registers a division operation leftType / rightType.
        /// </summary>
        /// <param name="leftType">Quantity type for the left Quantity. Must be subtype of QuantityBase.</param>
        /// <param name="rightType">Quantity type for the right Quantity. Must be subtype of QuantityBase.</param>
        /// <param name="operation"></param>
        public void RegisterDivideOperation(Type leftType, Type rightType, Func<QuantityBase, QuantityBase, QuantityBase> operation)
        {
            _divisionStore.AddOperation(leftType, rightType, operation);
        }
        
        /// <summary>
        /// Registers an addition operation Quantity&lt;T&gt; + Quantity&lt;T&gt;.
        /// </summary>
        /// <typeparam name="T">Data value type for the Quantities.</typeparam>
        /// <param name="operation"></param>
        public void RegisterAddOperation<T>(Func<QuantityBase, QuantityBase, QuantityBase> operation)
        {
            _additionStore.AddOperation<T, T>(operation);
        }

        /// <summary>
        /// Registers an addition operation leftType + rightType.
        /// </summary>
        /// <param name="termType">Quantity type for the Quantities. Must be subtype of QuantityBase.</param>
        /// <param name="operation"></param>
        public void RegisterAddOperation(Type termType, Func<QuantityBase, QuantityBase, QuantityBase> operation)
        {
            _additionStore.AddOperation(termType, termType, operation);
        }

        /// <summary>
        /// Registers an subtraction operation Quantity&lt;T&gt; - Quantity&lt;T&gt;.
        /// </summary>
        /// <typeparam name="T">Data value type for the Quantities.</typeparam>
        /// <param name="operation"></param>
        public void RegisterSubtractOperation<T>(Func<QuantityBase, QuantityBase, QuantityBase> operation)
        {
            _substractionStore.AddOperation<T, T>(operation);
        }

        /// <summary>
        /// Registers an addition operation termType - termType.
        /// </summary>
        /// <param name="termType">Quantity type for the Quantities. Must be subtype of QuantityBase.</param>
        /// <param name="operation"></param>
        public void RegisterSubtractOperation(Type termType, Func<QuantityBase, QuantityBase, QuantityBase> operation)
        {
            _substractionStore.AddOperation(termType, termType, operation);
        }

        /// <summary>
        /// Multiplies the left quantity with the right one. There must be a registered multiplication
        /// operation for the given types.
        /// </summary>
        /// <exception cref="InvalidOperationException">When there is no registered operation for the types.</exception>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>left*right</returns>
        public QuantityBase Multiply(QuantityBase left, QuantityBase right)
        {
            return _multiplicationStore.PerformOperation(left, right);
        }

        /// <summary>
        /// Divides the dividend quantity with the divisor quantity. There must be a registered division
        /// operation for the given types.
        /// </summary>
        /// <exception cref="InvalidOperationException">When there is no registered operation for the types.</exception>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns>dividend / divisor</returns>
        public QuantityBase Divide(QuantityBase dividend, QuantityBase divisor)
        {
            return _divisionStore.PerformOperation(dividend, divisor);
        }

        /// <summary>
        /// Sums the two quantities given. The exact type of the two quantities must be the same.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// When there is no registered operation for the type, or the type of the terms differ.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When the units of the provided quantities differ.
        /// </exception>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>dividend + divisor</returns>
        public QuantityBase Add(QuantityBase left, QuantityBase right)
        {
            // Units must be equal
            if (!Equals(left.Unit, right.Unit))
                throw new ArgumentException(string.Format("Units of quantities must be the same (was '{0}' and '{1}'", left.Unit, right.Unit));
            return _additionStore.PerformOperation(left, right);
        }

        /// <summary>
        /// Subtracts the right quantity from the left one. The exact type of the two quantities must be the same.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// When there is no registered operation for the type, or the type of the terms differ.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// When the units of the provided quantities differ.
        /// </exception>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>left - right</returns>
        public QuantityBase Subtract(QuantityBase left, QuantityBase right)
        {
            // Units must be equal
            if (!Equals(left.Unit, right.Unit))
                throw new ArgumentException(string.Format("Units of quantities must be the same (was '{0}' and '{1}'", left.Unit, right.Unit));
            return _substractionStore.PerformOperation(left, right);
        }
    }
}
