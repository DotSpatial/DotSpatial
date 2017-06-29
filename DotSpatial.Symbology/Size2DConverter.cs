// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
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
    /// The ExpandableSetConverter works by assuming that a pair of
    /// </summary>
    public class Size2DConverter : ExpandableObjectConverter
    {
        #region Protected Methods

        /// <summary>
        /// Returns true if the source type is string
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string)) return true;
            if (sourceType == typeof(Size2D)) return true;
            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Converts a string into a Size2D
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <returns></returns>
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
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                if (value.GetType() == typeof(Size2D))
                {
                    Size2D pt = (Size2D)value;
                    return string.Format("{0}, {1}", pt.Width, pt.Height);
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