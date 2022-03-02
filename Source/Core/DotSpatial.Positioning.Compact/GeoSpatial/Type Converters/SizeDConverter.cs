#if !PocketPC || DesignTime
using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;


namespace DotSpatial.Positioning.Design
{
    /// <summary>
    /// Provides functionality to converting values to and from <strong>SizeD</strong>
    /// objects.
    /// </summary>
    /// <remarks>
    /// 	<para>This class allows any <strong>Position</strong> object to be converted
    ///     between other data types, such as <strong>String</strong>. This class is used
    ///     primarily during the Windows Forms designer to give detailed information about
    ///     properties of type <strong>Position</strong>, and also allows developers to type in
    ///     string values such as "29.23,15.65" and have them converted to
    ///     <strong>Position</strong> objects automatically. Finally, this class controls
    ///     design-time serialization of <strong>Position</strong> object properties.</para>
    /// 	<para>In most situations this class is used by the Visual Studio.NET IDE and is
    ///     rarely created at run-time.</para>
    /// </remarks>
#if Framework20
    public sealed class SizeDConverter : GeoFrameworkObjectConverter
    {
        protected override string HandledTypeName
        {
            get
            {
                return "GeoFramework.SizeD";
            }
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            // What is the destination type?
            if (destinationType == typeof(string) && value == null)
            {
                return "0.0,0.0";
            }
            else if (destinationType == typeof(InstanceDescriptor))
            {
                // Instance descriptor.  Used during Windows Forms serialization.

                // Get the type of the object (probably an SizeD)
                Type SizeDType = value.GetType();
                // Is the value null?
                if (value == null)
                {
                    // Yes.  Return the "Empty" static value
                    return new InstanceDescriptor(SizeDType.GetField("Empty"), null);
                }
                else
                {
                    // Get the properties needed to generate a constructor
                    object[] ConstructorParameters = new object[] 
                            { SizeDType.GetProperty("Width").GetValue(value, null),
                              SizeDType.GetProperty("Height").GetValue(value, null) };
                    Type[] ConstructorTypes = new Type[] { typeof(double), typeof(double) };

                    // Now activate the constructor
                    ConstructorInfo Constructor = SizeDType.GetConstructor(ConstructorTypes);
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
		            "Invalid" });
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }
#else
	public sealed class SizeDConverter : ExpandableObjectConverter
	{
		#region ConvertFrom: Converts an object to an SizeD

		// Indicates if the type can be converted
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType.Equals(typeof(System.String))
				|| sourceType.Equals(typeof(InstanceDescriptor))
				|| sourceType.Equals(typeof(SizeD))
				|| sourceType.Equals(typeof(SizeF))
				|| sourceType.Equals(typeof(Size)))
				return true;
			// Defer to the base class
			return base.CanConvertFrom(context, sourceType);
		}

		// Converts an object of one type to an SizeD
		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			Type ValueType = value.GetType();		
			if(ValueType.Equals(typeof(System.String)))
				return SizeD.Parse((string)value, culture);
			else if(ValueType.Equals(typeof(SizeD)))
				return value;
			else if(ValueType.Equals(typeof(SizeF)))
				return (PointD)value;
			else if(ValueType.Equals(typeof(Size)))
				return (PointD)value;
			// Defer to the base class
			return base.ConvertFrom(context, culture, value);
		}

		#endregion

		#region ConvertTo: Converts an SizeD to another type

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if(destinationType.Equals(typeof(System.String))
				|| destinationType.Equals(typeof(SizeF))
				|| destinationType.Equals(typeof(Size))
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
			else if(destinationType.Equals(typeof(SizeF)))
			{
				SizeD TheValue = (SizeD)value;
				return new SizeF((float)TheValue.Width, (float)TheValue.Height);
			}
			else if(destinationType.Equals(typeof(Size)))
			{
				SizeD TheValue = (SizeD)value;
				return new Size((int)TheValue.Width, (int)TheValue.Height);
			}			
			else if(destinationType == typeof(InstanceDescriptor))
			{
				ConstructorInfo Constructor = typeof(SizeD).GetConstructor(
					new Type[] {typeof(double), typeof(double)});
				SizeD size = (SizeD)value;
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