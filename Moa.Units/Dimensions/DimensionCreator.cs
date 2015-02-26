namespace Moa.Units.Dimensions
{
    /// <summary>
    /// Class that enables creating combined dimensions in a fluent manner.
    /// <example>
    /// <code>
    /// // Mass*Length / (Time*Time).
    /// IDimension force = new DimensionCreator(Mass).Multiply(Length).Divide(Time).Divide(Time).Create().
    /// </code>
    /// </example>
    /// </summary>
    public class DimensionCreator
    {
        private CompoundDimension _dimension;

        /// <summary>
        /// Initialize the creator with the dimension given.
        /// </summary>
        /// <param name="baseDimension"></param>
        public DimensionCreator(IDimension baseDimension)
        {
            UpdateDimension(baseDimension);
        }

        /// <summary>
        /// Initialize the creator to be dimensionless.
        /// </summary>
        public DimensionCreator()
        {
            UpdateDimension(new CompoundDimension(Dimsensionless.Instance));
        }

        /// <summary>
        /// Multiply the current dimension with the dimension(s) given.
        /// </summary>
        /// <param name="dimensions">One or more dimensions.</param>
        public DimensionCreator Multiply(params IDimension[] dimensions)
        {
            UpdateDimension(_dimension.Multiply(dimensions));
            return this;
        }

        /// <summary>
        /// Divide the current dimension with the dimension(s) given.
        /// </summary>
        /// <param name="dimensions">One or more dimensions.</param>
        public DimensionCreator Divide(params IDimension[] dimensions)
        {
            UpdateDimension(_dimension.Divide(dimensions));
            return this;
        }

        /// <summary>
        /// Determine the resulting dimension and return.
        /// </summary>
        /// <returns></returns>
        public IDimension Create()
        {
            return _dimension.Multiply(Dimsensionless.Instance);
        }

        private void UpdateDimension(IDimension dimension)
        {
            if (dimension is CompoundDimension)
            {
                _dimension = (CompoundDimension) dimension;
            }
            else
            {
                _dimension = new CompoundDimension(dimension);
            }
        }
    }
}
