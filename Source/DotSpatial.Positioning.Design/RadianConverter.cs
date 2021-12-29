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
    /// Type converter for angles in radians.
    /// </summary>
    // The Original Code is from http://geoframework.codeplex.com/ version 2.0
    public sealed class RadianConverter : PositioningNumericObjectConverter
    {
        /// <inheritdoc />
        protected override string HandledTypeName => "GeoFramework.Radian";

        /// <inheritdoc />
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
            {
                // Instance descriptor.  Used during Windows Forms serialization.

                // Get the type of the object to convert
                Type sourceType = value.GetType();

                // Get the properties needed to generate a constructor
                object[] constructorParameters = { sourceType.GetProperty("Value").GetValue(value, null) };
                Type[] constructorTypes = { typeof(double) };

                // Now activate the constructor
                ConstructorInfo constructor = sourceType.GetConstructor(constructorTypes);
                return new InstanceDescriptor(constructor, constructorParameters);
            }

            // Defer to the base class
            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <inheritdoc/>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return false;
        }

        /// <inheritdoc/>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return null;
        }

        /// <inheritdoc/>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}