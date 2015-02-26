namespace Moa.Units
{
    /// <summary>
    /// Common base class for (most) IDimensions. Provides default
    /// implementations of Equals() and GetHashCode().
    /// </summary>
    public abstract class AbstractDimension: IDimension
    {
        /// <summary>
        /// The symbol of the dimension, e.g. "V".
        /// </summary>
        public abstract string Symbol { get; }
        /// <summary>
        /// The name of the dimension, e.g. "Volume".
        /// </summary>
        public abstract string Name { get; }

        public abstract bool Equals(IDimension other);

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return Equals((IDimension) obj);            
        }

        public override int GetHashCode()
        {
            return Symbol.GetHashCode();
        }
    }
}
