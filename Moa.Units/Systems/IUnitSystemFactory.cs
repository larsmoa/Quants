namespace Moa.Units.Systems
{
    /// <summary>
    /// Interface for factories that create unit system instances.
    /// Implementations will typically create an UnitSystem instance
    /// and populate it with units from e.g. the SI-system.
    /// </summary>
    public interface IUnitSystemFactory
    {
        /// <summary>
        /// Creates the unit system.
        /// </summary>
        /// <returns></returns>
        IUnitSystem Create();
    }
}
