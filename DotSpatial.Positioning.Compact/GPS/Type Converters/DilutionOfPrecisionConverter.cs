#if !PocketPC || DesignTime
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System;
using System.Globalization;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

#if !Framework20
using GeoFramework.Gps;
#endif

namespace GeoFramework.Design
{
    /// <summary>
    /// Provides functionality to convert string values to and from DilutionOfPrecision objects at design time.
    /// </summary>
#if Framework20
    public sealed class DilutionOfPrecisionConverter : GpsNumericObjectConverter
    {
        protected override string HandledTypeName
        {
            get
            {
                return "GeoFramework.Gps.DilutionOfPrecision";
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

                // Get the type of the object (probably an Angle)
                Type DilutionOfPrecisionType = value.GetType();
                // Is the value null?
                if (value == null)
                {
                    // Yes.  Return the "Empty" static value
                    return new InstanceDescriptor(DilutionOfPrecisionType.GetField("Empty"), null);
                }
                else
                {
                    // Build the parameters for the type converter
                    object[] ConstructorParameters = new object[] { DilutionOfPrecisionType.GetProperty("Value").GetValue(value, null) };
                    Type[] ConstructorTypes = new Type[] { typeof(double) };

                    // Now activate the constructor
                    ConstructorInfo Constructor = DilutionOfPrecisionType.GetConstructor(ConstructorTypes);
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
		            "Ideal",
		            "Excellent",
                    "Good",
                    "Moderate",
		            "Fair",
		            "Poor"		            
		             });
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }
#else
	public class DilutionOfPrecisionConverter : ExpandableObjectConverter
	{
		#region ConvertFrom: Converts an object to an DilutionOfPrecision

		// Indicates if the type can be converted
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType.Equals(typeof(string))
				|| sourceType.Equals(typeof(double))
				|| sourceType.Equals(typeof(InstanceDescriptor))
				|| sourceType.Equals(typeof(DilutionOfPrecision)))
				return true;
			// Defer to the base class
			return base.CanConvertFrom(context, sourceType);
		}

		// Converts an object of one type to an DilutionOfPrecision
		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			Type ValueType = value.GetType();
			if (ValueType.Equals(typeof(string)))
				return new DilutionOfPrecision((string)value, culture);
			else if (ValueType.Equals(typeof(double)))
				return new DilutionOfPrecision((double)value);
			else if (ValueType.Equals(typeof(DilutionOfPrecision)))
				return value;
			// Defer to the base class
			return base.ConvertFrom(context, culture, value);
		}

		#endregion

		#region ConvertTo: Converts an DilutionOfPrecision to another type

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType.Equals(typeof(string))
				|| destinationType.Equals(typeof(double))
				|| destinationType.Equals(typeof(InstanceDescriptor))
				|| destinationType.Equals(typeof(DilutionOfPrecision)))
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
				return ((SignalToNoiseRatio)value).Value;
			else if (destinationType.Equals(typeof(SignalToNoiseRatio)))
				return value;
			else if(destinationType == typeof(InstanceDescriptor))
			{
				ConstructorInfo Constructor = typeof(DilutionOfPrecision).GetConstructor(
					new Type[] {typeof(double)});
				DilutionOfPrecision dilutionOfPrecision = (DilutionOfPrecision)value;
				return new InstanceDescriptor(Constructor, new object[] { dilutionOfPrecision.Value });
			}
			// Defer to the base class
			return base.ConvertTo(context, culture, value, destinationType);
		}

		#endregion
	}
#endif
}
#endif