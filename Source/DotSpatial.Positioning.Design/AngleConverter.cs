// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace DotSpatial.Positioning.Design
{
    /* notice: After a whole ton of failed attempts to write this class, I learned some important things
         * about type converters.  First and foremost, the class should not use the object that it converts.
         * For example, the AngleConverter should not use any reference to an Angle class.  Instead, TypeResolver
         * services are used to create the type.  This allows objects to be converted to the correct type, regardless
         * of which assembly they live in.
         */

    /// <summary>
    /// Provides functionality to converting values to and from <strong>Angle</strong>
    /// objects.
    /// </summary>
    /// <remarks>
    /// <para>This class allows any <strong>Angle</strong> object to be converted between
    /// other data types, such as <strong>Double</strong>, <strong>Integer</strong> and
    /// <strong>String</strong>. This class is used primarily during the Windows Forms
    /// designer to give detailed information about properties of type
    /// <strong>Angle</strong>, and also allows developers to type in string values such as
    /// "1.50" and have them converted to <strong>Angle</strong> objects automatically.
    /// Finally, this class controls design-time serialization of <strong>Angle</strong>
    /// object properties.</para>
    /// <para>In most situations this class is used by the Visual Studio.NET IDE and is
    /// rarely created at run-time.</para>
    /// </remarks>
    // The Original Code is from http://geoframework.codeplex.com/ version 2.0
    public class AngleConverter : PositioningNumericObjectConverter
    {
        /// <inheritdoc />
        protected override string HandledTypeName => "GeoFramework.Angle";

        /// <inheritdoc />
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            // What is the destination type?
            if (destinationType == typeof(InstanceDescriptor))
            {
                // Instance descriptor.  Used during Windows Forms serialization.

                // Get the type of the object (probably an Angle)
                Type angleType = value.GetType();

                // Get the properties needed to generate a constructor
                object[] constructorParameters = { angleType.GetProperty("DecimalDegrees").GetValue(value, null) };
                Type[] constructorTypes = { typeof(double) };

                // Now activate the constructor
                ConstructorInfo constructor = angleType.GetConstructor(constructorTypes);
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
            return new StandardValuesCollection(new[] { "0°", "45°", "90°", "135°", "180°", "225°", "270°", "315°" });
        }

        /// <inheritdoc />
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}