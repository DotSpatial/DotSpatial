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
    /// Provides functionality to converting values to and from
    /// <strong>GeographicRectangle</strong> objects.
    /// </summary>
    /// <remarks>
    /// 	<para>This class allows any <strong>GeographicRectangle</strong> object to be
    ///     converted between other data types, such as <strong>String</strong>. This class is
    ///     used primarily during the Windows Forms designer to give detailed information about
    ///     properties of type <strong>GeographicRectangle</strong>, and also allows developers
    ///     to type in string values such as "1, 2, 10, 10" and have them converted to
    ///     <strong>GeographicRectangle</strong> objects automatically. Finally, this class
    ///     controls design-time serialization of <strong>GeographicRectangle</strong> object
    ///     properties.</para>
    /// 	<para>In most situations this class is used by the Visual Studio.NET IDE and is
    ///     rarely created at run-time.</para>
    /// </remarks>
    public sealed class GeographicRectangleConverter : PositioningObjectConverter
    {
        /// <inheritdocs/>
        protected override string HandledTypeName
        {
            get { return "GeoFramework.GeographicRectangle"; }
        }

        /// <inheritdocs/>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
            {
                // Instance descriptor.  Used during Windows Forms serialization.

                // Get the type of the object (probably an GeographicRectangle)
                Type geographicRectangleType = value.GetType();

                // Build the parameters for the type converter
                object[] constructorParameters = new[]
                                                     { geographicRectangleType.GetProperty("Top").GetValue(value, null),
                                                       geographicRectangleType.GetProperty("Left").GetValue(value, null),
                                                       geographicRectangleType.GetProperty("Bottom").GetValue(value, null),
                                                       geographicRectangleType.GetProperty("Right").GetValue(value, null)};
                Type[] constructorTypes = new[]
                                              { Type.GetType("GeoFramework.Latitude"),
                                                Type.GetType("GeoFramework.Longitude"),
                                                Type.GetType("GeoFramework.Latitude"),
                                                Type.GetType("GeoFramework.Longitude") };

                //#if DEBUG
                //                        System.Diagnostics.EventLog.WriteEntry("SUCCESS: GeographicRectangleConverter: ConvertTo: \"" + value.ToString() + "\" to New GeographicRectangle(" + DecimalDegrees.ToString() + ").", "");
                //#endif

                // Now activate the constructor
                ConstructorInfo constructor = geographicRectangleType.GetConstructor(constructorTypes);
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
            return new StandardValuesCollection(new[] {
		            "Empty",
		            "Maximum",
		            "Minimum" });
        }

        /// <inheritdocs/>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}