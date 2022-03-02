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
    /// Provides functionality to convert string values to and from SignalToNoiseRatio objects at design time.
    /// </summary>
#if Framework20
    public sealed class SignalToNoiseRatioConverter : GpsNumericObjectConverter
    {
        protected override string HandledTypeName
        {
            get
            {
                return "GeoFramework.Gps.SignalToNoiseRatio";
            }
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            // What is the destination type?
            if (destinationType == typeof(string) && value == null)
            {
                return "0";
            }
            else if (destinationType == typeof(InstanceDescriptor))
            {
                // Instance descriptor.  Used during Windows Forms serialization.

                // Get the type of the object (probably an SignalToNoiseRatio)
                Type SignalToNoiseRatioType = value.GetType();
                // Is the value null?
                if (value == null)
                {
                    // Yes.  Return the "Empty" static value
                    return new InstanceDescriptor(SignalToNoiseRatioType.GetField("Empty"), null);
                }
                else
                {
                    // Build the parameters for the type converter
                    object[] ConstructorParameters = new object[] { SignalToNoiseRatioType.GetProperty("Value").GetValue(value, null) };
                    Type[] ConstructorTypes = new Type[] { typeof(double) };

                    // Now activate the constructor
                    ConstructorInfo Constructor = SignalToNoiseRatioType.GetConstructor(ConstructorTypes);
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
		            "FullSignal",
		            "HalfSignal",
                    "NoSignal"	            
		             });
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }
#else
	public sealed class SignalToNoiseRatioConverter : ExpandableObjectConverter
	{
		#region ConvertFrom: Converts an object to an SignalToNoiseRatio

		// Indicates if the type can be converted
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType.Equals(typeof(string))
				|| sourceType.Equals(typeof(int))
				|| sourceType.Equals(typeof(InstanceDescriptor))
				|| sourceType.Equals(typeof(SignalToNoiseRatio)))
				return true;
			// Defer to the base class
			return base.CanConvertFrom(context, sourceType);
		}

		// Converts an object of one type to an SignalToNoiseRatio
		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			Type ValueType = value.GetType();
			if (ValueType.Equals(typeof(string)))
				return new SignalToNoiseRatio((string)value, culture);
			else if (ValueType.Equals(typeof(double)))
				return new SignalToNoiseRatio((int)value);
			else if (ValueType.Equals(typeof(SignalToNoiseRatio)))
				return value;
			// Defer to the base class
			return base.ConvertFrom(context, culture, value);
		}

		#endregion

		#region ConvertTo: Converts an SignalToNoiseRatio to another type

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType.Equals(typeof(string))
				|| destinationType.Equals(typeof(int))
				|| destinationType.Equals(typeof(InstanceDescriptor))
				|| destinationType.Equals(typeof(SignalToNoiseRatio)))
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
			else if (destinationType.Equals(typeof(int)))
				return ((SignalToNoiseRatio)value).Value;
			else if (destinationType.Equals(typeof(SignalToNoiseRatio)))
				return value;
			if (destinationType == typeof(InstanceDescriptor))
			{
				ConstructorInfo Constructor = typeof(SignalToNoiseRatio).GetConstructor(
					new Type[] { typeof(int) });
				SignalToNoiseRatio SignalToNoiseRatio = (SignalToNoiseRatio)value;
				return new InstanceDescriptor(Constructor, new object[] { SignalToNoiseRatio.Value });
			}
			// Defer to the base class
			return base.ConvertTo(context, culture, value, destinationType);
		}

		#endregion
	}
#endif
}
#endif