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
    /// Provides functionality to converting values to and from <strong>Speed</strong>
    /// objects.
    /// </summary>
    /// <remarks>
    /// <para>This class allows any <strong>Speed</strong> object to be converted between
    ///  other data types, such as <strong>String</strong>. This class is used primarily
    ///  during the Windows Forms designer to give detailed information about properties of
    ///  type <strong>Speed</strong>, and also allows developers to type in string values
    ///  such as "120 km/h" and have them converted to <strong>Speed</strong> objects
    ///  automatically. Finally, this class controls design-time serialization of
    ///  <strong>Speed</strong> object properties.</para>
    /// <para>In most situations this class is used by the Visual Studio.NET IDE and is
    ///  rarely created at run-time.</para>
    /// </remarks>
    // The Original Code is from http://geoframework.codeplex.com/ version 2.0
    public sealed class SpeedConverter : PositioningObjectConverter
    {
        /// <inheritdoc />
        protected override string HandledTypeName => "GeoFramework.Speed";

        /// <inheritdoc />
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            // What is the destination type?
            if (destinationType == typeof(InstanceDescriptor))
            {
                // Instance descriptor.  Used during Windows Forms serialization.

                // Get the type of the object (probably an Speed)
                Type speedType = value.GetType();

                // Build the parameters for the type converter
                object[] constructorParameters = { speedType.GetProperty("Value").GetValue(value, null), speedType.GetProperty("Units").GetValue(value, null) };
                Type[] constructorTypes = { typeof(double), Type.GetType("GeoFramework.SpeedUnit") };

                // Now activate the constructor
                ConstructorInfo constructor = speedType.GetConstructor(constructorTypes);
                return new InstanceDescriptor(constructor, constructorParameters);
            }

            // Defer to the base class
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}