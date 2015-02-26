using System;
using System.Collections.Generic;
using System.Linq;

namespace Moa.Units.Conversion
{
    /// <summary>
    /// Converter that converts between two units through a series of converters, e.g.
    /// A->D through A->B, B->C and C->D.
    /// </summary>
    public sealed class CompositeConverter: IValueConverter
    {
        private readonly LinkedList<IValueConverter> _converters;

        /// <summary>
        /// Creates an composite converter. Use AddConverter() to add
        /// conversion stages.
        /// </summary>
        public CompositeConverter()
        {
            _converters = new LinkedList<IValueConverter>();
        }

        /// <summary>
        /// Adds an IValueConverter to the sequence of converters. If the converter
        /// is not is the first converter added, it's Source unit must match the Target
        /// unit of the previous converter.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Throw when the target unit of the previous converter does not match source 
        /// unit of the provided converter.
        /// </exception>
        /// <param name="converter"></param>
        public void AddConverter(IValueConverter converter)
        {
            if (_converters.Count != 0)            
            {
                IUnit source = converter.Source;
                IUnit prevTarget = _converters.Last.Value.Target;
                if (!source.Equals(prevTarget))
                {
                    string message = 
                        string.Format("Source unit '{0}' does not match target unit '{1}' of the previous converter.",
                                      source, prevTarget);
                    throw new ArgumentException(message);
                }
            }
            _converters.AddLast(converter);
        }

        private CompositeConverter(IEnumerable<IValueConverter> converters)
        {
            _converters = new LinkedList<IValueConverter>(converters);
        }

        public IUnit Target
        {
            get
            {
                if (_converters.Count > 0)
                    return _converters.Last.Value.Target;
                // No converters yet
                return null;
            }
        }

        public IUnit Source
        {
            get
            {
                if (_converters.Count > 0)
                    return _converters.First.Value.Source;
                // No converters yet
                return null;
            }
        }

        public double Convert(double value)
        {
            double intermediate = value;
            LinkedListNode<IValueConverter> converter = _converters.First;
            while (converter != null)
            {
                intermediate = converter.Value.Convert(intermediate);
                converter = converter.Next;
            }
            return intermediate;
        }

        public float Convert(float value)
        {
            float intermediate = value;
            LinkedListNode<IValueConverter> converter = _converters.First;
            while (converter != null)
            {
                intermediate = converter.Value.Convert(intermediate);
                converter = converter.Next;
            }
            return intermediate;
        }

        public IValueConverter Inversed()
        {
            return new CompositeConverter(_converters.Reverse());
        }
    }
}
