namespace Quants
{
    /// <summary>
    /// Base interface for "compound" classes. This is used for compound units and dimensions.
    /// </summary>
    /// <typeparam name="T">The base type of the compound, e.g. IUnit.</typeparam>
    /// <typeparam name="TSelf">The class implementing the compound, e.g. CompoundUnit.</typeparam>
    public interface ICompound<T, out TSelf>
    {
        /*
        /// <summary>
        /// Returns a deep clone.
        /// </summary>
        /// <returns></returns>
        TSelf Clone();
        /// <summary>
        /// Returns this comound multiplied by the given factor.
        /// </summary>
        /// <param name="factor"></param>
        /// <returns></returns>
        T Multiply(T factor);
        /// <summary>
        /// Returns this compound divided by the given divisor.
        /// </summary>
        /// <param name="divisor"></param>
        /// <returns></returns>
        T Divide(T divisor);
        */
    }
}
