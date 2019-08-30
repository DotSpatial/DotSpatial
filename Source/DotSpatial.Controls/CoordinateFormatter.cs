using System;
using System.Globalization;
using System.Text.RegularExpressions;
using DotSpatial.Serialization;
using GeoAPI.Geometries;

namespace DotSpatial.Controls
{
    public class CoordinateFormatter : SerializationFormatter
    {
        /// <inheritdoc/>
        public override object FromString(string value)
        {
            // Something like "(12.5, 2.0, NaN, NaN)"
            Regex rx = new Regex(@"^\((\s*((-?[0-9]+([,.]+[0-9]+)?)|(NaN))\s*,){3}( \s*((-?[0-9]+([,.]+[0-9]+)?)|(NaN)))\s*\)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            if (rx.IsMatch(value))
            {
                var xyzm = value.Trim()
                            .Trim('(', ')')
                            .Replace(", ", "|")
                            .Split('|');
                double x = Parse(xyzm[0]);
                double y = Parse(xyzm[1]);
                double z = Parse(xyzm[2]);
                double m = Parse(xyzm[3]);
                return new Coordinate(x, y, z, m);
            }
            else
            {
                throw new FormatException(value + "is not a coordinate");
            }
        }

        private static double Parse(string val)
        {
            if (val.Contains("NaN"))
                return double.NaN;

            try
            {
                return double.Parse(val.Trim(), NumberFormatInfo.InvariantInfo);
            }
            catch (FormatException e)
            {
                return double.Parse(val.Trim());
            }
        }

        /// <inheritdoc/>
        public override string ToString(object value)
        {
            if (value is Coordinate)
            {
                var c = (Coordinate)value;
                return "(" + c.X.ToString(NumberFormatInfo.InvariantInfo) + ", " + c.Y.ToString(NumberFormatInfo.InvariantInfo) + ", " + c.Z.ToString(NumberFormatInfo.InvariantInfo) + ", " + c.M.ToString(NumberFormatInfo.InvariantInfo) + ")";
            }
            else
            {
                throw new ArgumentException("Must be a Coordinate");
            }
        }
    }
}
