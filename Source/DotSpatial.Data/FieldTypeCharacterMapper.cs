using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotSpatial.Data
{
    /// <summary>
    /// Default FieldTypeCharacterMapper that is responsible for mapping a Type to a FieldTypeCharacter
    /// </summary>
    public class FieldTypeCharacterMapper : IFieldTypeCharacterMapper
    {
        /// <summary>
        /// Maps a Type to a FieldTypeCharacter
        /// </summary>
        /// <param name="type">A Type to convert to the char FieldTypeCharacter</param>
        /// <returns> _</returns>
        public char Map(Type type)
        {
            if (type == typeof(bool)) return FieldTypeCharacters.Logic;
            if (type == typeof(DateTime)) return FieldTypeCharacters.DateTime;

            // We are using numeric in most cases here, because that is the format most compatible with other
            // Applications
            if (type == typeof(float)) return FieldTypeCharacters.Number;
            if (type == typeof(double)) return FieldTypeCharacters.Number;
            if (type == typeof(decimal)) return FieldTypeCharacters.Number;
            if (type == typeof(byte)) return FieldTypeCharacters.Number;
            if (type == typeof(short)) return FieldTypeCharacters.Number;
            if (type == typeof(int)) return FieldTypeCharacters.Number;
            if (type == typeof(long)) return FieldTypeCharacters.Number;

            // The default is to store it as a string type
            return FieldTypeCharacters.Text;
        }
    }
}
