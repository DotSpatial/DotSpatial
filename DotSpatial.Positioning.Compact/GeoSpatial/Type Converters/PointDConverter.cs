#if !PocketPC || DesignTime
using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;


namespace DotSpatial.Positioning.Design
{
    /// <summary>
    /// Provides functionality to converting values to and from <strong>PointD</strong>
    /// objects.
    /// </summary>
    /// <remarks>
    /// 	<para>This class allows any <strong>PointD</strong> object to be converted between
    ///     other data types, such as <strong>String</strong>. This class is used primarily
    ///     during the Windows Forms designer to give detailed information about properties of
    ///     type <strong>PointD</strong>, and also allows developers to type in string values
    ///     such as "10.123, 20.123" and have them converted to <strong>PointD</strong> objects
    ///     automatically. Finally, this class controls design-time serialization of
    ///     <strong>PointD</strong> object properties.</para>
    /// 	<para>In most situations this class is used by the Visual Studio.NET IDE and is
    ///     rarely created at run-time.</para>
    /// </remarks>
#if Framework20
    public sealed class PointDConverter : GeoFrameworkObjectConverter
    {
        protected override string HandledTypeName
        {
            get
            {
                return "GeoFramework.PointD";
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

                // Get the type of the object (probably an PointD)
                Type PointDType = value.GetType();
                // Is the value null?
                if (value == null)
                {
                    // Yes.  Return the "Empty" static value
                    return new InstanceDescriptor(PointDType.GetField("Empty"), null);
                }
                else
                {
                    // Get the properties needed to generate a constructor
                    object[] ConstructorParameters = new object[] 
                            { PointDType.GetProperty("X").GetValue(value, null),
                              PointDType.GetProperty("Y").GetValue(value, null) };
                    Type[] ConstructorTypes = new Type[] { typeof(double), typeof(double) };

                    //#if DEBUG
                    //                        System.Diagnostics.EventLog.WriteEntry("SUCCESS: PointDConverter: ConvertTo: \"" + value.ToString() + "\" to New PointD(" + DecimalDegrees.ToString() + ").", "");
                    //#endif

                    // Now activate the constructor
                    ConstructorInfo Constructor = PointDType.GetConstructor(ConstructorTypes);
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
	public sealed class PointDConverter : ExpandableObjectConverter
	{
		#region ConvertFrom: Converts an object to an PointD

		// Indicates if the type can be converted
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType.Equals(typeof(System.String))
				|| sourceType.Equals(typeof(PointD))
				|| sourceType.Equals(typeof(PointF))
				|| sourceType.Equals(typeof(Point))
				|| sourceType.Equals(typeof(InstanceDescriptor)))
				return true;
			// Defer to the base class
			return base.CanConvertFrom(context, sourceType);
		}

		// Converts an object of one type to an PointD
		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			Type ValueType = value.GetType();
			if(ValueType.Equals(typeof(System.String)))
				return PointD.Parse((string)value, culture);
			else if(ValueType.Equals(typeof(PointD)))
				return value;
			else if(ValueType.Equals(typeof(PointF)))
				return (PointD)value;
			else if(ValueType.Equals(typeof(Point)))
				return (PointD)value;
			// Defer to the base class
			return base.ConvertFrom(context, culture, value);
		}

		#endregion

		#region ConvertTo: Converts an PointD to another type

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if(destinationType.Equals(typeof(System.String))
				|| destinationType.Equals(typeof(InstanceDescriptor))
				|| destinationType.Equals(typeof(PointD))
				|| destinationType.Equals(typeof(PointF))
				|| destinationType.Equals(typeof(Point)))
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
			else if(destinationType.Equals(typeof(PointF)))
			{
				PointD TheValue = (PointD)value;
				return new PointF((float)TheValue.X, (float)TheValue.Y);
			}
			else if(destinationType.Equals(typeof(Point)))
			{
				PointD TheValue = (PointD)value;
				return new Point((int)TheValue.X, (int)TheValue.Y);
			}
			else if(destinationType == typeof(InstanceDescriptor))
			{
				ConstructorInfo Constructor = typeof(PointD).GetConstructor(
					new Type[] {typeof(double), typeof(double)});
				PointD point = (PointD)value;
				return new InstanceDescriptor(Constructor, new object[] { point.X, point.Y });
			}
			// Defer to the base class
			return base.ConvertTo(context, culture, value, destinationType);
		}

		#endregion
	}
#endif
    
}
#endif