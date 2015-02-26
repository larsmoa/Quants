using Moa.Units.Units;

namespace Moa.Units
{
    /// <summary>
    /// Class for combining two or more units into a new unit, e.g. from multiplication.
    /// Always use this class to do combine units. You should always referer to units
    /// as IUnit and do not use there "known" type. This is because there is no way of
    /// determining what the result of an unit combination will be.
    /// 
    /// The combiner is "smart" and will return the smallest representation possible, e.g.
    /// will "(a*b)/(a*b)" return the unitless unit and expressions such as "((a*b)/c)*c" results
    /// in "a*b".
    /// </summary>
    public static class UnitCombiner
    {
        /// <summary>
        /// Multiply two units to combine a new unit. The order of
        /// the arguments are arbitrary.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static IUnit Multiply(IUnit left, IUnit right)
        {
            if (left == Unitless.Instance && right == Unitless.Instance)
            {
                return Unitless.Instance;
            }
            if (left == Unitless.Instance)
            {
                return right;
            }
            if (right == Unitless.Instance)
            {
                return left;
            }
            if (left is CompoundUnit)
            {
                return ((CompoundUnit) left).Multiply(right);
            }
            if (right is CompoundUnit)
            {
                return ((CompoundUnit) right).Multiply(left);
            }
            CompoundUnit unit = new CompoundUnit(left);
            return unit.Multiply(right);
        }

        /// <summary>
        /// Multiplies three or more units to form one. The order of the
        /// factors are arbitrary.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="factors"></param>
        /// <returns></returns>
        public static IUnit Multiply(IUnit left, IUnit right, params IUnit[] factors)
        {
            IUnit unit = Multiply(left, right);
            foreach (IUnit factor in factors)
            {
                unit = Multiply(unit, factor);
            }
            return unit;
        }

        /// <summary>
        /// Divides the first unit with the second to form a new unit.
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public static IUnit Divide(IUnit dividend, IUnit divisor)
        {
            if (ReferenceEquals(dividend, divisor))
            {
                return Unitless.Instance;
            }
            if (dividend == Unitless.Instance && divisor == Unitless.Instance)
            {
                return Unitless.Instance;
            }
            if (divisor == Unitless.Instance)
            {
                return dividend;
            }
            if (dividend is CompoundUnit)
            {
                return ((CompoundUnit) dividend).Divide(divisor);
            }
            CompoundUnit unit = new CompoundUnit(dividend);
            return unit.Divide(divisor);
        }
    }
}
