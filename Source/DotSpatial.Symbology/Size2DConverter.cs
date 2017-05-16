// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Globalization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Size2DConverter
    /// </summary>
    public class Size2DConverter : ExpandableObjectConverter
    {
        #region Methods

        /// <summary>
        /// Returns true if the source type is string or Size2D.
        /// </summary>
        /// <param name="context">The type descriptor context.</param>
        /// <param name="sourceType">The source type.</param>
        /// <returns>True, if the value can be converted.</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string)) return true;
            if (sourceType == typeof(Size2D)) return true;

            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Converts a string into a Size2D.
        /// </summary>
        /// <param name="context">The type descriptor context.</param>
        /// <param name="culture">The culture info.</param>
        /// <param name="value">The value.</param>
        /// <returns>The resulting Size2D.</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as string;
            if (s != null)
            {
                try
                {
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

                    Size2D result = new Size2D(x, y);
                    return result;
                }
                catch
                {
                    throw new ArgumentException("Cannot convert [" + value + "] to Size2D");
                }
            }

            if (value is Size2D)
            {
                return value;
            }

            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Converts the Size2D into a string
        /// </summary>
        /// <param name="context">The type descriptor context.</param>
        /// <param name="culture">The culture info.</param>
        /// <param name="value">The value.</param>
        /// <param name="destinationType">Type the value should be converted to.</param>
        /// <returns>The resulting object.</returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                if (value.GetType() == typeof(Size2D))
                {
                    Size2D pt = (Size2D)value;
                    return $"{pt.Width}, {pt.Height}";
                }
            }

            if (destinationType == typeof(Size2D))
            {
                if (value.GetType() == typeof(Size2D))
                {
                    return value;
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion
    }
}