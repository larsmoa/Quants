using System;

namespace Quants.Quantities
{
    /// <summary>
    /// Immutable quantity (value with unit).
    /// </summary>
    /// <typeparam name="T">Value type, e.g. double or Matrix.</typeparam>
    public sealed class Quantity<T>: QuantityBase
    {
        private readonly IUnit _unit;
        private readonly T _value;

        /// <summary>
        /// Creates a new quantity.
        /// </summary>
        /// <param name="value">The value of the quantity.</param>
        /// <param name="unit">The unit of the quantity. Cannot be null.</param>
        public Quantity(T value, IUnit unit)
        {
            _unit = unit;
            _value = value;
        }

        /// <summary>
        /// Returns the quantity value. Accessing the value through this function is fast.
        /// </summary>
        public T Value
        {
            get { return _value; }
        }

        public override Type QuantityValueType
        {
            get { return typeof (T); }
}

        public override object QuantityValue
        {
            get { return _value; }
        }

        public override IUnit Unit
        {
            get { return _unit; }
        }

        public override IDimension Dimension
        {
            get { return _unit.Dimension; }
        }        
    }
}
