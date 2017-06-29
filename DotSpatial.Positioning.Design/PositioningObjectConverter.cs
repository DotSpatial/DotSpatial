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

namespace DotSpatial.Positioning.Design
{
    /// <summary>
    /// A base Expandable object converter
    /// </summary>
    public abstract class PositioningObjectConverter : ExpandableObjectConverter
    {
        #region ConvertFrom: Converts an object to an Distance

        /// <inheritdocs/>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string)
                || sourceType == typeof(InstanceDescriptor)
                || sourceType.Name == HandledTypeName)
            {
                //Console.WriteLine("DEBUG: " + sourceType.Name + " CAN be converted to " + HandledTypeName + "...");

                return true;
            }

            //Console.WriteLine("DEBUG: " + sourceType.Name + " probably can NOT be converted to " + HandledTypeName + "...");

            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Indicates the name of the type handled by this type converter.
        /// </summary>
        protected abstract string HandledTypeName { get; }

        /// <summary>
        /// Indicates the full nume of the assembly housing objects handled by this type converter.
        /// </summary>
        protected virtual string HandledAssemblyName
        {
            get
            {
                return "DotSpatial.Positioning, Culture=neutral, Version=" + HandledAssemblyVersion.ToString(4) + ", PublicKeyToken=b4b0b185210c9dae";
            }
        }

        /// <summary>
        /// Gets the hard coded assembly version of the library that this designer handles.
        /// </summary>
        protected virtual Version HandledAssemblyVersion
        {
            get
            {
                return new Version("1.0.0.*");
            }
        }

        /// <inheritdocs/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            // If the type is a string or a double, it's ok to proceed with conversion.
            if (value is string)
            {
                // Try to resolve the target property type
                Type destinationType = context.PropertyDescriptor.PropertyType;

                // If a destination type was found, go ahead and create a new instance!
                if (destinationType != null)
                {
                    return Activator.CreateInstance(destinationType, new[] { value });
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

        #endregion

        #region ConvertTo: Converts an Distance to another type

        /// <inheritdocs/>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string)
                || destinationType == typeof(InstanceDescriptor))
            {
                return true;
            }
            // Defer to the base class
            return base.CanConvertTo(context, destinationType);
        }

        /// <inheritdocs/>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            // What is the destination type?
            if (destinationType == typeof(string))
            {
                // String.  Is the value null?
                return value.ToString();
            }

            // Defer to the base class
            return base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion
    }
}