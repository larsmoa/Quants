using System;
using Moa.Units;
using Moa.Units.Catalogs;
using Moa.Units.Catalogs.SI;
using Moa.Units.Quantities;

namespace BasicExample
{
    class Program
    {
        private readonly IUnitSystem _siSystem;

        public Program()
        {
            SiUnitSystemFactory siFactory = new SiUnitSystemFactory();
            _siSystem = siFactory.Create();
            StandardQuantitiesCatalog.Setup();
        }

        public void Run()
        {
            MultiplyLengthToFormVolume();
            ConvertTemperatures();
            CalculateDensityOfHumanBody();
            CalculatePressureUnderWater();
            DivideQuantitiesWithSameUnits();
        }

        private void MultiplyLengthToFormVolume()
        {
            // Combine Area and Length to form Volume
            var width = new Quantity<double>(1.0f, StandardUnitsCatalog.Desimeter);
            var height = width;
            var depth = width;
            var volume = width*height*width;

            Console.WriteLine("--- Multiply three lengths to form a volume ---");
            Console.WriteLine("{0}*{1}*{2} = {3}", width, height, depth, volume);
            Console.WriteLine();
        }

        private void ConvertTemperatures()
        {
            var celcius = new Quantity<double>(20, StandardUnitsCatalog.Celcius);
            IValueConverter toKelvin = _siSystem.CreateConverter(celcius.Unit, StandardUnitsCatalog.Kelvin);
            IValueConverter toFahrenheit = _siSystem.CreateConverter(celcius.Unit, StandardUnitsCatalog.Fahrenheit);
            var kelvin = new Quantity<double>(toKelvin.Convert(celcius.Value), toKelvin.Target);
            var fahrenheit = new Quantity<double>(toFahrenheit.Convert(celcius.Value), toFahrenheit.Target);
            IValueConverter fromFtoK = _siSystem.CreateConverter(fahrenheit.Unit, StandardUnitsCatalog.Kelvin);
            var kelvinFromFahrenheit = new Quantity<double>(fromFtoK.Convert(fahrenheit.Value), toKelvin.Target);

            Console.WriteLine("--- Temperature conversion ---");
            Console.WriteLine("{0} is equivalent to {1}", celcius, fahrenheit);
            Console.WriteLine("{0} is equivalent to {1}", celcius, kelvin);
            Console.WriteLine("{0} is equivalent to {1}", fahrenheit, kelvinFromFahrenheit);
            Console.WriteLine();
        }

        private void CalculateDensityOfHumanBody()
        {
            var mass = new Quantity<double>(80.0, StandardUnitsCatalog.Kilogram);
            var volume = new Quantity<double>(0.07921, StandardUnitsCatalog.CubicMeter);
            var density = mass/volume;
            
            Console.WriteLine("--- Density of the human body ---");
            Console.WriteLine("{0} / {1} = {2}", mass, volume, density);
            Console.WriteLine();
        }

        private void CalculatePressureUnderWater()
        {
            var densityUnit = UnitCombiner.Divide(StandardUnitsCatalog.Kilogram, StandardUnitsCatalog.CubicMeter);
            var accelerationUnit = UnitCombiner.Divide(StandardUnitsCatalog.MetersPerSecond, StandardUnitsCatalog.Second);
            var depth = new Quantity<double>(30.0, StandardUnitsCatalog.Meter);
            var density = new Quantity<double>(1000.0, densityUnit);
            var gravity = new Quantity<double>(9.81, accelerationUnit);
            var pressure = depth*gravity*density;
            
            Console.WriteLine("--- Pressure under water ---");
            Console.WriteLine("At {0}: {1}", depth, pressure);
            Console.WriteLine();
        }

        private void DivideQuantitiesWithSameUnits()
        {
            var myPascal = UnitCombiner.Divide(StandardUnitsCatalog.Kilogram,
                UnitCombiner.Multiply(StandardUnitsCatalog.Second, UnitCombiner.Multiply(StandardUnitsCatalog.Second, StandardUnitsCatalog.Meter)));
            var dividend = new Quantity<double>(782.0, StandardUnitsCatalog.Pascal);
            var divisor = new Quantity<double>(100.0, myPascal);
            var result = dividend/divisor;

            Console.WriteLine("--- Divide pressure with pressure ---");
            Console.WriteLine("{0} / {1} = {2}", dividend, divisor, result);
            Console.WriteLine();
        }


        static void Main(string[] args)
        {
            Program p = new Program();
            p.Run();
        }
    }
}
