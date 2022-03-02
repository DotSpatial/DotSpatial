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
    /// Provides functionality to converting values to and from <strong>PointD</strong>
    /// objects.
    /// </summary>
    /// <remarks>
    /// <para>This class allows any <strong>PointD</strong> object to be converted between
    /// other data types, such as <strong>String</strong>. This class is used primarily
    /// during the Windows Forms designer to give detailed information about properties of
    /// type <strong>PointD</strong>, and also allows developers to type in string values
    /// such as "10.123, 20.123" and have them converted to <strong>PointD</strong> objects
    /// automatically. Finally, this class controls design-time serialization of
    /// <strong>PointD</strong> object properties.</para>
    /// <para>In most situations this class is used by the Visual Studio.NET IDE and is
    /// rarely created at run-time.</para>
    /// </remarks>
    // The Original Code is from http://geoframework.codeplex.com/ version 2.0
    public sealed class PointDConverter : PositioningObjectConverter
    {
        /// <inheritdoc />
        protected override string HandledTypeName => "GeoFramework.PointD";

        /// <inheritdoc />
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
            {
                // Instance descriptor.  Used during Windows Forms serialization.

                // Get the type of the object (probably an PointD)
                Type pointDType = value.GetType();

                // Get the properties needed to generate a constructor
                object[] constructorParameters = { pointDType.GetProperty("X").GetValue(value, null), pointDType.GetProperty("Y").GetValue(value, null) };
                Type[] constructorTypes = { typeof(double), typeof(double) };

                // Now activate the constructor
                ConstructorInfo constructor = pointDType.GetConstructor(constructorTypes);
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
            return new StandardValuesCollection(new[] { "Empty", "Invalid" });
        }

        /// <inheritdoc />
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}