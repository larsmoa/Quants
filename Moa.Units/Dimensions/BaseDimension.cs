namespace Moa.Units.Dimensions
{
    /// <summary>
    /// Represents a base dimension (as opposition to a composite dimension). Such
    /// dimensions can be "Length" or "Time", but usually not "Area" with is a
    /// composite "Length*Length";
    /// </summary>
    public class BaseDimension: AbstractDimension
    {
        private readonly string _symbol;
        private readonly string _name;

        /// <summary>
        /// Creates a new BaseDimensions. BaseDimensions describe elementary 
        /// dimensions, e.g. Length and Mass. Other dimensions may be composite
        /// dimensions such as Area (Length * Length).
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="name"></param>
        public BaseDimension(string symbol, string name)
        {
            _symbol = symbol;
            _name = name;
        }

        public override string Symbol
        {
            get { return _symbol; }
        }

        public override string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Base units are only equal to themself.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(IDimension other)
        {
            return ReferenceEquals(this, other);
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Symbol, Name);
        }
    }
}
