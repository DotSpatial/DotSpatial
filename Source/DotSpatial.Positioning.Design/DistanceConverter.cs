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
    /// Provides functionality to converting values to and from <strong>Distance</strong>
    /// objects.
    /// </summary>
    /// <remarks>
    /// <para>This class allows any <strong>Distance</strong> object to be converted
    /// between other data types, such as <strong>String</strong>. This class is used
    /// primarily during the Windows Forms designer to give detailed information about
    /// properties of type <strong>Distance</strong>, and also allows developers to type in
    /// string values such as "2 miles" and have them converted to
    /// <strong>Distance</strong> objects automatically. Finally, this class controls
    /// design-time serialization of <strong>Distance</strong> object properties.</para>
    /// <para>In most situations this class is used by the Visual Studio.NET IDE and is
    /// rarely created at run-time.</para>
    /// </remarks>
    // The Original Code is from http://geoframework.codeplex.com/ version 2.0
    public sealed class DistanceConverter : PositioningObjectConverter
    {
        /// <inheritdoc />
        protected override string HandledTypeName => "GeoFramework.Distance";

        /// <inheritdoc />
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
            {
                // Instance descriptor.  Used during Windows Forms serialization.

                // Get the type of the object (probably an Distance)
                Type distanceType = value.GetType();

                // Build the parameters for the type converter
                object[] constructorParameters =
                {
                    distanceType.GetProperty("Value").GetValue(value, null),
                    distanceType.GetProperty("Units").GetValue(value, null)
                };
                Type[] constructorTypes = { typeof(double), Type.GetType("GeoFramework.DistanceUnit") };

                // Now activate the constructor
                ConstructorInfo constructor = distanceType.GetConstructor(constructorTypes);
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
            // Are we currently in metric or imperial?
            if (RegionInfo.CurrentRegion.IsMetric)
            {
                return new StandardValuesCollection(new[]
                {
                    "1 centimeter",
                    "10 centimeters",
                    "100 centimeters",
                    "1 meter",
                    "10 meters",
                    "100 meters",
                    "1 kilometer",
                    "10 kilometers",
                    "100 kilometers"
                });
            }

            return new StandardValuesCollection(new[]
            {
                "1 inch",
                "10 inches",
                "100 inches",
                "1 foot",
                "10 feet",
                "100 feet",
                "1 mile",
                "10 miles",
                "100 miles"
            });
        }

        /// <inheritdoc />
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}