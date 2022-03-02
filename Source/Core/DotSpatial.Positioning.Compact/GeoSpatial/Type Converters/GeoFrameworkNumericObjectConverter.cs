#if (!PocketPC || DesignTime) && Framework20
using System;
using System.ComponentModel;
using System.Globalization;

namespace DotSpatial.Positioning.Design
{
    public abstract class GeoFrameworkNumericObjectConverter : GeoFrameworkObjectConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(double)
                    || sourceType == typeof(int)
                    || sourceType == typeof(float))
            {
                return true;
            }

            // Defer to the base class
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            // If the type is a string or a double, it's ok to proceed with conversion.
            if (value is string || value is double || value is int || value is float)
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
#if PocketPC
                        DestinationType = Type.GetType(HandledTypeName + ", GeoFramework.PocketPC, Version=" + HandledAssemblyVersion.ToString(4) + ", Culture=neutral, PublicKeyToken=3ed3cdf4fdda3400");
#else
                        DestinationType = Type.GetType(HandledTypeName + ", GeoFramework, Version=" + HandledAssemblyVersion.ToString(4) + ", Culture=neutral, PublicKeyToken=3ed3cdf4fdda3400");
#endif
                    }
                }

                // If a destination type was found, go ahead and create a new instance!
                if (DestinationType != null)
                {
                    return Activator.CreateInstance(DestinationType, new object[] { value });
                }
            }

            // Defer to the base class
            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(double)
                    || destinationType == typeof(int)
                    || destinationType == typeof(float))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }
    }
}
#endif