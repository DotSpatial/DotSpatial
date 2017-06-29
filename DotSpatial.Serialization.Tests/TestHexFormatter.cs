using System;
using System.Globalization;
using DotSpatial.Serialization;

namespace MapWindow.Tests.XML.TestData
{
    public class ObjectWithIntMember
    {
        [Serialize("Number", Formatter = typeof(TestHexFormatter), ConstructorArgumentIndex = 0)]
        private int _number;

        public int Number
        {
            get { return _number; }
            set { _number = value; }
        }

        public ObjectWithIntMember(int number)
        {
            _number = number;
        }
    }

    public class TestHexFormatter : SerializationFormatter
    {
        public override string ToString(object value)
        {
            if (value.GetType() != typeof(int))
                throw new NotSupportedException("Only integers are supported by this formatter");

            return "0x" + ((int)value).ToString("X", CultureInfo.InvariantCulture);
        }

        public override object FromString(string value)
        {
            if (value.StartsWith("0x"))
                value = value.Substring(2);

            return int.Parse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        }
    }
}