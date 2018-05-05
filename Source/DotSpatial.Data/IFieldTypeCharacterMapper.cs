using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// Responsible for mapping a Type to a FieldTypeCharacter
    /// </summary>
    public interface IFieldTypeCharacterMapper
    {
        /// <summary>
        /// Maps a Type to a FieldTypeCharacter
        /// </summary>
        /// <param name="type">A Type to convert to the char FieldTypeCharacter</param>
        /// <returns></returns>
        char Map(Type type);
    }
}