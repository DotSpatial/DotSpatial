#if (!PocketPC || DesignTime) && Framework20
using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace DotSpatial.Positioning.Design
{
    public abstract class GeoFrameworkObjectConverter : ExpandableObjectConverter
    {
        #region ConvertFrom: Converts an object to an Distance

        // Indicates if the type can be converted
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string)
                || sourceType == typeof(InstanceDescriptor)
				|| sourceType.Name == HandledTypeName)
            {
                //Console.WriteLine("DEBUG: " + sourceType.Name + " CAN be converted to " + HandledTypeName + "...");

                return true;
            }

            //Console.WriteLine("DEBUG: " + sourceType.Name + " probably can NOT be converted to " + HandledTypeName + "...");

            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Indicates the name of the type handled by this type converter.
        /// </summary>
        protected abstract string HandledTypeName { get; }
        
        /// <summary>
        /// Indicates the full nume of the assembly housing objects handled by this type converter.
        /// </summary>
        protected virtual string HandledAssemblyName 
        {
            get
            {
#if PocketPC
                return "GeoFramework.PocketPC, Culture=neutral, Version=" + HandledAssemblyVersion.ToString(4) + ", PublicKeyToken=3ed3cdf4fdda3400";
#else
                return "GeoFramework, Culture=neutral, Version=" + HandledAssemblyVersion.ToString(4) + ", PublicKeyToken=3ed3cdf4fdda3400";
#endif
            }
        }

        protected virtual Version HandledAssemblyVersion
        {
            get
            {
                return new Version("2.0.0.0");
            }
        }

        // Converts an object of one type to an Distance
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            // If the type is a string or a double, it's ok to proceed with conversion.
            if (value is string)
            {
                // Try to resolve the target property type
                Type DestinationType = null;

                if (context != null)
                {
                    DestinationType = context.PropertyDescriptor.PropertyType;
                }
                else
                {
                    DestinationType = Type.GetType(HandledTypeName);
                    if (DestinationType == null && Environment.OSVersion.Platform == PlatformID.Win32NT)
                    {
                        DestinationType = Type.GetType(HandledTypeName + ", " + HandledAssemblyName);
                    }
                }

                // If a destination type was found, go ahead and create a new instance!
				if (DestinationType != null)
				{
					return Activator.CreateInstance(DestinationType, new object[] { value });
				}
				else
				{
				}
            }

#if DEBUG
            //Console.WriteLine("ConvertFrom: Could NOT convert " + value.GetType().Name + " to " + HandledTypeName);
#endif

            return base.ConvertFrom(context, culture, value);
        }

        #endregion

        #region ConvertTo: Converts an Distance to another type

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string)
                || destinationType == typeof(InstanceDescriptor))
            {
#if DEBUG
                Console.WriteLine("DEBUG: " + HandledTypeName + " CAN be converted to " + destinationType.Name + "...");
#endif

                return true;
            }

#if DEBUG
            Console.WriteLine("DEBUG: " + HandledTypeName + " probably can NOT be converted to " + destinationType.Name + "...");
#endif

            // Defer to the base class
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            // What is the destination type?
            if (destinationType == typeof(string))
            {
                // String.  Is the value null?
                if (value != null)
                {
                    return value.ToString();
                }
            }

#if DEBUG
            Console.WriteLine("ConvertFrom: Could NOT convert " + HandledTypeName + " to " + destinationType.Name);
#endif

            // Defer to the base class
            return base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion

    }
}
#endif