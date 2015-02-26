namespace Quants
{
    /// <summary>
    /// Special unit for describing the unit of dimensionless quantities.
    /// </summary>
    public class Unitless: IUnit
    {
        private static Unitless _instance;

        /// <summary>
        /// Returns the IUnit implementation containing the unitless unit. There is only
        /// one instance of these.
        /// </summary>
        public static IUnit Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Unitless();
                }
                return _instance;
            }
        }

        private Unitless()
        {
        }

        public string Unit {
            get { return ""; }
        }

        public string Description
        {
            get { return "dimensionless"; }
        }

        /// <summary>
        /// Returns DimensionlessMeasurement.Instance
        /// </summary>
        public IDimension Dimension
        {
            get { return Dimsensionless.Instance; }
        }

        /// <summary>
        /// Returns true if the provided unit also is the
        /// dimensionless unit.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool IsEquivialent(IUnit unit)
        {
            return (this == unit);
        }

        public bool Equals(IUnit other)
        {
            return ReferenceEquals(this, other);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(obj, this);
        }

        public override int GetHashCode()
        {
            return 82367727;
        }

    }
}
