using System.Collections.Generic;

namespace Quants
{
    /// <summary>
    /// Represents a unit system with dimensions (IDimension) and at least one unit (IUnit) per measurement.
    /// One unit is specified to be the base unit for each measurement, e.g. measurement "Mass" may have
    /// units base unit "g" and alternative units "kg" and "lbs".
    /// </summary>
    public interface IUnitSystem
    {
        /// <summary>
        /// Returns the dimensions supported by the unit system.
        /// </summary>
        IEnumerable<IDimension> Dimensions { get; }

        /// <summary>
        /// Returns true if the system supports the dimension given.
        /// </summary>
        /// <param name="dimension"></param>
        /// <returns></returns>
        bool SupportsDimension(IDimension dimension);

        /// <summary>
        /// Returns the base unit of the dimension given. Returns null if not found.
        /// </summary>
        /// <param name="dimension"></param>
        /// <returns></returns>
        IUnit GetBaseUnit(IDimension dimension);

        /// <summary>
        /// Returns all units supported by the given dimension. The order of the 
        /// returned units are arbitrary.
        /// </summary>
        /// <param name="dimension"></param>
        /// <returns>All units supported by the given dimension.</returns>
        IEnumerable<IUnit> GetSupportedUnits(IDimension dimension);

        /// <summary>
        /// Retruns true if the system supports the unit given.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        bool SupportsUnit(IUnit unit);

        /// <summary>
        /// Returns true if the unit system is able to convert between a source- and target-unit.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        bool CanConvert(IUnit source, IUnit target);

        /// <summary>
        /// Creates a converter that converts from the given unit to the target unit.
        /// Throws NotSupportedException() if CanConvert(source, target) returns false.
        /// </summary>
        /// <param name="source">Unit that the converter will convert from.</param>
        /// <param name="target">Unit that the converter will convert to.</param>
        /// <returns></returns>
        IValueConverter CreateConverter(IUnit source, IUnit target);
    }
}
