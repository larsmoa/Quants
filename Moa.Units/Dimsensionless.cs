namespace Quants
{
    /// <summary>
    /// Special measurement for dimensionless quantities, or scalars.
    /// </summary>
    public class Dimsensionless: AbstractDimension
    {
        private static IDimension _instance;

        /// <summary>
        /// Returns the application wide dimensionless.
        /// </summary>
        public static IDimension Instance
        {
            get 
            { 
                if (_instance == null)
                {
                    _instance = new Dimsensionless();
                }
                return _instance;
            }
        }

        private Dimsensionless()
        {
        }

        public override string Symbol
        {
            get { return ""; }
        }

        public override string Name
        {
            get { return ""; }
        }

        public override bool Equals(IDimension other)
        {
            return ReferenceEquals(other, this);
        }
    }
}
