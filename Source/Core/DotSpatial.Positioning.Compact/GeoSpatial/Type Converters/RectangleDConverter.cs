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
	/// <strong>RectangleD</strong> objects.
	/// </summary>
	/// <remarks>
	/// 	<para>This class allows any <strong>RectangleD</strong> object to be converted
	///     between other data types, such as <strong>String</strong>. This class is used
	///     primarily during the Windows Forms designer to give detailed information about
	///     properties of type <strong>RectangleD</strong>, and also allows developers to type
	///     in string values such as "1.5,2.5,4.5,7.8" and have them converted to
	///     <strong>RectangleD</strong> objects automatically. Finally, this class controls
	///     design-time serialization of <strong>RectangleD</strong> object properties.</para>
	/// 	<para>In most situations this class is used by the Visual Studio.NET IDE and is
	///     rarely created at run-time.</para>
	/// </remarks>
#if Framework20
    public sealed class RectangleDConverter : GeoFrameworkObjectConverter 
    {
        protected override string HandledTypeName
        {
            get
            {
                return "GeoFramework.RectangleD";
            }
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
                // What is the destination type?
                if (destinationType == typeof(string) && value == null)
                {
                        return "0.0, 0.0, 0.0, 0.0";
                    }
                else if (destinationType == typeof(InstanceDescriptor))
                {
                    // Instance descriptor.  Used during Windows Forms serialization.

                    // Get the type of the object (probably an RectangleD)
                    Type RectangleDType = value.GetType();
                    // Is the value null?
                    if (value == null)
                    {
                        // Yes.  Return the "Empty" static value
                        return new InstanceDescriptor(RectangleDType.GetField("Empty"), null);
                    }
                    else
                    {
                        // Get the properties needed to generate a constructor
                        object[] ConstructorParameters = new object[] 
                            { RectangleDType.GetProperty("Left").GetValue(value, null),
                              RectangleDType.GetProperty("Top").GetValue(value, null),
                              RectangleDType.GetProperty("Right").GetValue(value, null),
                              RectangleDType.GetProperty("Bottom").GetValue(value, null) };
                        Type[] ConstructorTypes = new Type[] 
                            { typeof(double), 
                              typeof(double),
                              typeof(double), 
                              typeof(double) };

                        // Now activate the constructor
                        ConstructorInfo Constructor = RectangleDType.GetConstructor(ConstructorTypes);
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
	public sealed class RectangleDConverter : ExpandableObjectConverter
	{
		#region ConvertFrom: Converts an object to an RectangleD

		// Indicates if the type can be converted
		/// <summary>
		/// Returns of an object can be converted into a <strong>Speed</strong>
		/// object.
		/// </summary>
		/// <param name="sourceType">
		/// The source type to be converted, typically a <strong>String</strong> or
		/// <strong>InstanceDescriptor</strong> object.
		/// </param>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType.Equals(typeof(System.String))
				|| sourceType.Equals(typeof(RectangleD))
				|| sourceType.Equals(typeof(RectangleF))
				|| sourceType.Equals(typeof(Rectangle))
				|| sourceType.Equals(typeof(InstanceDescriptor)))
				return true;

			// Defer to the base class
			return base.CanConvertFrom(context, sourceType);
		}

		// Converts an object of one type to an RectangleD
		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			Type ValueType = value.GetType();		
			if (ValueType.Equals(typeof(System.String)))
				return RectangleD.Parse((string)value, culture);
			else if (ValueType.Equals(typeof(RectangleD)))
				return value;
			else if (ValueType.Equals(typeof(RectangleF)))
				return (RectangleD)value;
			else if (ValueType.Equals(typeof(Rectangle)))
				return (RectangleD)value;
			// Defer to the base class
			return base.ConvertFrom(context, culture, value);
		}

		#endregion

		#region ConvertTo: Converts an RectangleD to another type

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if(destinationType.Equals(typeof(System.String))
				|| destinationType.Equals(typeof(RectangleD))
				|| destinationType.Equals(typeof(RectangleF))
				|| destinationType.Equals(typeof(Rectangle))
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
			else if(destinationType.Equals(typeof(RectangleF)))
			{
				RectangleD TheValue = (RectangleD)value;
				return new RectangleF((float)TheValue.Left, (float)TheValue.Top, (float)TheValue.Width, (float)TheValue.Height);
			}
			else if(destinationType.Equals(typeof(Rectangle)))
			{
				RectangleD TheValue = (RectangleD)value;
				return new Rectangle((int)TheValue.Left, (int)TheValue.Top, (int)TheValue.Width, (int)TheValue.Height);
			}			
			else if(destinationType == typeof(InstanceDescriptor))
			{
				ConstructorInfo Constructor = typeof(RectangleD).GetConstructor(
					new Type[] {typeof(double), typeof(double), typeof(double), typeof(double)});
				RectangleD rectangle = (RectangleD)value;
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