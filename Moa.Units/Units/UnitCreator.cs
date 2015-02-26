namespace Quants.Units
{
    /// <summary>
    /// Class that enables creating combined units in a fluent manner.
    /// <example>
    /// <code>
    /// // kg*m/ (s*s).
    /// IUnit newton = new UnitCreator(kg).Multiply(m).Divide(s, s).Create().
    /// </code>
    /// </example>
    /// </summary>
    public class UnitCreator
    {
        private CompoundUnit _unit;

        /// <summary>
        /// Initialize the creator with the unit given.
        /// </summary>
        /// <param name="baseUnit"></param>
        public UnitCreator(IUnit baseUnit)
        {
            UpdateUnit(baseUnit);
        }

        /// <summary>
        /// Initialize the creator to be dimensionless.
        /// </summary>
        public UnitCreator()
        {
            UpdateUnit(new CompoundUnit(Unitless.Instance));
        }

        /// <summary>
        /// Multiply the current unit with the units(s) given.
        /// </summary>
        /// <param name="units">One or more units.</param>
        public UnitCreator Multiply(params IUnit[] units)
        {
            UpdateUnit(_unit.Multiply(units));
            return this;
        }

        /// <summary>
        /// Divide the current unit with the unit(s) given.
        /// </summary>
        /// <param name="units">One or more units.</param>
        public UnitCreator Divide(params IUnit[] units)
        {
            UpdateUnit(_unit.Divide(units));
            return this;
        }

        /// <summary>
        /// Determine the resulting unit and return.
        /// </summary>
        /// <returns></returns>
        public IUnit Create()
        {
            return _unit.Multiply(Unitless.Instance);
        }

        private void UpdateUnit(IUnit unit)
        {
            if (unit is CompoundUnit)
            {
                _unit = (CompoundUnit) unit;
            }
            else
            {
                _unit = new CompoundUnit(unit);
            }
        }
    }
}
