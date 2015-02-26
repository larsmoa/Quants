using System.Collections.Generic;
using System.Linq;
using Moa.Units.Dimensions;

namespace Moa.Units.Units
{
    /// <summary>
    /// Unit that consists of zero or more dividends and zero or more divisors, e.g.
    /// (a*b) / (c*d). All operations are immutable.
    /// 
    /// This class should be considered internal, use UnitCombiner.
    /// <see cref="UnitCombiner"/>
    /// </summary>
    public class CompoundUnit: AbstractCompound<IUnit, CompoundUnit>, IUnit
    {
        /// <summary>
        /// Creates a new compound unit that contains the unitless unit.
        /// </summary>
        public CompoundUnit()
            : base(Unitless.Instance)
        {
        }

        /// <summary>
        /// Creates a new compound unit that contains the factor given.
        /// </summary>
        /// <param name="dividend"></param>
        public CompoundUnit(IUnit dividend)
            : base(Unitless.Instance, dividend)
        {
        }

        /// <summary>
        /// Crates a new compound unit that consists of a division.
        /// </summary>
        /// <param name="dividend">a in a/b</param>
        /// <param name="divisor">b in a/b</param>
        public CompoundUnit(IUnit dividend, IUnit divisor)
            : base(Unitless.Instance, dividend, divisor)
        {
        }

        private CompoundUnit(IDictionary<IUnit, int> factors)
            : base(Unitless.Instance, factors)
        {            
        }

        protected override CompoundUnit Clone()
        {
            return new CompoundUnit(Factors);
        }

        protected override CompoundUnit CreateFromFactors(IDictionary<IUnit, int> factors)
        {
            return new CompoundUnit(factors);
        }

        public string Unit
        {
            get { return GenerateString(x => x.Unit); }
        }

        public string Description
        {
            get { return GenerateString(x => x.Description); }
        }

        public IDimension Dimension
        {
            get
            {
                IDictionary<IDimension, int> dimensionFactors = new Dictionary<IDimension, int>(Factors.Count);
                foreach (KeyValuePair<IUnit, int> factor in Factors)
                {
                    IDimension dim = factor.Key.Dimension;
                    int power = 0;
                    dimensionFactors.TryGetValue(dim, out power);
                    power += factor.Value;
                    dimensionFactors[dim] = power;
                }
                // Remove canceled factors
                dimensionFactors = dimensionFactors.Where(x => (x.Value != 0)).ToDictionary(x => x.Key, x => x.Value);

                CompoundDimension dimension = new CompoundDimension(dimensionFactors);
                return dimension.Simplify();
            }
        }

        public bool IsEquivialent(IUnit unit)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return Unit;
        }
    }
}
