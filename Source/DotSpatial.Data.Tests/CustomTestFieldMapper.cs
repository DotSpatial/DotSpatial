using System;

namespace DotSpatial.Data.Tests
{
    class CustomTestFieldMapper : IFieldTypeCharacterMapper
    {
        public char Map(Type type)
        {
            if (type == typeof(bool)) return FieldTypeCharacters.Logic;
            if (type == typeof(DateTime)) return FieldTypeCharacters.DateTime;

            // We are using numeric in most cases here, because that is the format most compatible with other
            // Applications
            if (type == typeof(float)) return FieldTypeCharacters.Double;
            if (type == typeof(double)) return FieldTypeCharacters.Double;
            if (type == typeof(decimal)) return FieldTypeCharacters.Double;
            if (type == typeof(byte)) return FieldTypeCharacters.Number;
            if (type == typeof(short)) return FieldTypeCharacters.Number;
            if (type == typeof(int)) return FieldTypeCharacters.Number;
            if (type == typeof(long)) return FieldTypeCharacters.Number;

            // The default is to store it as a string type
            return FieldTypeCharacters.Text;
        }
    }
}