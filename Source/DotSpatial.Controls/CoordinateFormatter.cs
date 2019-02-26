using System;
using DotSpatial.Serialization;
using GeoAPI.Geometries;

namespace DotSpatial.Controls
{
    public class CoordinateFormatter : SerializationFormatter
    {
        public override object FromString(string value)
        {
            var xyzm = value.Split(',');
            double x = parse(xyzm[0]);
            double y = parse(xyzm[1]);
            double z = parse(xyzm[2]);
            double m = parse(xyzm[3]);
            return new Coordinate(x, y, z, m);
        }

        private double parse(string val)
        {
            try
            {
                return double.Parse(val.Trim(' ', '(', ')'));
            }
            catch (FormatException e)
            {
                return double.NaN;
            }
        }

        public override string ToString(object value)
        {
            if (value is Coordinate)
            {
                return ((Coordinate)value).ToString();
            }
            else
            {
                throw new ArgumentException("Must be a Coordinate");
            }
        }
    }
}
