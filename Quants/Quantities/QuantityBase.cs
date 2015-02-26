using System;

namespace Quants.Quantities
{
    /// <summary>
    /// Base class for quantities. This class is an over-simplification of quantities, but 
    /// is a necessary "evil" because there's a need for a non-templated base class. Usually 
    /// this class is merly used as a return type, in general Quantity&lt;T&gt; is used as the
    /// top level quantity type. However, as some functions in Quantity&lt;T&gt; return 
    /// QuantityBase, casts are necessary. In rare cases where the returned type cannot be 
    /// determined programatically, this class might be of use. 
    /// </summary>
    public abstract class QuantityBase
    {
        /// <summary>
        /// Returns the unit of the quantity.
        /// </summary>
        public abstract IUnit Unit { get; }
        /// <summary>
        /// Returns the dimension of the quantity.
        /// </summary>
        public abstract IDimension Dimension { get; }
        /// <summary>
        /// Returns the type of the quantity value.
        /// </summary>
        public abstract Type QuantityValueType { get; }

        /// <summary>
        /// Returns the (possibly boxed) value of the quantity.
        /// </summary>
        public abstract object QuantityValue { get; }

        /// <summary>
        /// Converts the quantity to the type given if possible. If impossible, the success-parameter
        /// is set to false. This function has a big overhead, and usage should be avoided.
        /// 
        /// Uses System.Convert to do conversion.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="success"></param>
        /// <returns>The value if cast is possible, default(T) if not.</returns>
        public T ValueCast<T>(out bool success)
        {
            success = false;
            try
            {
                T value = (T) Convert.ChangeType(QuantityValue, typeof(T));
                success = true;
                return value;
            }
            catch (InvalidCastException)
            {
            }
            catch (FormatException)
            {
            }
            catch (OverflowException)
            {
            }
            return default(T);
        }

        /// <summary>
        /// Converts the quantity to the type given. Throws InvalidCastException if the cast cannot be performed.
        /// This function has a big overhead, and usage should be avoided.
        ///
        /// Uses System.Convert to do conversion.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ValueCast<T>()
        {
            try
            {
                return (T) Convert.ChangeType(QuantityValue, typeof(T));
            }
            catch (FormatException e)
            {
                throw new InvalidCastException(string.Format("Could not convert quantity with type '{0}' to type '{1}'",
                                               QuantityValueType.Name, typeof (T).Name), e);
            }
            catch (OverflowException e)
            {
                throw new InvalidCastException(string.Format("Could not convert quantity with type '{0}' to type '{1}'",
                                               QuantityValueType.Name, typeof(T).Name), e);
            }
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", QuantityValue, Unit.Unit);
        }

        /// <summary>
        /// Calls <see cref="ArithmeticsStore.Multiply" /> and returns the result.
        /// The concrete type combination must be registered in the <see cref="ArithmeticsStore"/> for this function to succeed.
        /// </summary>
        /// <param name="left">QuantityBase. Must be registered in the active ArithmeticsStore.</param>
        /// <param name="right">QuantityBase. Must be registered in the active ArithmeticsStore.</param>
        /// <returns></returns>
        /// <seealso cref="ArithmeticsStore.Multiply"/>
        public static QuantityBase operator*(QuantityBase left, QuantityBase right)
        {
            VerifySameUnitsIfSameDimension(left, right);
            return ArithmeticsStore.Instance.Multiply(left, right);
        }

        /// <summary>
        /// Calls <see cref="ArithmeticsStore.Divide" /> and returns the result.
        /// The concrete type combination must be registered in the <see cref="ArithmeticsStore"/> for this function to succeed.
        /// </summary>
        /// <param name="dividend">QuantityBase. Must be registered in the active ArithmeticsStore.</param>
        /// <param name="divisor">QuantityBase. Must be registered in the active ArithmeticsStore.</param>
        /// <returns></returns>
        /// <seealso cref="ArithmeticsStore.Divide"/>
        public static QuantityBase operator /(QuantityBase dividend, QuantityBase divisor)
        {
            VerifySameUnitsIfSameDimension(dividend, divisor);
            return ArithmeticsStore.Instance.Divide(dividend, divisor);
        }

        /// <summary>
        /// Calls <see cref="ArithmeticsStore.Add" /> and returns the result.
        /// The concrete type must be registered in the <see cref="ArithmeticsStore"/> for this function to succeed.
        /// </summary>
        /// <param name="left">QuantityBase. Must be registered in the active ArithmeticsStore. Must be the same type as right.</param>
        /// <param name="right">QuantityBase. Must be registered in the active ArithmeticsStore. Must be the same type as left.</param>
        /// <returns></returns>
        /// <seealso cref="ArithmeticsStore.Add"/>
        public static QuantityBase operator +(QuantityBase left, QuantityBase right)
        {
            VerifySameDimension(left, right);
            return ArithmeticsStore.Instance.Add(left, right);
        }

        /// <summary>
        /// Calls <see cref="ArithmeticsStore.Subtract" /> and returns the result.
        /// The concrete type must be registered in the <see cref="ArithmeticsStore"/> for this function to succeed.
        /// </summary>
        /// <param name="left">QuantityBase. Must be registered in the active ArithmeticsStore. Must be the same type as right.</param>
        /// <param name="right">QuantityBase. Must be registered in the active ArithmeticsStore. Must be the same type as left.</param>
        /// <returns></returns>
        /// <seealso cref="ArithmeticsStore.Subtract"/>
        public static QuantityBase operator -(QuantityBase left, QuantityBase right)
        {
            VerifySameDimension(left, right);
            return ArithmeticsStore.Instance.Subtract(left, right);
        }

        private static void VerifySameDimension(QuantityBase x, QuantityBase y)
        {
            if (!Equals(x.Unit, y.Unit))
                throw new InvalidOperationException("Quantitites must have same units");
        }

        private static void VerifySameUnitsIfSameDimension(QuantityBase x, QuantityBase y)
        {
            if (Equals(x.Dimension, y.Dimension) &&
                !Equals(x.Unit, y.Unit))
                throw new NotSupportedException("Performing arithmetic on two quantities that has the same dimension but different units are not supported - yet.");
        }
    }
}