namespace Moa.Units.Units
{
    /// <summary>
    /// Class that represents a base unit, e.g. "m". Such units may be building blocks of other, more complex 
    /// units (such as "m/s"). This class is immutable.
    /// </summary>
    public sealed class BaseUnit: AbstractUnit
    {
        private readonly string _unit;
        private readonly string _description;
        private readonly IDimension _dimension;

        /// <summary>
        /// Creates a new base unit. Base units are elementary and describes how
        /// a quantity is stored.
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="description"></param>
        /// <param name="dimension"></param>
        public BaseUnit(string unit, string description, IDimension dimension)
        {
            _unit = unit;
            _description = description;
            _dimension = dimension;
        }

        public override string Unit { get { return _unit; } }
        public override string Description { get { return _description; } }
        public override IDimension Dimension { get { return _dimension; } }

        public override bool IsEquivialent(IUnit unit)
        {
            return ReferenceEquals(this, unit);
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Unit, Description);
        }
    }
}
