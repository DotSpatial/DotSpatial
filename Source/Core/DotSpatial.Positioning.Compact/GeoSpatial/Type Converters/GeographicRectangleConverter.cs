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
    /// <strong>GeographicRectangle</strong> objects.
    /// </summary>
    /// <remarks>
    /// 	<para>This class allows any <strong>GeographicRectangle</strong> object to be
    ///     converted between other data types, such as <strong>String</strong>. This class is
    ///     used primarily during the Windows Forms designer to give detailed information about
    ///     properties of type <strong>GeographicRectangle</strong>, and also allows developers
    ///     to type in string values such as "1,2,10,10" and have them converted to
    ///     <strong>GeographicRectangle</strong> objects automatically. Finally, this class
    ///     controls design-time serialization of <strong>GeographicRectangle</strong> object
    ///     properties.</para>
    /// 	<para>In most situations this class is used by the Visual Studio.NET IDE and is
    ///     rarely created at run-time.</para>
    /// </remarks>
#if Framework20
    public sealed class GeographicRectangleConverter : GeoFrameworkObjectConverter
    {
        protected override string HandledTypeName
        {
            get { return "GeoFramework.GeographicRectangle"; }
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            // What is the destination type?
            if (destinationType == typeof(string) && value == null)
            {
                return "0°,0°,0°,0°";
            }
            else if (destinationType == typeof(InstanceDescriptor))
            {
                // Instance descriptor.  Used during Windows Forms serialization.

                // Get the type of the object (probably an GeographicRectangle)
                Type GeographicRectangleType = value.GetType();
                // Is the value null?
                if (value == null)
                {
                    // Yes.  Return the "Empty" static value
                    return new InstanceDescriptor(GeographicRectangleType.GetField("Empty"), null);
                }
                else
                {
                    // Build the parameters for the type converter
                    object[] ConstructorParameters = new object[] 
                            { GeographicRectangleType.GetProperty("Top").GetValue(value, null), 
                              GeographicRectangleType.GetProperty("Left").GetValue(value, null),
                              GeographicRectangleType.GetProperty("Bottom").GetValue(value, null), 
                              GeographicRectangleType.GetProperty("Right").GetValue(value, null)};
                    Type[] ConstructorTypes = new Type[] 
                            { Type.GetType("GeoFramework.Latitude"),
                              Type.GetType("GeoFramework.Longitude"),
                              Type.GetType("GeoFramework.Latitude"),
                              Type.GetType("GeoFramework.Longitude") };

                    //#if DEBUG
                    //                        System.Diagnostics.EventLog.WriteEntry("SUCCESS: GeographicRectangleConverter: ConvertTo: \"" + value.ToString() + "\" to New GeographicRectangle(" + DecimalDegrees.ToString() + ").", "");
                    //#endif

                    // Now activate the constructor
                    ConstructorInfo Constructor = GeographicRectangleType.GetConstructor(ConstructorTypes);
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
	public sealed class GeographicRectangleConverter : ExpandableObjectConverter
	{
		#region ConvertFrom: Converts an object to an GeographicRectangle

		// Indicates if the type can be converted
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType.Equals(typeof(System.String))
				|| sourceType.Equals(typeof(GeographicRectangle))
				|| sourceType.Equals(typeof(InstanceDescriptor)))
				return true;

			// Defer to the base class
			return base.CanConvertFrom(context, sourceType);
		}

		// Converts an object of one type to an GeographicRectangle
		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			Type ValueType = value.GetType();
			if (ValueType.Equals(typeof(System.String)))
				return GeographicRectangle.Parse((string)value, culture);
			else if(ValueType.Equals(typeof(GeographicRectangle)))
				return value;
			// Defer to the base class
			return base.ConvertFrom(context, culture, value);
		}

		#endregion

		#region ConvertTo: Converts an GeographicRectangle to another type

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if(destinationType.Equals(typeof(System.String))
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
				ConstructorInfo Constructor = typeof(GeographicRectangle).GetConstructor(
					new Type[] {typeof(Longitude), typeof(Latitude), typeof(Longitude), typeof(Latitude)});
				GeographicRectangle rectangle = (GeographicRectangle)value;
				return new InstanceDescriptor(Constructor, new object[] { rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom });
			}
			// Defer to the base class
			return base.ConvertTo(context, culture, value, destinationType);
		}

		#endregion
	}
#endif
}
#endif