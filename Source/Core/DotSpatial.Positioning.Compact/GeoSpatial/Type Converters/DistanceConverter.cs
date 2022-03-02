#if !PocketPC || DesignTime
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
#if Framework20
    public sealed class DistanceConverter : GeoFrameworkObjectConverter
    {
        protected override string HandledTypeName
        {
            get 
            {
                return "GeoFramework.Distance";
            }
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            // What is the destination type?
            if (destinationType == typeof(string) && value == null)
            {
                return "0 cm";
            }
            else if (destinationType == typeof(InstanceDescriptor))
            {
                // Instance descriptor.  Used during Windows Forms serialization.

                // Get the type of the object (probably an Distance)
                Type DistanceType = value.GetType();
                // Is the value null?
                if (value == null)
                {
                    // Yes.  Return the "Empty" static value
                    return new InstanceDescriptor(DistanceType.GetField("Empty"), null);
                }
                else
                {
                    // Build the parameters for the type converter
                    object[] ConstructorParameters = new object[] 
                            {   DistanceType.GetProperty("Value").GetValue(value, null), 
                                DistanceType.GetProperty("Units").GetValue(value, null) };
                    Type[] ConstructorTypes = new Type[] { typeof(double), Type.GetType("GeoFramework.DistanceUnit") };

                    // Now activate the constructor
                    ConstructorInfo Constructor = DistanceType.GetConstructor(ConstructorTypes);
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
            // Are we currently in metric or imperial?
            if (RegionInfo.CurrentRegion.IsMetric)
            {
                return new StandardValuesCollection(new string[] {
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
            else
            {
                return new StandardValuesCollection(new string[] {
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
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }
#else
	public sealed class DistanceConverter : ExpandableObjectConverter
	{
		#region ConvertFrom: Converts an object to an Distance

		// Indicates if the type can be converted
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType.Equals(typeof(System.String))
				|| sourceType.Equals(typeof(Distance))
				|| sourceType.Equals(typeof(InstanceDescriptor)))
				return true;
			// Defer to the base class
			return base.CanConvertFrom(context, sourceType);
		}

		// Converts an object of one type to an Distance
		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			Type ValueType = value.GetType();
			if(ValueType.Equals(typeof(System.String)))
				return Distance.Parse((string)value, culture);
			else if(ValueType.Equals(typeof(Distance)))
				return value;
			// Defer to the base class
			return base.ConvertFrom(context, culture, value);
		}

		#endregion

		#region ConvertTo: Converts an Distance to another type

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if(destinationType.Equals(typeof(System.String))
				|| destinationType.Equals(typeof(Distance))
				|| destinationType.Equals(typeof(InstanceDescriptor)))
				return true;
            
			// Defer to the base class
			return base.CanConvertTo(context, destinationType);
		}


		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			if(destinationType.Equals(value.GetType()))
				return value;
			else if(destinationType.Equals(typeof(System.String)))
				return value.ToString();
			else if(destinationType.Equals(typeof(InstanceDescriptor)))
			{
				ConstructorInfo Constructor = typeof(Distance).GetConstructor(
					new Type[] {typeof(double), typeof(DistanceUnit)});
				Distance Distance = (Distance)value;
				return new InstanceDescriptor(Constructor, new object[] { Distance.Value, Distance.Units });
			}
			// Defer to the base class
			return base.ConvertTo(context, culture, value, destinationType);
		}

		#endregion
	}
#endif
}
#endif