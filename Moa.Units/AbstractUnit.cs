namespace Moa.Units
{
    /// <summary>
    /// Abstract base class for IUnit that implements Equals(object) that uses
    /// Equals(IUnit) (implemented by subclasses) and GetHashCode() that
    /// uses the fields declared in IUnit. Provides and default implementation
    /// of Equals(IUnit) that checks if all fields are equal and that the
    /// types are identical. Also implements ToString() that returns the value 
    /// of Unit. 
    /// </summary>
    public abstract class AbstractUnit : IUnit
    {
        /// <summary>
        /// Returns true if type is equal and Equals(IUnit) returns
        /// true.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return Equals((AbstractUnit)obj);
        }

        /// <summary>
        /// Returns true if Type, Unit, Name and Dimension are equal.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual bool Equals(IUnit other)
        {
            if (other == null || other.GetType() != GetType())
                return false;
            return Equals(Unit, other.Unit) &&
                   Equals(Description, other.Description) &&
                   Equals(Dimension, other.Dimension);
        }

        /// <summary>
        /// Returns a hash code from Description, Unit and Dimension.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return 17 * (Description != null ? Description.GetHashCode() : -35723) +
                       47 * (Unit != null ? Unit.GetHashCode() : 3856287) +
                       87 * (Dimension != null ? Dimension.GetHashCode() : -217933);
            }
        }

        /// <summary>
        /// Returns the Unit.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Unit;
        }

        /// <summary>
        /// Returns the textual representation of the unit, e.g. "kg".
        /// </summary>
        public abstract string Unit { get; }
        /// <summary>
        /// Returns the long textual representation of the unit, e.g. "kilograms".
        /// </summary>
        public abstract string Description { get; }

        public abstract IDimension Dimension { get; }
        public abstract bool IsEquivialent(IUnit unit);
    }
}
