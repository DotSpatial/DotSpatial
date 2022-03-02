#if !PocketPC || DesignTime
using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace DotSpatial.Positioning.Design
{
    /// <summary>
    /// Provides functionality to converting values to and from <strong>Speed</strong>
    /// objects.
    /// </summary>
    /// <remarks>
    /// 	<para>This class allows any <strong>Speed</strong> object to be converted between
    ///     other data types, such as <strong>String</strong>. This class is used primarily
    ///     during the Windows Forms designer to give detailed information about properties of
    ///     type <strong>Speed</strong>, and also allows developers to type in string values
    ///     such as "120 km/h" and have them converted to <strong>Speed</strong> objects
    ///     automatically. Finally, this class controls design-time serialization of
    ///     <strong>Speed</strong> object properties.</para>
    /// 	<para>In most situations this class is used by the Visual Studio.NET IDE and is
    ///     rarely created at run-time.</para>
    /// </remarks>
#if Framework20
    public sealed class SpeedConverter : GeoFrameworkObjectConverter
    {
        protected override string HandledTypeName
        {
            get
            {
                return "GeoFramework.Speed";
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

                // Get the type of the object (probably an Speed)
                Type SpeedType = value.GetType();
                // Is the value null?
                if (value == null)
                {
                    // Yes.  Return the "Empty" static value
                    return new InstanceDescriptor(SpeedType.GetField("Empty"), null);
                }
                else
                {
                    // Build the parameters for the type converter
                    object[] ConstructorParameters = new object[] 
                            {   SpeedType.GetProperty("Value").GetValue(value, null), 
                                SpeedType.GetProperty("Units").GetValue(value, null) };
                    Type[] ConstructorTypes = new Type[] 
                            { 
                                typeof(double), 
                                Type.GetType("GeoFramework.SpeedUnit") };

                    // Now activate the constructor
                    ConstructorInfo Constructor = SpeedType.GetConstructor(ConstructorTypes);
                    return new InstanceDescriptor(Constructor, ConstructorParameters);
                }
            }

            // Defer to the base class
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
#else
	public sealed class SpeedConverter : ExpandableObjectConverter
	{
		#region ConvertFrom: Converts an object to an Speed

		// Indicates if the type can be converted
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType.Equals(typeof(System.String))
				|| sourceType.Equals(typeof(Speed))
				|| sourceType.Equals(typeof(InstanceDescriptor)))
				return true;
			// Defer to the base class
			return base.CanConvertFrom(context, sourceType);
		}

		// Converts an object of one type to an Speed
		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			Type ValueType = value.GetType();
			if(ValueType.Equals(typeof(System.String)))
				return Speed.Parse((string)value, culture);
			else if(ValueType.Equals(typeof(Speed)))
				return value;
			// Defer to the base class
			return base.ConvertFrom(context, culture, value);
		}

		#endregion

		#region ConvertTo: Converts an Speed to another type

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if(destinationType.Equals(typeof(System.String))
				|| destinationType.Equals(typeof(Speed))
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
				ConstructorInfo Constructor = typeof(Speed).GetConstructor(
					new Type[] {typeof(double), typeof(SpeedUnit)});
				Speed Speed = (Speed)value;
				return new InstanceDescriptor(Constructor, new object[] { Speed.Value, Speed.Units });
			}
			// Defer to the base class
			return base.ConvertTo(context, culture, value, destinationType);
		}

		#endregion
	}
#endif
}
#endif