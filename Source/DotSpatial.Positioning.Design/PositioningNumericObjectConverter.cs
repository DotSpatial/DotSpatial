// ********************************************************************************************************
// Product Name: DotSpatial.Positioning.dll
// Description:  A library for managing GPS connections.
// ********************************************************************************************************
//
// The Original Code is from http://geoframework.codeplex.com/ version 2.0
//
// The Initial Developer of this original code is Jon Pearson. Submitted Oct. 21, 2010 by Ben Tombs (tidyup)
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// -------------------------------------------------------------------------------------------------------
// |    Developer             |    Date    |                             Comments
// |--------------------------|------------|--------------------------------------------------------------
// | Tidyup  (Ben Tombs)      | 10/21/2010 | Original copy submitted from modified GeoFrameworks 2.0
// | Shade1974 (Ted Dunsford) | 10/21/2010 | Added file headers reviewed formatting with resharper.
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Globalization;

namespace DotSpatial.Positioning.Design
{
    /// <summary>
    /// An abstract class that is inherited by several other classes to handle type conversions.
    /// </summary>
    public abstract class PositioningNumericObjectConverter : PositioningObjectConverter
    {
        /// <inheritdoc/>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(double)
                    || sourceType == typeof(int)
                    || sourceType == typeof(float))
            {
                return true;
            }

            // Defer to the base class
            return base.CanConvertFrom(context, sourceType);
        }

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            // If the type is a string or a double, it's ok to proceed with conversion.
            if (value is string || value is double || value is int || value is float)
            {
                // Try to resolve the target property type
                Type destinationType = context.PropertyDescriptor.PropertyType;

                // If a destination type was found, go ahead and create a new instance!
                if (destinationType != null)
                {
                    return Activator.CreateInstance(destinationType, new[] { value });
                }
            }

            // Defer to the base class
            return base.ConvertFrom(context, culture, value);
        }

        /// <inheritdoc/>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(double)
                    || destinationType == typeof(int)
                    || destinationType == typeof(float))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }
    }
}