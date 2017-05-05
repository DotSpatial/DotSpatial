// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/31/2009 2:16:31 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Globalization;
using DotSpatial.Data;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// CategoryCollectionConverter
    /// </summary>
    public class CategoryCollectionConverter : CollectionConverter
    {
        #region Methods

        /// <summary>
        /// Determines how to convert from an interface
        /// </summary>
        /// <param name="context">The type descriptor context.</param>
        /// <param name="sourceType">The source type.</param>
        /// <returns>True if conversion is possible.</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(IChangeEventList<IPointCategory>))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Converts the collection to a string.
        /// </summary>
        /// <param name="context">The type descriptor context.</param>
        /// <param name="culture">The culture info.</param>
        /// <param name="value">The value.</param>
        /// <param name="destinationType">The destination type.</param>
        /// <returns>The resulting string.</returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return "Collection";
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion
    }
}