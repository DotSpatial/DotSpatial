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
using System.Reflection;

namespace DotSpatial.Positioning.Design
{
    /// <summary>
    /// Provides functionality to converting values to and from
    /// <strong>GeographicSize</strong> objects.
    /// </summary>
    /// <remarks>
    /// <para>This class allows any <strong>GeographicSize</strong> object to be converted
    /// between other data types, such as <strong>String</strong>. This class is used
    /// primarily during the Windows Forms designer to give detailed information about
    /// properties of type <strong>GeographicSize</strong>, and also allows developers to
    /// type in string values such as "1, 2" and have them converted to
    /// <strong>GeographicSize</strong> objects automatically. Finally, this class controls
    /// design-time serialization of <strong>GeographicSize</strong> object
    /// properties.</para>
    /// <para>In most situations this class is used by the Visual Studio.NET IDE and is
    /// rarely created at run-time.</para>
    /// </remarks>
    public sealed class GeographicSizeConverter : PositioningObjectConverter
    {
        /// <inheritdoc />
        protected override string HandledTypeName => "GeoFramework.GeographicSize";

        /// <inheritdoc />
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
            {
                // Instance descriptor.  Used during Windows Forms serialization.

                // Get the type of the object (probably an GeographicSize)
                Type geographicSizeType = value.GetType();

                // Build the parameters for the type converter
                object[] constructorParameters =
                {
                    geographicSizeType.GetProperty("Width").GetValue(value, null),
                    geographicSizeType.GetProperty("Height").GetValue(value, null)
                };
                Type[] constructorTypes = { Type.GetType("GeoFramework.Longitude"), Type.GetType("GeoFramework.Latitude") };

                // Now activate the constructor
                ConstructorInfo constructor = geographicSizeType.GetConstructor(constructorTypes);
                return new InstanceDescriptor(constructor, constructorParameters);
            }

            // Defer to the base class
            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <inheritdoc />
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <inheritdoc />
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new[] { "Empty", "Maximum", "Minimum" });
        }

        /// <inheritdoc />
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}