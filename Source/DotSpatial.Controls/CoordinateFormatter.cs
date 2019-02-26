using System;
using System.Text.RegularExpressions;
using DotSpatial.Serialization;
using GeoAPI.Geometries;

namespace DotSpatial.Controls
{
    public class CoordinateFormatter : SerializationFormatter
    {
        public override object FromString(string value)
        {
            value = value.Trim();

            //Something like "(12.5, 2.0, NaN, NaN)"
            Regex rx = new Regex(@"^\((\s*(([0-9]+(\.+[0-9]+)?)|(NaN))\s*,){3}(\s*(([0-9]+(\.+[0-9]+)?)|(NaN)))\s*\)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            if (rx.IsMatch(value))
            {
                var xyzm = value.Split(',');
                double x = parse(xyzm[0]);
                double y = parse(xyzm[1]);
                double z = parse(xyzm[2]);
                double m = parse(xyzm[3]);
                return new Coordinate(x, y, z, m);
            }
            else
            {
                throw new FormatException(value + "is not a coordinate");
            }
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
