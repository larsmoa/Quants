using System;
using System.Collections.Generic;
using System.Linq;

namespace Moa.Units
{
    /// <summary>
    /// Common base class for "compounds" (combined units or dimensions). Implements internal logic.
    /// This class is internal and should not be of interrest to the end user.
    /// </summary>
    /// <typeparam name="T">The base type, e.g. <see cref="IUnit"/> for units. Must be an interface.</typeparam>
    /// <typeparam name="TCompound">The concrete compound type, this is the type that inherits this class. Must implement T.</typeparam>
    /// <example>
    /// <code>
    /// class UnitCompound: AbstractCompound&lt;IUnit, UnitCompound&gt;, IUnit { ... }
    /// </code>
    /// </example>
    public abstract class AbstractCompound<T, TCompound> : IEquatable<T>
                                                         where T : class 
                                                         where TCompound: AbstractCompound<T, TCompound>, T, new()
    {
        private readonly T _identityValue;
        private readonly IDictionary<T, int> _factors;

        /// <summary>
        /// Dictionary that holds the factors of the comound. The result of the compound is
        /// factor0.Key^factor0.Value * factor1.Key^factor1.Value * ... * factorN.Key^factoryN.Value.
        /// </summary>
        public IDictionary<T, int> Factors { get { return _factors; } }

        /// <summary>
        /// Creates an empty compound.
        /// </summary>
        /// <param name="identityValue">The base value that is considered to be the "identity" or no-value. For units this would be Unitless.Instance.</param>
        protected AbstractCompound(T identityValue)
        {
            if (!typeof(T).IsInterface)
                throw new TypeLoadException(string.Format("Type '{0}' must be an interface.", typeof(T).Name));
            if ((this as T) == null)
                throw new TypeLoadException("Subclasses of AbstractCompound<T, TCompound> must also implement T.");

            _identityValue = identityValue;
            _factors = new Dictionary<T, int>();
        }

        /// <summary>
        /// Creates a single-factor compound.
        /// </summary>
        /// <param name="identityValue">The base value that is considered to be the "identify" or no-value. For units this would be <see cref="Unitless.Instance"/>.</param>
        /// <param name="dividend"></param>
        protected AbstractCompound(T identityValue, T dividend)
            : this(identityValue)
        {
            MultiplySelf(dividend, 1);
        }

        /// <summary>
        /// Creates a (a/b)-compound.
        /// </summary>
        /// <param name="identityValue">The base value that is considered to be the "identify" or no-value. For units this would be <see cref="Unitless.Instance"/>.</param>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        protected AbstractCompound(T identityValue, T dividend, T divisor)
            : this(identityValue)
        {
            MultiplySelf(dividend, 1);
            MultiplySelf(divisor, -1);
        }

        /// <summary>
        /// Creates a compound from the given factors. The dictionay contains
        /// factors and a power, e.g. "a^2, b^-1" maps to (a^2/b).
        /// </summary>
        /// <param name="identityValue">The base value that is considered to be the "identify" or no-value. For units this would be <see cref="Unitless.Instance"/>.</param>
        /// <param name="factors"></param>
        protected AbstractCompound(T identityValue, IDictionary<T, int> factors)
            : this(identityValue)
        {
            _factors = new Dictionary<T, int>(factors);
        }

        /// <summary>
        /// Implement in subclass to create a deep clone of the compound with the same factors.
        /// </summary>
        /// <returns></returns>
        protected abstract TCompound Clone();
        /// <summary>
        /// Implement in subclass to create a compound from the given factors.
        /// </summary>
        /// <param name="factors"></param>
        /// <returns></returns>
        protected abstract TCompound CreateFromFactors(IDictionary<T, int>  factors);

        /// <summary>
        /// Multiplies this compound with the given factors and returns the result. Notice
        /// that the return type is not a compound, the result may be simpler as a result
        /// of cancelling out factors.
        /// </summary>
        /// <param name="factors"></param>
        /// <returns></returns>
        public T Multiply(params T[] factors)
        {
            if (factors.All(x => (Equals(x, _identityValue))))
                return Simplify();

            TCompound clone = Clone();
            foreach (T factor in factors)
            {
                clone.MultiplySelf(factor, 1);
            }
            return clone.Simplify();
        }

        /// <summary>
        /// Divides this compound with the given divisors and returns the result. Notice
        /// that the return type is not a compound, the result may be simpler as a result
        /// of cancelling out factors.
        /// </summary>
        /// <param name="divisors"></param>
        /// <returns></returns>
        public T Divide(params T[] divisors)
        {
            if (divisors.All(x => (Equals(x, _identityValue))))
                return Simplify();

            TCompound clone = Clone();
            foreach (T divisor in divisors)
            {
                // Multiply with the inverse
                clone.MultiplySelf(divisor, -1);
            }
            return clone.Simplify();

        }

        /// <summary>
        /// Returns the arithmetic inverse of the compound.
        /// </summary>
        /// <returns></returns>
        public T Inverse()
        {
            IDictionary<T, int> clonedFactors = new Dictionary<T, int>();
            foreach (KeyValuePair<T, int> factor in _factors)
            {
                clonedFactors[factor.Key] = -factor.Value;
            }
            return CreateFromFactors(clonedFactors).Simplify();
        }

        /// <summary>
        /// Simplifies compound if possible (e.g. one factor only doesn't need to
        /// be compound).
        /// </summary>
        /// <returns></returns>
        public T Simplify()
        {
            if (_factors.Count == 0)
            {
                return _identityValue;
            }
            if (_factors.Count == 1 && _factors.First().Value == 1) // a
            {
                return _factors.First().Key;
            }
            if (_factors.Count == 1 && Equals(_factors.First().Key, _identityValue)) // Identity^X
            {
                return _identityValue;
            }
            return this as T;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;
            return Equals((T) obj);
        }

        public bool Equals(T other)
        {
            if (other == null || other.GetType() != GetType())
            {
                return false;
            }

            AbstractCompound<T, TCompound> compound = other as AbstractCompound<T, TCompound>;
            if (compound != null && _factors.Count == compound._factors.Count) // compound is never null, check disables warning
            {
                foreach (KeyValuePair<T, int> factor in _factors)
                {
                    if (!compound._factors.ContainsKey(factor.Key) || compound._factors[factor.Key] != factor.Value)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int code = 358372852;
                foreach (var factor in _factors)
                    code ^= factor.GetHashCode();
                return code;
            }
        }

        private void MultiplySelf(T unit, int power)
        {
            if (unit is AbstractCompound<T, TCompound>)
            {
                // Don't nest compounds, unroll
                AbstractCompound<T, TCompound> compound = unit as AbstractCompound<T, TCompound>;
                foreach (KeyValuePair<T, int> factor in compound._factors)
                {
                    MultiplySelf(factor.Key, power*factor.Value);
                }
            }
            else
            {
                IncreaseFactor(unit, power);
            }
        }

        private void IncreaseFactor(T factor, int by)
        {
            if (by == 0 || Equals(factor, _identityValue))
                return;

            if (_factors.ContainsKey(factor))
            {
                int power = _factors[factor] + by;
                if (power != 0)
                {
                    _factors[factor] = power;
                }
                else
                {
                    _factors.Remove(factor);
                }
            }
            else
            {
                _factors[factor] = by;
            }
        }

        protected string GenerateString(Func<T, string> factorToString)
        {
            int dividends = _factors.Where(x => x.Value > 0).Sum(x => x.Value);
            int divisors = -_factors.Where(x => x.Value < 0).Sum(x => x.Value);

            if (dividends > 0 && divisors > 0)
            {
                string dividendsString = GenerateFactorsStrings(_factors.Where(x => x.Value > 0), factorToString);
                string divisorsString = GenerateFactorsStrings(_factors.Where(x => x.Value < 0), factorToString);
                if (dividends > 1)
                    dividendsString = "(" + dividendsString + ")";
                if (divisors > 1)
                    divisorsString = "(" + divisorsString + ")";
                return dividendsString + "/" + divisorsString;
            }
            if (dividends > 0) // divisors == 0
            {
                return GenerateFactorsStrings(_factors, factorToString);
            }
            if (divisors > 1) // dividens == 0
            {
                return "1/(" + GenerateFactorsStrings(_factors, factorToString) + ")";
            }
            if (divisors == 1) // dividens == 0
            {
                return "1/" + GenerateFactorsStrings(_factors, factorToString);
            }
            // else if divisors == 0 && dividens == 0
            return factorToString(_identityValue);
        }

        private static string GenerateFactorsStrings(IEnumerable<KeyValuePair<T, int>> factors, Func<T, string> unitToString)
        {
            return string.Join("*", factors.Select(x => GenerateFactorString(x.Key, Math.Abs(x.Value), unitToString)).OrderBy(x => x));
        }

        private static string GenerateFactorString(T unit, int power, Func<T, string> unitToString)
        {
            if (power == 1)
                return unitToString(unit);
            return unitToString(unit) + "^" + power;
        }
    }
}