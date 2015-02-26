using System;

namespace Moa.Units
{
    /// <summary>
    /// Represents a physical quantity, e.g. "Volume" or "Length". A dimensions
    /// describes what a quantity is, while it's unit describes how it is measured.
    /// For each dimension there are typically many different units, e.g. may "Volume"
    /// be represented in "litres" or "barrels".
    /// </summary>
    public interface IDimension: IEquatable<IDimension>
    {
        /// <summary>
        /// The symbol of the measurement, e.g. "V".
        /// </summary>
        string Symbol { get; }
        /// <summary>
        /// The name of the dimension, e.g. "Volume".
        /// </summary>
        string Name { get; }
    }
}
