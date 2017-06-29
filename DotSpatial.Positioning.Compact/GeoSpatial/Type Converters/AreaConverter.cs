#if !PocketPC || DesignTime
using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace DotSpatial.Positioning.Design
{
	/// <summary>
	/// Provides functionality to convert string values to and from Area objects at design time.
	/// </summary>
#if Framework20
    public sealed class AreaConverter : GeoFrameworkObjectConverter
    {
        protected override string HandledTypeName
        {
            get 
            { 
                return "GeoFramework.Area"; 
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

                    // Get the type of the object (probably an Area)
                    Type AreaType = value.GetType();
                    // Is the value null?
                    if (value == null)
                    {
                        // Yes.  Return the "Empty" static value
                        return new InstanceDescriptor(AreaType.GetField("Empty"), null);
                    }
                    else
                    {
                        // Build the parameters for the type converter
                        object[] ConstructorParameters = new object[] 
                            {   AreaType.GetProperty("Value").GetValue(value, null), 
                                AreaType.GetProperty("Units").GetValue(value, null) };
                        Type[] ConstructorTypes = new Type[] { typeof(double), Type.GetType("GeoFramework.AreaUnit") };

                        // Now activate the constructor
                        ConstructorInfo Constructor = AreaType.GetConstructor(ConstructorTypes);
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
		            "Infinity",
		            "SeaLevel" });
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }
#else
	public sealed class AreaConverter : ExpandableObjectConverter
	{
		#region ConvertFrom: Converts an object to an Area

		// Indicates if the type can be converted
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType.Equals(typeof(System.String))
				|| sourceType.Equals(typeof(InstanceDescriptor))
                || sourceType.Equals(typeof(Area)))
                return true;
            // Defer to the base class
            return base.CanConvertFrom(context, sourceType);
		}

		// Converts an object of one type to an Area
		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			Type ValueType = value.GetType();
			if(ValueType.Equals(typeof(System.String)))
				return Area.Parse((string)value, culture);
			else if(ValueType.Equals(typeof(Area)))
				return value;
			// Defer to the base class
			return base.ConvertFrom(context, culture, value);
		}

		#endregion

		#region ConvertTo: Converts an Area to another type

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
				ConstructorInfo Constructor = typeof(Area).GetConstructor(
					new Type[] {typeof(double), typeof(AreaUnit)});
				Area Area = (Area)value;
				return new InstanceDescriptor(Constructor, new object[] { Area.Value, Area.Units });
			}
			// Defer to the base class
			return base.ConvertTo(context, culture, value, destinationType);
		}

		#endregion
	}
#endif
}

#endif