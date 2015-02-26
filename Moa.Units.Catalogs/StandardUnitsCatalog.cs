using Quants.Units;

namespace Quants.Catalogs
{
    /// <summary>
    /// Catalog of units for the dimensions in <see cref="StandardDimensionsCatalog" />. This class
    /// only defines a set of valid units and assosciate them with a dimension, it does not define
    /// or know how to convert between them. It also does not order the units in an <see cref="IUnitSystem"/>.
    /// </summary>
    /// Note that this class is not to be confused with the SI-unit catalog, it may contain units from
    /// other catalogs too.
    public static class StandardUnitsCatalog
    {
        /// <summary>
        /// Kilogram is a Mass.
        /// </summary>
        public static readonly IUnit Kilogram;
        /// <summary>
        /// Gram is a Mass.
        /// </summary>
        public static readonly IUnit Gram;
        /// <summary>
        /// Tonne is a Mass.
        /// </summary>
        public static readonly IUnit Tonne;

        /// <summary>
        /// Meter is a Length.
        /// </summary>
        public static readonly IUnit Meter;
        /// <summary>
        /// Desimeter is a Length.
        /// </summary>
        public static readonly IUnit Desimeter;
        /// <summary>
        /// Centimeter is a Length.
        /// </summary>
        public static readonly IUnit Centimeter;
        /// <summary>
        /// Kilometer is a Length.
        /// </summary>
        public static readonly IUnit Kilometer;

        /// <summary>
        /// Second is a Time.
        /// </summary>
        public static readonly IUnit Second;
        /// <summary>
        /// Minute is a Time.
        /// </summary>
        public static readonly IUnit Minute;
        /// <summary>
        /// Hour is a Time.
        /// </summary>
        public static readonly IUnit Hour;
        /// <summary>
        /// Day is a Time.
        /// </summary>
        public static readonly IUnit Day;

        /// <summary>
        /// Amper is an ElectricCurrent.
        /// </summary>
        public static readonly IUnit Amper;

        /// <summary>
        /// Kelvin is a Temperature.
        /// </summary>
        public static readonly IUnit Kelvin;
        /// <summary>
        /// Celcius is a Temperature.
        /// </summary>
        public static readonly IUnit Celcius;
        /// <summary>
        /// Fahrenheit is a Temperature.
        /// </summary>
        public static readonly IUnit Fahrenheit;

        /// <summary>
        /// Candela is a LuminousIntensity.
        /// </summary>
        public static readonly IUnit Candela;

        /// <summary>
        /// Mole is an AmountOfSubstance.
        /// </summary>
        public static readonly IUnit Mole;

        /// <summary>
        /// SquareMeter is an Area. Composite of Meter.
        /// </summary>
        public static readonly IUnit SquareMeter;
        /// <summary>
        /// SquareCentimeter is an Area. Composite of Centimeter.
        /// </summary>
        public static readonly IUnit SquareCentimeter;
        /// <summary>
        /// SquareKilometer is an Area. Composite of Kilometer.
        /// </summary>
        public static readonly IUnit SquareKilometer;

        /// <summary>
        /// CubicMeter is a Volume. Composite of Meter.
        /// </summary>
        public static readonly IUnit CubicMeter;

        /// <summary>
        /// Liter is a Volume. Composite of Desimeter.
        /// </summary>
        public static readonly IUnit Liter;

        /// <summary>
        /// MetersPerSecond is a Speed. Composite of Meter / Second.
        /// </summary>
        public static readonly IUnit MetersPerSecond;

        /// <summary>
        /// KilometersPerHour is a Speed. Composite of Kilometer / Hour.
        /// </summary>
        public static readonly IUnit KilometersPerHour;

        /// <summary>
        /// Newton is a Force. Composite of (Kilogram * Meter) / (Second * Second).
        /// </summary>
        public static readonly IUnit Newton;

        /// <summary>
        /// Joule is an Energy. Composite of Newton * Meter
        /// </summary>
        public static readonly IUnit Joule;

        /// <summary>
        /// Pascal is a Pressure. Composite of Newton / (Meter * Meter)
        /// </summary>
        public static readonly IUnit Pascal;

        static StandardUnitsCatalog()
        {
            // Base units:

            // Mass
            Kilogram = new BaseUnit("kg", "kilogram", StandardDimensionsCatalog.Mass);
            Gram = new BaseUnit("g", "gram", StandardDimensionsCatalog.Mass);
            Tonne = new BaseUnit("t", "tonne", StandardDimensionsCatalog.Mass);
            // Length
            Meter = new BaseUnit("m", "metre", StandardDimensionsCatalog.Length);
            Centimeter = new BaseUnit("cm", "centimetre", StandardDimensionsCatalog.Length);
            Desimeter = new BaseUnit("dm", "desimeter", StandardDimensionsCatalog.Length);   
            Kilometer = new BaseUnit("km", "kilometre", StandardDimensionsCatalog.Length);
            // Time
            Second = new BaseUnit("s", "second", StandardDimensionsCatalog.Time);
            Minute = new BaseUnit("min", "minute", StandardDimensionsCatalog.Time);
            Hour = new BaseUnit("h", "hour", StandardDimensionsCatalog.Time);
            Day = new BaseUnit("d", "day", StandardDimensionsCatalog.Time);
            // Electric current
            Amper = new BaseUnit("a", "amper", StandardDimensionsCatalog.ElectricCurrent);
            // Temperature
            Kelvin = new BaseUnit("K", "kelvin", StandardDimensionsCatalog.Temperature);
            Celcius = new BaseUnit("°C", "celcius", StandardDimensionsCatalog.Temperature);
            Fahrenheit = new BaseUnit("°F", "fahrenheit", StandardDimensionsCatalog.Temperature);
            // Luminous intensity
            Candela = new BaseUnit("cd", "candela", StandardDimensionsCatalog.LuminousIntensity);
            // Amount of substance
            Mole = new BaseUnit("mol", "mole", StandardDimensionsCatalog.AmountOfSubstance);
            
            // Derived units:
            // FIXME 20120211 lmo: The units are correct, but should also include a better symbol/description.
            // Refactor UnitCreator and CompoundUnit to support this.
           
            // Area
            SquareMeter = new UnitCreator().Multiply(Meter, Meter).Create();
            SquareKilometer = new UnitCreator().Multiply(Kilometer, Kilometer).Create();
            SquareCentimeter = new UnitCreator().Multiply(Centimeter, Centimeter).Create();

            // Volume
            CubicMeter = new UnitCreator().Multiply(Meter, Meter, Meter).Create();
            Liter = new UnitCreator().Multiply(Desimeter, Desimeter, Desimeter).Create();

            // Speed
            MetersPerSecond = new UnitCreator().Multiply(Meter).Divide(Second).Create();
            KilometersPerHour = new UnitCreator().Multiply(Kilometer).Divide(Hour).Create();

            // Force
            Newton = new UnitCreator().Multiply(Kilogram, Meter).Divide(Second, Second).Create();

            // Pressure
            Pascal = new UnitCreator().Multiply(Newton).Divide(Meter, Meter).Create();

            // Energy
            Joule = new UnitCreator().Multiply(Newton, Meter).Create();
        }
    }
}
