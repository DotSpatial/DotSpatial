// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// PointFConverter
    /// </summary>
    public class PointFConverter : ExpandableObjectConverter
    {
        #region Methods

        /// <summary>
        /// Boolean, true if the source type is a string.
        /// </summary>
        /// <param name="context">The type descriptor context.</param>
        /// <param name="sourceType">The source type.</param>
        /// <returns>True, if the given type can be converted to PointF.</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string)) return true;
            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Converts the specified string into a PointF
        /// </summary>
        /// <param name="context">The type descriptor context.</param>
        /// <param name="culture">The culture info.</param>
        /// <param name="value">The PointF.</param>
        /// <returns>The converted value</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as string;
            if (s != null)
            {
                try
                {
                    string[] converterParts = s.Split(',');
                    float x;
                    float y;
                    if (converterParts.Length > 1)
                    {
                        x = float.Parse(converterParts[0].Trim());
                        y = float.Parse(converterParts[1].Trim());
                    }
                    else if (converterParts.Length == 1)
                    {
                        x = float.Parse(converterParts[0].Trim());
                        y = 0;
                    }
                    else
                    {
                        x = 0F;
                        y = 0F;
                    }

                    return new PointF(x, y);
                }
                catch
                {
                    throw new ArgumentException("Cannot convert [" + value + "] to pointF");
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Converts the PointF into a string
        /// </summary>
        /// <param name="context">The type descriptor context.</param>
        /// <param name="culture">The culture info.</param>
        /// <param name="value">The PointF.</param>
        /// <param name="destinationType">The destination type.</param>
        /// <returns>The converted value.</returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                if (value is PointF)
                {
                    PointF pt = (PointF)value;
                    return string.Format("{0}, {1}", pt.X, pt.Y);
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion
    }
}