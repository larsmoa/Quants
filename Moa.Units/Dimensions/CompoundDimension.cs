using System.Collections.Generic;

namespace Moa.Units.Dimensions
{
    /// <summary>
    /// Dimension that consists of zero or more dividends and zero or more divisors, e.g.
    /// (a*b) / (c*d). All operations are immutable.
    /// 
    /// This class should be considered internal.
    /// <see cref="UnitCombiner"/>
    /// </summary>
    public class CompoundDimension: AbstractCompound<IDimension, CompoundDimension>, IDimension
    {
        /// <summary>
        /// Creates a new compound dimension that holds the dimensionless.
        /// </summary>
        public CompoundDimension()
            : base(Dimsensionless.Instance)
        {
        }

        /// <summary>
        /// Creates a new compound dimensions that holds a single factor.
        /// </summary>
        /// <param name="dividend"></param>
        public CompoundDimension(IDimension dividend)
            : base(Dimsensionless.Instance, dividend)
        {
        }

        /// <summary>
        /// Creates a new compound dimensions that holds a division.
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        public CompoundDimension(IDimension dividend, IDimension divisor)
            : base(Dimsensionless.Instance, dividend, divisor)
        {
        }

        /// <summary>
        /// Creates a new compound dimensions that holds a set of factors.
        /// </summary>
        /// <param name="factors">
        /// A dictionary of dimensions with factors. The result is
        /// factor0.Key^factor0.Value * factor1.Key^factor1.Value * ... * factorN.Key^factoryN.Value.
        /// </param>
        public CompoundDimension(IDictionary<IDimension, int> factors)
            : base(Dimsensionless.Instance, factors)
        {            
        }

        protected override CompoundDimension Clone()
        {
            return new CompoundDimension(Factors);
        }

        protected override CompoundDimension CreateFromFactors(IDictionary<IDimension, int> factors)
        {
            return new CompoundDimension(factors);
        }

        public override string ToString()
        {
            return Symbol;
        }

        #region IDimension Members

        public string Symbol
        {
            get { return GenerateString(x => x.Symbol); }
        }

        public string Name
        {
            get
            {
                return GenerateString(x => "\"" + x.Name + "\"");
            }
        }

         #endregion
    }
}
