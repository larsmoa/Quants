using System;
using System.Collections.Generic;

namespace Moa.Units.Systems
{
    /// <summary>
    /// Standard implementation of IUnitSystem. Supports conversion between units.
    /// </summary>
    public class UnitSystem: IUnitSystem, ICloneable
    {
        private readonly IDictionary<IDimension, DimensionContainer> _dimensions;
        private readonly IDictionary<KeyValuePair<IUnit, IUnit>, IValueConverter> _converters;

        /// <summary>
        /// Creates an empty unit system.
        /// </summary>
        public UnitSystem()
        {
            _dimensions = new Dictionary<IDimension, DimensionContainer>();
            _converters = new Dictionary<KeyValuePair<IUnit, IUnit>, IValueConverter>();
        }

        /// <summary>
        /// Creates a deep clone of the unit system. The clone will reference
        /// the same units and dimensions as the original.
        /// <see cref="CreateFrom" />.
        /// </summary>
        /// <returns>A cloned UnitSystem.</returns>
        public object Clone()
        {
            UnitSystem clone = new UnitSystem();
            foreach (var container in _dimensions)
            {
                clone._dimensions.Add(container.Key, (DimensionContainer)container.Value.Clone());
            }            
            foreach (var converter in _converters)
            {
                clone._converters.Add(converter);
            }
            return clone;
        }

        /// <summary>
        /// Creates an unit system from the given prototype. This can be useful when
        /// e.g. extending one of the standard unit systems with custom units.
        /// If the prototype is an UnitSystem the created unit system will be an
        /// identical copy of the unit system. If not, only units that are convertible
        /// to the base unit will be added.
        /// </summary>
        /// <param name="prototype"></param>
        /// <returns></returns>
        public static UnitSystem CreateFrom(IUnitSystem prototype)
        {
            if (prototype is UnitSystem)
            {
                return (UnitSystem) ((UnitSystem) prototype).Clone();
            }
            
            UnitSystem system = new UnitSystem();
            foreach (IDimension dimension in prototype.Dimensions)
            {
                system.AddDimension(dimension);

                // Register base unit
                IUnit baseUnit = prototype.GetBaseUnit(dimension);
                system.AddBaseUnit(baseUnit);

                // Register scaled units
                foreach (IUnit unit in prototype.GetSupportedUnits(dimension))
                {
                    if (!ReferenceEquals(unit, baseUnit) && prototype.CanConvert(baseUnit, unit))
                    {
                        system.AddScaledUnit(baseUnit, unit, prototype.CreateConverter(baseUnit, unit));
                    }
                }                
            }
            return system;
        }

        public IEnumerable<IDimension> Dimensions
        {
            get { return _dimensions.Keys; }
        }

        public bool SupportsDimension(IDimension dimension)
        {
            return _dimensions.ContainsKey(dimension);
        }

        /// <summary>
        /// Adds a dimension. A dimension is a physical dimension such as "Mass", "Length" or "Time", but
        /// does not say anything about the unit. The dimension defines what a quantity is describes, but
        /// not how it is stored.
        /// </summary>
        /// <param name="dimension"></param>
        public void AddDimension(IDimension dimension)
        {
            if (_dimensions.ContainsKey(dimension))
                throw new ArgumentException(string.Format("Dimension '{0}' allready added.", dimension));

            _dimensions[dimension] = new DimensionContainer(dimension, CreateAdjacentValueConverter);
        }


        /// <summary>
        /// Adds a base unit. The base unit is the most basic unit to store values in. E.g., in the SI system
        /// the base unit for "Mass" is "kg" (http://en.wikipedia.org/wiki/International_System_of_Units#Units_and_prefixes).
        /// There can only be one base unit per dimension. 
        /// </summary>
        /// <param name="unit">The base unit of a dimension in the unit system. Dimension is determined from IUnit.Dimension.</param>
        public void AddBaseUnit(IUnit unit)
        {
            if (!_dimensions.ContainsKey(unit.Dimension))
                throw new ArgumentException(string.Format("Dimension '{0}' not registered.", unit.Dimension));

            _dimensions[unit.Dimension].SetBaseUnit(unit);
        }

        public IUnit GetBaseUnit(IDimension dimension)
        {
            if (!_dimensions.ContainsKey(dimension))
                throw new ArgumentException(string.Format("Dimension '{0}' not registered.", dimension));

            return _dimensions[dimension].BaseUnit;
        }

        public IEnumerable<IUnit> GetSupportedUnits(IDimension dimension)
        {
            if (!_dimensions.ContainsKey(dimension))
                throw new ArgumentException(string.Format("Dimension '{0}' not registered.", dimension));

            return _dimensions[dimension].Units;
        }

        public bool SupportsUnit(IUnit unit)
        {
            return _dimensions[unit.Dimension].ContainsUnit(unit);
        }

        /// <summary>
        /// Adds a "scaled" unit. Scaled units are alternative represesentations of the quantities of the same dimensions.
        /// Examples of this is that Mass can be stored in quantities as kg, pounds, gram. etc. The base unit given does not
        /// need to be an unit added using AddBaseUnit(), it can also be a previously added scaled unit.
        /// </summary>
        /// <param name="sourceUnit">An allready registered unit. Not necessarily a base unit.</param>
        /// <param name="scaledUnit">The unit to add. Relates to the sourceUnit through the converter.</param>
        /// <param name="converter">IValueConverter that converts from sourceUnit to scaledUnit.</param>
        /// <example>
        /// IUnitSystem system;
        /// IDimension mass = new BaseDimension("M", "Mass");
        /// IUnit kilogram = new BaseUnit("kg", "kilogram", massDimension);
        /// IUnit pound = new BaseUnit("lb", "pound", massDimension);
        /// system.AddDimension(mass);
        /// system.AddBaseUnit(kilogram);
        /// system.AddScaledUnit(kilogram, pound, new ScaledConverter(kilogram, pound, 2.205, 0.0));
        /// </example>
        public void AddScaledUnit(IUnit sourceUnit, IUnit scaledUnit, IValueConverter converter)
        {
            if (!Equals(sourceUnit.Dimension, scaledUnit.Dimension))
                throw new ArgumentException(string.Format("Units '{0}' and '{1}' are not in the same dimension ('{2}' and '{3}').", sourceUnit, scaledUnit, sourceUnit.Dimension, scaledUnit.Dimension));
            if (_converters.ContainsKey(CreateConverterId(sourceUnit, scaledUnit)))
                throw new ArgumentException(string.Format("Relation between units '{0}' and '{1}' allready registered.", sourceUnit, scaledUnit));
            if (_converters.ContainsKey(CreateConverterId(scaledUnit, sourceUnit)))
                throw new ArgumentException(string.Format("Reversed relation between units '{0}' and '{1}' registered.", sourceUnit, scaledUnit));

            _converters[CreateConverterId(sourceUnit, scaledUnit)] = converter;
            _dimensions[sourceUnit.Dimension].AddUnit(sourceUnit, scaledUnit);
        }

        public bool CanConvert(IUnit source, IUnit target)
        {
            if (Equals(source.Dimension, target.Dimension))
            {
                return _dimensions[source.Dimension].CanConvert(source, target);
            }
            return false;
        }

        public IValueConverter CreateConverter(IUnit source, IUnit target)
        {
            if (!Equals(source.Dimension, target.Dimension))
                throw new ArgumentException(string.Format("Units '{0}' and '{1}' are not in the same dimension ('{2}' and '{3}').", source, target, source.Dimension, target.Dimension));
            return _dimensions[source.Dimension].CreateConverter(source, target);
        }

        private IValueConverter CreateAdjacentValueConverter(IUnit source, IUnit target)
        {
            KeyValuePair<IUnit, IUnit> id = CreateConverterId(source, target);
            if (_converters.ContainsKey(id))
            {
                return _converters[id];
            }
            if (_converters.ContainsKey(ReversedConverterId(id)))
            {
                IValueConverter inversedConverter = _converters[ReversedConverterId(id)];
                IValueConverter converter = inversedConverter.Inversed();
                // Store for later
                _converters[id] = converter;
                return converter;
            }
            throw new KeyNotFoundException(string.Format("No converter from '{0}' to '{1}' found.", source, target));
        }

        private static KeyValuePair<IUnit, IUnit> CreateConverterId(IUnit source, IUnit target)
        {
            return new KeyValuePair<IUnit, IUnit>(source, target);
        }

        private static KeyValuePair<IUnit, IUnit> ReversedConverterId(KeyValuePair<IUnit, IUnit> id)
        {
            return CreateConverterId(id.Value, id.Key);
        }
    }
}
