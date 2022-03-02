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
    /// Provides functionality to converting values to and from
    /// <strong>RectangleD</strong> objects.
    /// </summary>
    /// <remarks>
    /// <para>This class allows any <strong>RectangleD</strong> object to be converted
    ///  between other data types, such as <strong>String</strong>. This class is used
    ///  primarily during the Windows Forms designer to give detailed information about
    ///  properties of type <strong>RectangleD</strong>, and also allows developers to type
    ///  in string values such as "1.5, 2.5, 4.5, 7.8" and have them converted to
    ///  <strong>RectangleD</strong> objects automatically. Finally, this class controls
    ///  design-time serialization of <strong>RectangleD</strong> object properties.</para>
    /// <para>In most situations this class is used by the Visual Studio.NET IDE and is
    ///  rarely created at run-time.</para>
    /// </remarks>
    // The Original Code is from http://geoframework.codeplex.com/ version 2.0
    public sealed class RectangleDConverter : PositioningObjectConverter
    {
        /// <inheritdoc />
        protected override string HandledTypeName => "GeoFramework.RectangleD";

        /// <inheritdoc />
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
            {
                // Get the type of the object (probably an RectangleD)
                Type rectangleDType = value.GetType();

                // Get the properties needed to generate a constructor
                object[] constructorParameters =
                    {
                        rectangleDType.GetProperty("Left").GetValue(value, null),
                        rectangleDType.GetProperty("Top").GetValue(value, null),
                        rectangleDType.GetProperty("Right").GetValue(value, null),
                        rectangleDType.GetProperty("Bottom").GetValue(value, null)
                    };
                Type[] constructorTypes = { typeof(double), typeof(double), typeof(double), typeof(double) };

                // Now activate the constructor
                ConstructorInfo constructor = rectangleDType.GetConstructor(constructorTypes);
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
            return new StandardValuesCollection(new[] { "Empty" });
        }

        /// <inheritdoc />
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}