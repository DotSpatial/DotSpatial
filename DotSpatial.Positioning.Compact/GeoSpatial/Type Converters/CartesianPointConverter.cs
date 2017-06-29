#if !PocketPC || DesignTime
using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;


namespace DotSpatial.Positioning.Design
{
    /// <summary>
    /// Provides functionality to converting values to and from <strong>CartesianPoint</strong>
    /// objects.
    /// </summary>
    /// <remarks>
    /// 	<para>This class allows any <strong>CartesianPoint</strong> object to be converted between
    ///     other data types, such as <strong>String</strong>. This class is used primarily
    ///     during the Windows Forms designer to give detailed information about properties of
    ///     type <strong>CartesianPoint</strong>, and also allows developers to type in string values
    ///     such as "10m, 20m, 100m" and have them converted to <strong>CartesianPoint</strong> objects
    ///     automatically. Finally, this class controls design-time serialization of
    ///     <strong>CartesianPoint</strong> object properties.</para>
    /// 	<para>In most situations this class is used by the Visual Studio.NET IDE and is
    ///     rarely created at run-time.</para>
    /// </remarks>
#if Framework20
    public sealed class CartesianPointConverter : GeoFrameworkObjectConverter
    {
        protected override string HandledTypeName
        {
            get
            {
                return "GeoFramework.CartesianPoint";
            }
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            // What is the destination type?
            if (destinationType == typeof(string) && value == null)
            {
                return "0 m, 0 m, 0 m";
            }
            else if (destinationType == typeof(InstanceDescriptor))
            {
                // Instance descriptor.  Used during Windows Forms serialization.

                // Get the type of the object (probably an CartesianPoint)
                Type CartesianPointType = value.GetType();
                // Is the value null?
                if (value == null)
                {
                    // Yes.  Return the "Empty" static value
#if DEBUG
                        System.Diagnostics.EventLog.WriteEntry("SUCCESS: CartesianPointConverter: ConvertTo: Null to CartesianPoint.Empty", "");
#endif
                    return new InstanceDescriptor(CartesianPointType.GetField("Empty"), null);
                }
                else
                {
                    // Get the properties needed to generate a constructor
                    object[] ConstructorParameters = new object[] 
                            { CartesianPointType.GetProperty("X").GetValue(value, null),
                              CartesianPointType.GetProperty("Y").GetValue(value, null),
                              CartesianPointType.GetProperty("Z").GetValue(value, null) };
                    Type[] ConstructorTypes = new Type[] 
                            { Type.GetType("GeoFramework.Distance"),
                              Type.GetType("GeoFramework.Distance"),
                              Type.GetType("GeoFramework.Distance") };

                    // Now activate the constructor
                    ConstructorInfo Constructor = CartesianPointType.GetConstructor(ConstructorTypes);
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
		            "Empty" });
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }
#else
	public sealed class CartesianPointConverter : ExpandableObjectConverter
	{
		#region ConvertFrom: Converts an object to an CartesianPoint

		// Indicates if the type can be converted
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType.Equals(typeof(System.String))
				|| sourceType.Equals(typeof(InstanceDescriptor))
				|| sourceType.Equals(typeof(CartesianPoint)))
				return true;
			// Defer to the base class
			return base.CanConvertFrom(context, sourceType);
		}

		// Converts an object of one type to an CartesianPoint
		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			Type ValueType = value.GetType();
			if(ValueType.Equals(typeof(CartesianPoint)))
				return value;
			// Defer to the base class
			return base.ConvertFrom(context, culture, value);
		}

		#endregion

		#region ConvertTo: Converts an CartesianPoint to another type

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if(destinationType.Equals(typeof(System.String))
				|| destinationType.Equals(typeof(InstanceDescriptor))
				|| destinationType.Equals(typeof(CartesianPoint)))
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
				CartesianPoint CartesianPointValue = (CartesianPoint)value;
				ConstructorInfo Constructor = typeof(CartesianPoint).GetConstructor(
					new Type[] {typeof(Distance), typeof(Distance), typeof(Distance) });
				return new InstanceDescriptor(Constructor, new object[] 
					{ CartesianPointValue.X, CartesianPointValue.Y, CartesianPointValue.Z });
			}			
			// Defer to the base class
			return base.ConvertTo(context, culture, value, destinationType);
		}

		#endregion
	}
#endif
}
#endif
