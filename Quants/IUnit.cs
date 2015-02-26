using System;

namespace Quants
{
    /// <summary>
    /// Represents an immutable unit with a short and long textual representation. In addition
    /// each unit belongs to an IDimension. E.g. could a "kg"-unit be part of the "M"-dimension (Mass).
    /// 
    /// This library has been inspired by Boost.Units and uses some of the same concepts.
    /// </summary>
    public interface IUnit: IEquatable<IUnit>
    {
        /// <summary>
        /// Returns the unit, e.g. "m".
        /// </summary>
        string Unit { get; }
        /// <summary>
        /// Returns the long unit description, e.g. "meter".
        /// </summary>
        string Description { get; }
        /// <summary>
        /// Returns the dimension this unit is for, e.g. Length.
        /// </summary>
        IDimension Dimension { get; }

        /// <summary>
        /// Returns true if the two units are equivialent (quantities can be added/subtracted with
        /// conversions, e.g. Celsius to Kelvin, or the units are equal). This returns true for
        /// units that have the same measurement.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        bool IsEquivialent(IUnit unit);
    }
}
