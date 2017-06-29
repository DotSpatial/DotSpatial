#if !PocketPC || DesignTime
using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;


namespace DotSpatial.Positioning.Design
{
    /// <summary>
    /// Provides functionality to converting values to and from <strong>Position</strong>
    /// objects.
    /// </summary>
    /// <remarks>
    /// 	<para>This class allows any <strong>Position</strong> object to be converted
    ///     between other data types, such as <strong>String</strong>. This class is used
    ///     primarily during the Windows Forms designer to give detailed information about
    ///     properties of type <strong>Position</strong>, and also allows developers to type in
    ///     string values such as "29N,42W" (for 29°North by 42°West) and have them converted
    ///     to <strong>Position</strong> objects automatically. Finally, this class controls
    ///     design-time serialization of <strong>Position</strong> object properties. UTM
    ///     coordinates such as "13S 12345E 12345N" are also accepted.</para>
    /// 	<para>In most situations this class is used by the Visual Studio.NET IDE and is
    ///     rarely created at run-time.</para>
    /// </remarks>
#if Framework20
    public sealed class PositionConverter : GeoFrameworkObjectConverter
    {
        protected override string HandledTypeName
        {
            get
            {
                return "GeoFramework.Position";
            }
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            // What is the destination type?
            if (destinationType == typeof(string) && value == null)
            {
                // String.  Is the value null?
                if (value == null)
                {
                    // Yes.  Return just a blank
                    return "0°,0°";
                }
            }
            else if (destinationType == typeof(InstanceDescriptor))
            {
                // Instance descriptor.  Used during Windows Forms serialization.

                // Get the type of the object (probably an Position)
                Type PositionType = value.GetType();
                // Is the value null?
                if (value == null)
                {
                    // Yes.  Return the "Empty" static value
                    return new InstanceDescriptor(PositionType.GetField("Empty"), null);
                }
                else
                {
                    // Build the parameters for the type converter
                    object[] ConstructorParameters = new object[] 
                            { PositionType.GetProperty("Latitude").GetValue(value, null), 
                              PositionType.GetProperty("Longitude").GetValue(value, null) };
                    Type[] ConstructorTypes = new Type[] 
                            { Type.GetType("GeoFramework.Latitude"),
                              Type.GetType("GeoFramework.Longitude") };

                    //#if DEBUG
                    //                        System.Diagnostics.EventLog.WriteEntry("SUCCESS: PositionConverter: ConvertTo: \"" + value.ToString() + "\" to New Position(" + DecimalDegrees.ToString() + ").", "");
                    //#endif

                    // Now activate the constructor
                    ConstructorInfo Constructor = PositionType.GetConstructor(ConstructorTypes);
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
	public sealed class PositionConverter : ExpandableObjectConverter
	{
		#region ConvertFrom: Converts an object to an Position

		// Indicates if the type can be converted
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType.Equals(typeof(System.String))
				|| sourceType.Equals(typeof(Position))
				|| sourceType.Equals(typeof(InstanceDescriptor)))
				return true;
			// Defer to the base class
			return base.CanConvertFrom(context, sourceType);
		}

		// Converts an object of one type to an Position
		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			Type ValueType = value.GetType();		
			if(ValueType.Equals(typeof(System.String)))
				return Position.Parse((string)value, culture);
			else if(ValueType.Equals(typeof(PolarCoordinate)))
				return value;
			// Defer to the base class
			return base.ConvertFrom(context, culture, value);
		}

		#endregion

		#region ConvertTo: Converts an Position to another type

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if(destinationType.Equals(typeof(System.String))
				|| destinationType.Equals(typeof(Position))
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
			else if(destinationType == typeof(InstanceDescriptor))
			{
				ConstructorInfo Constructor = typeof(Position).GetConstructor(
					new Type[] {typeof(Latitude), typeof(Longitude)});
				Position position = (Position)value;
				return new InstanceDescriptor(Constructor, new object[] { position.Latitude, position.Longitude });
			}
			// Defer to the base class
			return base.ConvertTo(context, culture, value, destinationType);
		}

		#endregion
	}
#endif
}
#endif