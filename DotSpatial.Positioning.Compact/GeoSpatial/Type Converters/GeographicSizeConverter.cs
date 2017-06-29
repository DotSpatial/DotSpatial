#if !PocketPC || DesignTime
using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace DotSpatial.Positioning.Design
{
    /// <summary>
    /// Provides functionality to converting values to and from
    /// <strong>GeographicSize</strong> objects.
    /// </summary>
    /// <remarks>
    /// 	<para>This class allows any <strong>GeographicSize</strong> object to be converted
    ///     between other data types, such as <strong>String</strong>. This class is used
    ///     primarily during the Windows Forms designer to give detailed information about
    ///     properties of type <strong>GeographicSize</strong>, and also allows developers to
    ///     type in string values such as "1,2" and have them converted to
    ///     <strong>GeographicSize</strong> objects automatically. Finally, this class controls
    ///     design-time serialization of <strong>GeographicSize</strong> object
    ///     properties.</para>
    /// 	<para>In most situations this class is used by the Visual Studio.NET IDE and is
    ///     rarely created at run-time.</para>
    /// </remarks>
#if Framework20
    public sealed class GeographicSizeConverter : GeoFrameworkObjectConverter
    {
        protected override string HandledTypeName
        {
            get
            {
                return "GeoFramework.GeographicSize";
            }
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            // What is the destination type?
            if (destinationType == typeof(string) && value == null)
            {
                // Yes.  Return just a blank
                return "0°,0°";
            }
            else if (destinationType == typeof(InstanceDescriptor))
            {
                // Instance descriptor.  Used during Windows Forms serialization.

                // Get the type of the object (probably an GeographicSize)
                Type GeographicSizeType = value.GetType();
                // Is the value null?
                if (value == null)
                {
                    // Yes.  Return the "Empty" static value
                    return new InstanceDescriptor(GeographicSizeType.GetField("Empty"), null);
                }
                else
                {
                    // Build the parameters for the type converter
                    object[] ConstructorParameters = new object[] 
                            { GeographicSizeType.GetProperty("Width").GetValue(value, null), 
                              GeographicSizeType.GetProperty("Height").GetValue(value, null) };
                    Type[] ConstructorTypes = new Type[] 
                            { Type.GetType("GeoFramework.Longitude"),
                              Type.GetType("GeoFramework.Latitude") };

                    //#if DEBUG
                    //                        System.Diagnostics.EventLog.WriteEntry("SUCCESS: GeographicSizeConverter: ConvertTo: \"" + value.ToString() + "\" to New GeographicSize(" + DecimalDegrees.ToString() + ").", "");
                    //#endif

                    // Now activate the constructor
                    ConstructorInfo Constructor = GeographicSizeType.GetConstructor(ConstructorTypes);
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
		            "Empty",
		            "Maximum",
		            "Minimum" });
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

    }
#else
	public sealed class GeographicSizeConverter : ExpandableObjectConverter
	{
		#region ConvertFrom: Converts an object to an GeographicSize

		// Indicates if the type can be converted
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType.Equals(typeof(System.String))
				|| sourceType.Equals(typeof(GeographicSize))
				|| sourceType.Equals(typeof(InstanceDescriptor)))
				return true;
			// Defer to the base class
			return base.CanConvertFrom(context, sourceType);
		}

		// Converts an object of one type to an GeographicSize
		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			Type ValueType = value.GetType();
			if(ValueType.Equals(typeof(System.String)))
				return GeographicSize.Parse((string)value, culture);
			else if(ValueType.Equals(typeof(GeographicSize)))
				return value;
			// Defer to the base class
			return base.ConvertFrom(context, culture, value);
		}

		#endregion

		#region ConvertTo: Converts an GeographicSize to another type

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if(destinationType.Equals(typeof(System.String))
				|| destinationType.Equals(typeof(GeographicSize))
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
				// An InstanceDescriptor, used by the Form Designer
				ConstructorInfo Constructor = typeof(GeographicSize).GetConstructor(
					new Type[] {typeof(Angle), typeof(Angle)});
				GeographicSize size = (GeographicSize)value;
				return new InstanceDescriptor(Constructor, new object[] { size.Width, size.Height });
			}
			// Defer to the base class
			return base.ConvertTo(context, culture, value, destinationType);
		}

		#endregion
	}
#endif
}
#endif