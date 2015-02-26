using Moa.Units.Conversion;
using Moa.Units.Systems;

namespace Moa.Units.Catalogs.SI
{
    /// <summary>
    /// Factory that creates UnitSystem instances that contains definitions for 
    /// the SI unit system. The created system also contains other convinient 
    /// dimension and unit definitions.
    /// </summary>
    /// <remarks>
    /// Dimensions used are defined in <see cref="StandardDimensionsCatalog" />
    /// and units are defined in <see cref="StandardUnitsCatalog" />. This class
    /// defines the relations between the different units.
    /// 
    /// Below follows a list of supported dimensions and units (the first mentioned
    /// unit in each dimension is the base unit as defined by the SI system).
    /// <para>
    /// Mass (<see cref="StandardDimensionsCatalog.Mass"/>)
    /// <list type="bullet">
    /// <item><term>Kilogram</term><description>StandardUnitsCatalog.Kilogram</description></item>
    /// <item><term>Gram</term><description>StandardUnitsCatalog.Gram</description></item>
    /// <item><term>Tonne</term><description>StandardUnitsCatalog.Tonne</description></item>
    /// </list>
    /// Length (<see cref="StandardDimensionsCatalog.Length"/>)
    /// <list type="bullet">
    /// <item><term>Meter</term><description>StandardUnitsCatalog.Meter</description></item>
    /// <item><term>Centimeter</term><description>StandardUnitsCatalog.Centimeter</description></item>
    /// <item><term>Kilometer</term><description>StandardUnitsCatalog.Kilometer</description></item>
    /// </list>
    /// Time (<see cref="StandardDimensionsCatalog.Time"/>)
    /// <list type="bullet">
    /// <item><term>Second</term><description>StandardUnitsCatalog.Second</description></item>
    /// <item><term>Minute</term><description>StandardUnitsCatalog.Minute</description></item>
    /// <item><term>Hour</term><description>StandardUnitsCatalog.Hour</description></item>
    /// </list>
    /// Electric current (<see cref="StandardDimensionsCatalog.ElectricCurrent"/>)
    /// <list type="bullet">
    /// <item><term>Amper</term><description>StandardUnitsCatalog.Amper</description></item>
    /// </list>
    /// Temperature (<see cref="StandardDimensionsCatalog.Temperature"/>)
    /// <list type="bullet">
    /// <item><term>Kelvin</term><description>StandardUnitsCatalog.Kelvin</description></item>
    /// <item><term>Celcius</term><description>StandardUnitsCatalog.Celcius</description></item>
    /// <item><term>Fahrenheit</term><description>StandardUnitsCatalog.Fahrenheit</description></item>
    /// </list>
    /// Luminous intensity (<see cref="StandardDimensionsCatalog.LuminousIntensity"/>)
    /// <list type="bullet">
    /// <item><term>Candela</term><description>StandardUnitsCatalog.Candela</description></item>
    /// </list>
    /// Amount of substance (<see cref="StandardDimensionsCatalog.AmountOfSubstance"/>)
    /// <list type="bullet">
    /// <item><term>Mole</term><description>StandardUnitsCatalog.Mole</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// In addition to the SI dimensions the factory also adds a new derived dimensions/units:
    /// Area (<see cref="StandardDimensionsCatalog.Area"/>)
    /// <list type="bullet">
    /// <item><term>SquareMeter</term><description>StandardUnitsCatalog.SquareMeter</description></item>
    /// <item><term>SquareKilometer</term><description>StandardUnitsCatalog.SquareKilometer</description></item>
    /// <item><term>SquareCentimeter</term><description>StandardUnitsCatalog.SquareCentimeter</description></item>
    /// </list>
    /// Volume (<see cref="StandardDimensionsCatalog.Area"/>)
    /// <list type="bullet">
    /// <item><term>Liter</term><description>StandardUnitsCatalog.Liter</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    public class SiUnitSystemFactory: IUnitSystemFactory
    {
        public IUnitSystem Create()
        {
            UnitSystem si = new UnitSystem();
            AddMass(si);
            AddLength(si);
            AddTime(si);
            AddElectricCurrent(si);
            AddTemperature(si);
            AddLuminousIntensity(si);
            AddAmountOfSubstance(si);
            // Derived units
            AddArea(si);
            AddVolume(si);
            AddPressure(si);
            AddSpeed(si);
            AddForce(si);
            AddEnergy(si);
            return si;
        }

        #region Base units

        private static void AddMass(UnitSystem si)
        {
            si.AddDimension(StandardDimensionsCatalog.Mass);
            si.AddBaseUnit(StandardUnitsCatalog.Kilogram);
            AddScaledUnit(si, StandardUnitsCatalog.Kilogram, StandardUnitsCatalog.Gram, 1000.0);
            AddScaledUnit(si, StandardUnitsCatalog.Kilogram, StandardUnitsCatalog.Tonne, 1.0 / 1000.0);
        }

        private static void AddLength(UnitSystem si)
        {
            si.AddDimension(StandardDimensionsCatalog.Length);
            si.AddBaseUnit(StandardUnitsCatalog.Meter);
            AddScaledUnit(si, StandardUnitsCatalog.Meter, StandardUnitsCatalog.Centimeter, 100.0);
            AddScaledUnit(si, StandardUnitsCatalog.Meter, StandardUnitsCatalog.Desimeter, 10.0);
            AddScaledUnit(si, StandardUnitsCatalog.Meter, StandardUnitsCatalog.Kilometer, 1.0 / 1000.0);
        }

        private static void AddTime(UnitSystem si)
        {
            si.AddDimension(StandardDimensionsCatalog.Time);
            si.AddBaseUnit(StandardUnitsCatalog.Second);
            AddScaledUnit(si, StandardUnitsCatalog.Second, StandardUnitsCatalog.Minute, 1.0/60.0);
            AddScaledUnit(si, StandardUnitsCatalog.Minute, StandardUnitsCatalog.Hour, 1.0/60.0);
            AddScaledUnit(si, StandardUnitsCatalog.Hour, StandardUnitsCatalog.Hour, 1.0/24.0);
        }

        private static void AddElectricCurrent(UnitSystem si)
        {
            si.AddDimension(StandardDimensionsCatalog.ElectricCurrent);
            si.AddBaseUnit(StandardUnitsCatalog.Amper);
        }

        private static void AddTemperature(UnitSystem si)
        {
            si.AddDimension(StandardDimensionsCatalog.Temperature);
            si.AddBaseUnit(StandardUnitsCatalog.Kelvin);
            AddScaledUnit(si, StandardUnitsCatalog.Kelvin, StandardUnitsCatalog.Celcius, 1.0, -273.15);
            AddScaledUnit(si, StandardUnitsCatalog.Celcius, StandardUnitsCatalog.Fahrenheit, 9.0 / 5.0, 32.0);
        }

        private static void AddLuminousIntensity(UnitSystem si)
        {
            si.AddDimension(StandardDimensionsCatalog.LuminousIntensity);
            si.AddBaseUnit(StandardUnitsCatalog.Candela);
        }
            
        private static void AddAmountOfSubstance(UnitSystem si)
        {
            si.AddDimension(StandardDimensionsCatalog.AmountOfSubstance);
            si.AddBaseUnit(StandardUnitsCatalog.Mole);
        }

        #endregion

        #region Derived units

        private static void AddArea(UnitSystem si)
        {
            si.AddDimension(StandardDimensionsCatalog.Area);
            si.AddBaseUnit(StandardUnitsCatalog.SquareMeter);
            AddScaledUnit(si, StandardUnitsCatalog.SquareMeter, StandardUnitsCatalog.SquareKilometer, 1.0 / (1000.0 * 1000.0));
            AddScaledUnit(si, StandardUnitsCatalog.SquareMeter, StandardUnitsCatalog.SquareCentimeter, 100.0*100.0);
        }

        private static void AddVolume(UnitSystem si)
        {
            si.AddDimension(StandardDimensionsCatalog.Volume);
            si.AddBaseUnit(StandardUnitsCatalog.CubicMeter);
            AddScaledUnit(si, StandardUnitsCatalog.CubicMeter, StandardUnitsCatalog.Liter, 1000.0);
        }

        private static void AddPressure(UnitSystem si)
        {
            si.AddDimension(StandardDimensionsCatalog.Pressure);
            si.AddBaseUnit(StandardUnitsCatalog.Pascal);
        }

        private static void AddSpeed(UnitSystem si)
        {
            si.AddDimension(StandardDimensionsCatalog.Speed);
            si.AddBaseUnit(StandardUnitsCatalog.MetersPerSecond);
            AddScaledUnit(si, StandardUnitsCatalog.MetersPerSecond, StandardUnitsCatalog.KilometersPerHour, 1.0 / 3.6);
        }

        private static void AddForce(UnitSystem si)
        {
            si.AddDimension(StandardDimensionsCatalog.Force);
            si.AddBaseUnit(StandardUnitsCatalog.Newton);
        }

        private static void AddEnergy(UnitSystem si)
        {
            si.AddDimension(StandardDimensionsCatalog.Energy);
            si.AddBaseUnit(StandardUnitsCatalog.Joule);
        }

        #endregion

        private static void AddScaledUnit(UnitSystem si, IUnit source, IUnit target, double scale, double offset = 0.0)
        {
            si.AddScaledUnit(source, target, new ScaledConverter(scale, offset, source, target));
        }
    }
}
