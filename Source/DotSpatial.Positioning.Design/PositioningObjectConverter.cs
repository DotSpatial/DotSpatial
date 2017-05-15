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
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace DotSpatial.Positioning.Design
{
    /// <summary>
    /// A base Expandable object converter
    /// </summary>
    public abstract class PositioningObjectConverter : ExpandableObjectConverter
    {
        #region Properties

        /// <summary>
        /// Gets the full nume of the assembly housing objects handled by this type converter.
        /// </summary>
        protected virtual string HandledAssemblyName
        {
            get
            {
                return "DotSpatial.Positioning, Culture=neutral, Version=" + HandledAssemblyVersion.ToString(4) + ", PublicKeyToken=b4b0b185210c9dae";
            }
        }

        /// <summary>
        /// Gets the hard coded assembly version of the library that this designer handles.
        /// </summary>
        protected virtual Version HandledAssemblyVersion => new Version("1.0.0.*");

        /// <summary>
        /// Gets the name of the type handled by this type converter.
        /// </summary>
        protected abstract string HandledTypeName { get; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string) || sourceType == typeof(InstanceDescriptor) || sourceType.Name == HandledTypeName)
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        /// <inheritdoc />
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string) || destinationType == typeof(InstanceDescriptor))
            {
                return true;
            }

            // Defer to the base class
            return base.CanConvertTo(context, destinationType);
        }

        /// <inheritdoc />
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            // If the type is a string or a double, it's ok to proceed with conversion.
            if (value is string)
            {
                // Try to resolve the target property type
                Type destinationType = context.PropertyDescriptor.PropertyType;

                // If a destination type was found, go ahead and create a new instance!
                if (destinationType != null)
                {
                    return Activator.CreateInstance(destinationType, value);
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

        /// <inheritdoc />
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            // What is the destination type?
            if (destinationType == typeof(string))
            {
                // String.  Is the value null?
                return value.ToString();
            }

            // Defer to the base class
            return base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion
    }
}