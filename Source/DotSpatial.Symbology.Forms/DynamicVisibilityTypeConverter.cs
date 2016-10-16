// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/27/2009 6:09:24 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

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
        #region Private Variables

        #endregion

        #region Methods

        /// <summary>
        /// Returns true if we are converting to strings
        /// </summary>
        /// <param name="context"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string)) return true;
            return false;
        }

        /// <summary>
        /// Return
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
                if ((bool)value)
                {
                    return "Enabled";
                }
                return "Disabled";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>
        /// Returns true if we are converting from boolean
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(bool)) return true;
            return false;
        }

        #endregion

        #region Properties

        #endregion
    }
}