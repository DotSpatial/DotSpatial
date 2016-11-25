// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/1/2009 3:10:20 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Globalization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// PointFConverter
    /// </summary>
    public class Position2DConverter : ExpandableObjectConverter
    {
        #region Protected Methods

        /// <summary>
        /// True if the source type is a string
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns>Boolean, true if source type is a string</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string)) return true;
            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Uses a string to return a new Position2D
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <returns>A String</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                try
                {
                    string s = (string)value;
                    string[] converterParts = s.Split(',');
                    double x;
                    double y;
                    if (converterParts.Length > 1)
                    {
                        x = double.Parse(converterParts[0].Trim());
                        y = double.Parse(converterParts[1].Trim());
                    }
                    else if (converterParts.Length == 1)
                    {
                        x = double.Parse(converterParts[0].Trim());
                        y = 0;
                    }
                    else
                    {
                        x = 0;
                        y = 0;
                    }
                    return new Position2D(x, y);
                }
                catch
                {
                    throw new ArgumentException("Cannot convert [" + value + "] to pointF");
                }
            }
            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Creates a string from the specified Position2D
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns>A Position2D</returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                if (value.GetType() == typeof(Position2D))
                {
                    Position2D pt = (Position2D)value;
                    return string.Format("{0}, {1}", pt.X, pt.Y);
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion
    }
}