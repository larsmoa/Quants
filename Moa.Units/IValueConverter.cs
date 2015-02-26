namespace Quants
{
    /// <summary>
    /// Interface for unit conversion from a source unit, to a target unit.
    /// </summary>
    public interface IValueConverter
    {
        /// <summary>
        /// Target unit. Converter converts values to this unit.
        /// </summary>
        IUnit Target { get; }

        /// <summary>
        /// Source unit. Convert convert values from this unit.
        /// </summary>
        IUnit Source { get; }

        /// <summary>
        /// Converts a value to the target unit. The value
        /// unit is assumed to be the source unit.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        double Convert(double value);

        /// <summary>
        /// Converts a value to the target unit. The value
        /// unit is assumed to be the source unit.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        float Convert(float value);

        /// <summary>
        /// Returns an IValueConverter that converts from the target-unit to the source-unit.
        /// </summary>
        /// <returns></returns>
        IValueConverter Inversed();
    }
}
