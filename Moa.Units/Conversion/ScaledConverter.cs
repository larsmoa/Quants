using System;

namespace Moa.Units.Conversion
{
    /// <summary>
    /// Value converter that for converting between two units that relate
    /// by a scale and (optionally) an offset.
    /// </summary>
    public sealed class ScaledConverter: IValueConverter
    {
        private readonly double _scale;
        private readonly double _offset;
        private readonly IUnit _target;
        private readonly IUnit _source;

        /// <summary>
        /// Creates a new scaled converter.
        /// </summary>
        /// <param name="scale">
        /// How source and target relates to eachother in terms of scale.
        /// result = source
        /// </param>
        /// <param name="offset"></param>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public ScaledConverter(double scale, double offset, IUnit source, IUnit target)
        {
            if (scale == 0.0)
                throw new ArgumentException("Scale cannot be zero.");

            _scale = scale;
            _offset = offset;
            _target = target;
            _source = source;
        }

        public IUnit Target
        {
            get { return _target; }
        }

        public IUnit Source
        {
            get { return _source; }
        }

        public double Convert(double value)
        {
            return _scale*value + _offset;
        }

        public float Convert(float value)
        {
            return (float) (_scale*value + _offset);
        }


        public IValueConverter Inversed()
        {
            return new ScaledConverter(1.0/_scale, -_offset/_scale, _target, _source);
        }
    }
}
