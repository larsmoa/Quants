using Moa.Units.Dimensions;

namespace Moa.Units.Catalogs
{
    /// <summary>
    /// Contains definitions of elementary dimensions. The list is from
    /// http://en.wikipedia.org/wiki/International_System_of_Quantities and derived
    /// dimensions are from http://en.wikipedia.org/wiki/SI_derived_unit (partial support).
    /// </summary>
    public static class StandardDimensionsCatalog
    {
        /// <summary>
        /// Dimension describing mass (e.g. kilogram).
        /// </summary>
        public static readonly IDimension Mass;

        /// <summary>
        /// Dimension describing length (e.g. metre).
        /// </summary>
        public static readonly IDimension Length;

        /// <summary>
        /// Dimension describing time (e.g. seconds).
        /// </summary>
        public static readonly IDimension Time;

        /// <summary>
        /// Dimension describing elecric current (e.g. ampere).
        /// </summary>
        public static readonly IDimension ElectricCurrent;

        /// <summary>
        /// Dimension describing thermodynamic temperatur (e.g. kelvin).
        /// </summary>
        public static readonly IDimension Temperature;

        /// <summary>
        /// Dimension describing luminous instansity (e.g candela).
        /// </summary>
        public static readonly IDimension LuminousIntensity;

        /// <summary>
        /// Dimension describing an amount of elementary substance (e.g mole).
        /// </summary>
        public static readonly IDimension AmountOfSubstance;

        /// <summary>
        /// Dimension describing an area (e.g. square metre).
        /// </summary>
        public static readonly IDimension Area;

        /// <summary>
        /// Dimension describing a volume (e.g. cubed metre).
        /// </summary>
        public static readonly IDimension Volume;

        /// <summary>
        /// Dimension describing speed (e.g. metre per second).
        /// </summary>
        public static readonly IDimension Speed;

        /// <summary>
        /// Dimension describing force or weight (e.g. newton).
        /// </summary>
        public static readonly IDimension Force;

        /// <summary>
        /// Dimension describing pressure or stress (e.g. newton per square metre).
        /// </summary>
        public static readonly IDimension Pressure;

        /// <summary>
        /// Dimension describing energy or work (e.g. joule).
        /// </summary>
        public static readonly IDimension Energy;

        static StandardDimensionsCatalog()
        {
            // Base dimensions
            Mass = new BaseDimension("M", "mass");
            Length = new BaseDimension("L", "length");
            Time = new BaseDimension("T", "time");
            ElectricCurrent = new BaseDimension("I", "current");
            Temperature = new BaseDimension("T", "temperature");
            LuminousIntensity = new BaseDimension("J", "luminous intensity");
            AmountOfSubstance = new BaseDimension("N", "amout of substance");

            // Derived dimensions
            // FIXME 20120211 lmo: The dimensions are correct, but should also include a better symbol/description.
            // Refactor DimensionCreator and CompoundDimension to support this.
            Area = new DimensionCreator().Multiply(Length, Length).Create();
            Volume = new DimensionCreator().Multiply(Area, Length).Create();
            Speed = new DimensionCreator().Multiply(Length).Divide(Time).Create();
            Force = new DimensionCreator().Multiply(Length, Mass).Divide(Time, Time).Create();
            Pressure = new DimensionCreator().Multiply(Force).Divide(Area).Create();
            Energy = new DimensionCreator().Multiply(Force, Length).Create();
        }
    }
}
