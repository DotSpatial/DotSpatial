#if !PocketPC || DesignTime
using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace DotSpatial.Positioning.Design
{
	/// <summary>
	/// Provides functionality to convert string values to and from Longitude objects at design time.
	/// </summary>
	/// <remarks>
	/// 	<para>This class allows any <strong>Longitude</strong> object to be converted
	///     between other data types, such as <strong>Double</strong>, <strong>Integer</strong>
	///     and <strong>String</strong>. This class is used primarily during the Windows Forms
	///     designer to give detailed information about properties of type
	///     <strong>Longitude</strong>, and also allows developers to type in string values
	///     such as "54E" (for 54° East) and have them converted to <strong>Longitude</strong>
	///     objects automatically. Finally, this class controls design-time serialization of
	///     <strong>Longitude</strong> object properties.</para>
	/// 	<para>In most situations this class is used by the Visual Studio.NET IDE and is
	///     rarely created at run-time.</para>
	/// </remarks>
#if Framework20
    public sealed class LongitudeConverter : GeoFrameworkNumericObjectConverter
    {
        protected override string HandledTypeName
        {
            get
            {
                return "GeoFramework.Longitude";
            }
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
                // What is the destination type?
                if (destinationType == typeof(string) && value == null)
                {
                        return "0°";
                    }
                else if (destinationType == typeof(InstanceDescriptor))
                {
                    // Instance descriptor.  Used during Windows Forms serialization.

                    // Get the type of the object (probably an Longitude)
                    Type LongitudeType = value.GetType();
                    // Is the value null?
                    if (value == null)
                    {
                        // Yes.  Return the "Empty" static value
                        return new InstanceDescriptor(LongitudeType.GetField("Empty"), null);
                    }
                    else
                    {
                        // Get the properties needed to generate a constructor
                        object[] ConstructorParameters = new object[] { LongitudeType.GetProperty("DecimalDegrees").GetValue(value, null) };
                        Type[] ConstructorTypes = new Type[] { typeof(double) };

                        // Now activate the constructor
                        ConstructorInfo Constructor = LongitudeType.GetConstructor(ConstructorTypes);
                        return new InstanceDescriptor(Constructor, ConstructorParameters);
                    }
                }

                // Defer to the base class
                return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] {
		            "Equator",
		            "NorthPole",
		            "SouthPole",
		            "TropicOfCapricorn",
		            "TropicOfCancer" });
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }
#else
	public sealed class LongitudeConverter : ExpandableObjectConverter
	{
    #region ConvertFrom: Converts an object to an Longitude

		// Indicates if the type can be converted
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType.Equals(typeof(string))
				|| sourceType.Equals(typeof(double))
				|| sourceType.Equals(typeof(InstanceDescriptor))
				|| sourceType.Equals(typeof(Longitude)))
				return true;
			// Defer to the base class
			return base.CanConvertFrom(context, sourceType);
		}

		// Converts an object of one type to an Longitude
		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			Type ValueType = value.GetType();
			if (ValueType.Equals(typeof(string)))
				return new Longitude((string)value, culture);
			else if (ValueType.Equals(typeof(double)))
				return new Longitude((double)value);
			else if (ValueType.Equals(typeof(Longitude)))
				return value;
			// Defer to the base class
			return base.ConvertFrom(context, culture, value);
		}

        #endregion

    #region ConvertTo: Converts a Longitude to another type

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType.Equals(typeof(string))
				|| destinationType.Equals(typeof(double))
				|| destinationType.Equals(typeof(InstanceDescriptor))
				|| destinationType.Equals(typeof(Longitude)))
				return true;
			// Defer to the base class
			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			if(destinationType.Equals(value.GetType()))
				return value;
			else if (destinationType.Equals(typeof(string)))
				return value.ToString();
			else if (destinationType.Equals(typeof(double)))
				return ((Longitude)value).DecimalDegrees;
			else if (destinationType.Equals(typeof(Longitude)))
				return value;
			else if(destinationType.Equals(typeof(InstanceDescriptor)))
			{
				ConstructorInfo Constructor = typeof(Longitude).GetConstructor(
					new Type[] {typeof(double)});
				Longitude LongitudeValue = (Longitude)value;
				return new InstanceDescriptor(Constructor, new object[] { LongitudeValue.DecimalDegrees });
			}			
			// Defer to the base class
			return base.ConvertTo(context, culture, value, destinationType);
		}

        #endregion
	}
#endif
}
#endif