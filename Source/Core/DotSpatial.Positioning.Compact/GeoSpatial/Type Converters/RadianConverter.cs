#if !PocketPC || DesignTime
using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace DotSpatial.Positioning.Design
{
    public sealed class RadianConverter : GeoFrameworkNumericObjectConverter 
    {
        protected override string HandledTypeName
        {
            get
            {
                return "GeoFramework.Radian";
            }
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            // What is the destination type?
            if (destinationType == typeof(string) && value == null)
            {
                return "0";
            }
            else if (destinationType == typeof(InstanceDescriptor))
            {
                // Instance descriptor.  Used during Windows Forms serialization.

                // Get the type of the object to convert
                Type sourceType = value.GetType();

                // Is the value null?
                if (value == null)
                {
                    // Yes.  Return the "Empty" static value
                    return new InstanceDescriptor(sourceType.GetField("Empty"), null);
                }
                else
                {
                    // Get the properties needed to generate a constructor
                    object[] ConstructorParameters = new object[] { 
                            sourceType.GetProperty("Value").GetValue(value, null) };
                    Type[] ConstructorTypes = new Type[] { typeof(double) };

                    // Now activate the constructor
                    ConstructorInfo Constructor = sourceType.GetConstructor(ConstructorTypes);
                    return new InstanceDescriptor(Constructor, ConstructorParameters);
                }
            }

            // Defer to the base class
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return false;
        }

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return null;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}

#endif