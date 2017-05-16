// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace DotSpatial.Positioning.Design
{
    /// <summary>
    /// Provides functionality to converting values to and from <strong>Percent</strong>
    /// objects.
    /// </summary>
    /// <remarks>
    /// <para>This class allows any <strong>Percent</strong> object to be converted between
    ///  other data types, such as <strong>Double</strong>, <strong>Integer</strong> and
    ///  <strong>String</strong>. This class is used primarily during the Windows Forms
    ///  designer to give detailed information about properties of type
    ///  <strong>Percent</strong>, and also allows developers to type in string values such as
    ///  "1.50" and have them converted to <strong>Percent</strong> objects automatically.
    ///  Finally, this class controls design-time serialization of <strong>Percent</strong>
    ///  object properties.</para>
    /// <para>In most situations this class is used by the Visual Studio.NET IDE and is
    ///  rarely created at run-time.</para>
    /// </remarks>
    // The Original Code is from http://geoframework.codeplex.com/ version 2.0
    public class PercentConverter : PositioningNumericObjectConverter
    {
        /// <inheritdoc />
        protected override string HandledTypeName => "GeoFramework.Percent";

        /// <inheritdoc />
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
            {
                // Instance descriptor.  Used during Windows Forms serialization.

                // Get the type of the object (probably an Percent)
                Type percentType = value.GetType();

                // Get the properties needed to generate a constructor
                object[] constructorParameters = { percentType.GetProperty("Value").GetValue(value, null) };
                Type[] constructorTypes = { typeof(float) };

                // Now activate the constructor
                ConstructorInfo constructor = percentType.GetConstructor(constructorTypes);
                return new InstanceDescriptor(constructor, constructorParameters);
            }

            // Defer to the base class
            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <inheritdoc />
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return false;
        }

        /// <inheritdoc />
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return null;
        }

        /// <inheritdoc />
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}