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
        #region Private Variables

        #endregion

        #region Methods

        /// <summary>
        /// Converts the collection to a string
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
                return "Collection";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>
        /// Determines how to convert from an interface
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(IChangeEventList<IPointCategory>))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        #endregion

        #region Properties

        #endregion
    }
}