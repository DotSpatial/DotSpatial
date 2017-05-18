// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Globalization;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// DynamicVisibilityTypeConverter
    /// </summary>
    public class DynamicVisibilityTypeConverter : StringConverter
    {
        #region Methods

        /// <summary>
        /// Returns true if we are converting from boolean
        /// </summary>
        /// <param name="context">The type descriptor context.</param>
        /// <param name="sourceType">The source type.</param>
        /// <returns>True, if the source type is bool.</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(bool);
        }

        /// <summary>
        /// Returns true if we are converting to strings
        /// </summary>
        /// <param name="context">The type descriptor context.</param>
        /// <param name="destinationType">The destination type.</param>
        /// <returns>True, if the destination type is string.</returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string);
        }

        /// <summary>
        /// Returns the converted value.
        /// </summary>
        /// <param name="context">The type descriptor context.</param>
        /// <param name="culture">The culture info.</param>
        /// <param name="value">The value.</param>
        /// <param name="destinationType">The destination type.</param>
        /// <returns>The resulting value.</returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return (bool)value ? "Enabled" : "Disabled";
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion
    }
}