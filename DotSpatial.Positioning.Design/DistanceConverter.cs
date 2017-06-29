// ********************************************************************************************************
// Product Name: DotSpatial.Positioning.dll
// Description:  A library for managing GPS connections.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
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
    /// Provides functionality to converting values to and from <strong>Distance</strong>
    /// objects.
    /// </summary>
    /// <remarks>
    /// 	<para>This class allows any <strong>Distance</strong> object to be converted
    ///     between other data types, such as <strong>String</strong>. This class is used
    ///     primarily during the Windows Forms designer to give detailed information about
    ///     properties of type <strong>Distance</strong>, and also allows developers to type in
    ///     string values such as "2 miles" and have them converted to
    ///     <strong>Distance</strong> objects automatically. Finally, this class controls
    ///     design-time serialization of <strong>Distance</strong> object properties.</para>
    /// 	<para>In most situations this class is used by the Visual Studio.NET IDE and is
    ///     rarely created at run-time.</para>
    /// </remarks>
    public sealed class DistanceConverter : PositioningObjectConverter
    {
        /// <inheritdocs/>
        protected override string HandledTypeName
        {
            get
            {
                return "GeoFramework.Distance";
            }
        }

        /// <inheritdocs/>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
            {
                // Instance descriptor.  Used during Windows Forms serialization.

                // Get the type of the object (probably an Distance)
                Type distanceType = value.GetType();

                // Build the parameters for the type converter
                object[] constructorParameters = new[]
                                                     {   distanceType.GetProperty("Value").GetValue(value, null),
                                                         distanceType.GetProperty("Units").GetValue(value, null) };
                Type[] constructorTypes = new[] { typeof(double), Type.GetType("GeoFramework.DistanceUnit") };

                // Now activate the constructor
                ConstructorInfo constructor = distanceType.GetConstructor(constructorTypes);
                return new InstanceDescriptor(constructor, constructorParameters);
            }

            // Defer to the base class
            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <inheritdocs/>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <inheritdocs/>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            // Are we currently in metric or imperial?
            if (RegionInfo.CurrentRegion.IsMetric)
            {
                return new StandardValuesCollection(new[] {
		            "1 centimeter",
		            "10 centimeters",
		            "100 centimeters",
                    "1 meter",
                    "10 meters",
                    "100 meters",
                    "1 kilometer",
                    "10 kilometers",
                    "100 kilometers" });
            }
            return new StandardValuesCollection(new[] {
                                                          "1 inch",
                                                          "10 inches",
                                                          "100 inches",
                                                          "1 foot",
                                                          "10 feet",
                                                          "100 feet",
                                                          "1 mile",
                                                          "10 miles",
                                                          "100 miles" });
        }

        /// <inheritdocs/>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}