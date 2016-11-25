﻿// ********************************************************************************************************
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
    /// Provides functionality to convert string values to and from Latitude objects at design time.
    /// </summary>
    /// <remarks>
    /// 	<para>This class allows any <strong>Latitude</strong> object to be converted
    ///     between other data types, such as <strong>Double</strong>, <strong>Integer</strong>
    ///     and <strong>String</strong>. This class is used primarily during the Windows Forms
    ///     designer to give detailed information about properties of type
    ///     <strong>Latitude</strong>, and also allows developers to type in string values such
    ///     as "12N" (for 12° North) and have them converted to <strong>Latitude</strong>
    ///     objects automatically. Finally, this class controls design-time serialization of
    ///     <strong>Latitude</strong> object properties.</para>
    /// 	<para>In most situations this class is used by the Visual Studio.NET IDE and is
    ///     rarely created at run-time.</para>
    /// </remarks>
    public sealed class LatitudeConverter : PositioningNumericObjectConverter
    {
        /// <inheritdocs/>
        protected override string HandledTypeName
        {
            get
            {
                return "GeoFramework.Latitude";
            }
        }

        /// <inheritdocs/>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            // What is the destination type?
            if (destinationType == typeof(InstanceDescriptor))
            {
                // Instance descriptor.  Used during Windows Forms serialization.

                // Get the type of the object (probably an Latitude)
                Type latitudeType = value.GetType();

                // Get the properties needed to generate a constructor
                object[] constructorParameters = new[] { latitudeType.GetProperty("DecimalDegrees").GetValue(value, null) };
                Type[] constructorTypes = new[] { typeof(double) };

                // Now activate the constructor
                ConstructorInfo constructor = latitudeType.GetConstructor(constructorTypes);
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
		            "Equator",
		            "NorthPole",
		            "SouthPole",
		            "TropicOfCapricorn",
		            "TropicOfCancer" });
        }

        /// <inheritdocs/>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}