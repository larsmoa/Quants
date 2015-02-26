using System;
using System.Collections.Generic;

namespace Moa.Units.Quantities
{
    /// <summary>
    /// Class that maintains a register of operations that typically perform arithmetic 
    /// on quantities to form new quantities. Operations must be registered for every
    /// type combination that is to be supported - there is no checking for 'compatible'
    /// types such as int and float or subclasses of types registered.
    /// 
    /// This class is usually not used directly.
    /// <seealso cref="ArithmeticsStore"/>
    /// </summary>
    public class OperationStore
    {
        private struct OperationKey
        {
            private readonly Type _leftType;
            private readonly Type _rightType;

            public OperationKey(Type leftType, Type rightType)
            {
                _leftType = leftType;
                _rightType = rightType;
            }

            public override bool Equals(object obj)
            {
                if (obj == null || GetType() != obj.GetType())
                    return false;

                OperationKey other = (OperationKey) obj;
                bool equal = Equals(other._leftType, _leftType) && Equals(other._rightType, _rightType);
                return equal;
            }


            public override int GetHashCode()
            {
                unchecked
                {
                    int left = (_leftType != null) ? _leftType.Name.GetHashCode() : 873573;
                    int right = (_rightType != null) ? _rightType.Name.GetHashCode() : 3572962;
                    int hash = (left * 37) + right;
                    return hash;                    
                }
            }
        }

        private readonly Dictionary<OperationKey, Func<QuantityBase, QuantityBase, QuantityBase>> _operations;

        /// <summary>
        /// Creates an empty store.
        /// </summary>
        public OperationStore()
        {
            _operations = new Dictionary<OperationKey, Func<QuantityBase, QuantityBase, QuantityBase>>();
        }
        /// <summary>
        /// Registers a new operation. The operations are not commutative, i.e. 
        /// Perform(left, right) is not necessary equals Perform(right, left).
        /// </summary>
        /// <param name="leftType">The left type. Subclass of QuantityBase, or typically Quantity&lt;T&gt;.</param>
        /// <param name="rightType">The right type. Subclass of QuantityBase, or typically Quantity&lt;T&gt;.</param>
        /// <param name="operation">A function accepting TLeft and TRight as arguments, and returning a QuantityBase or typically Quantity&lt;T&gt;.</param>
        public void AddOperation(Type leftType, Type rightType, Func<QuantityBase, QuantityBase, QuantityBase> operation)
        {
            if (!leftType.IsSubclassOf(typeof(QuantityBase)))
                throw new InvalidCastException(string.Format("'{0}' must inherit '{1}'", leftType.Name,
                                                             typeof (QuantityBase).Name));
            if (!rightType.IsSubclassOf(typeof(QuantityBase)))
                throw new InvalidCastException(string.Format("'{0}' must inherit '{1}'", rightType.Name,
                                                             typeof(QuantityBase).Name));

            OperationKey key = new OperationKey(leftType, rightType);
            _operations.Add(key, operation);
        }


        /// <summary>
        /// Registers a new operation. The operations are not commutative, i.e. 
        /// Perform(left, right) is not necessary equals Perform(right, left).
        /// This is a convinence version for operations that work on Quantity&lt;T&gt;.
        /// </summary>
        /// <typeparam name="TLeft">The left value type. Quantity type registered will be Quantity&lt;TLeft&gt;.</typeparam>
        /// <typeparam name="TRight">The right value type. Quantity type registered will be Quantity&lt;TRight&gt;.</typeparam>
        /// <param name="operation">A function accepting Quantity&lt;TLeft&gt; and Quantity&lt;TRight&gt; as arguments, and returning a QuantityBase or typically Quantity&lt;T&gt;.</param>
        public void AddOperation<TLeft, TRight>(Func<QuantityBase, QuantityBase, QuantityBase> operation) 
        {
            AddOperation(typeof(Quantity<TLeft>), typeof(Quantity<TRight>), operation);
        }

        /// <summary>
        /// Performns a pre-registered operation on the left and right quantities. The operation 
        /// must be registreded using AddOperation.
        /// </summary>
        /// <exception cref="NullReferenceException">If any of the arguments are null.</exception>
        /// <exception cref="InvalidOperationException">If the operation are not supported.</exception>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public QuantityBase PerformOperation(QuantityBase left, QuantityBase right)
        {
            if (left == null || right == null)
                throw new NullReferenceException("Arguments cannot be null.");

            Func<QuantityBase, QuantityBase, QuantityBase> operation;
            OperationKey key = new OperationKey(left.GetType(), right.GetType());
            if (_operations.TryGetValue(key, out operation))
            {
                return operation(left, right);
            }
            
            throw new InvalidOperationException(string.Format("Operation op('{0}', '{1}') is not supported.", left.GetType(), right.GetType()));
        }
    }
}