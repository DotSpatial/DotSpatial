using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotSpatial.Data
{
    /// <summary>
    /// Specifies the Constant TypeCharacters used for Field.TypeCharacter
    /// </summary>
    public static class FieldTypeCharacters
    {
        /// <summary>
        /// Use for Floats, Number is used by default for Floats
        /// </summary>
        public const char Float = 'F';
        /// <summary>
        /// Use for Doubles, Number is used by default for Doubles
        /// </summary>
        public const char Double = 'B';
        /// <summary>
        /// Logic (True-False, Yes-No)
        /// </summary>
        public const char Logic = 'L';
        /// <summary>
        /// Number (Short, Integer, Long, Float, Double, byte)
        /// </summary>
        public const char Number = 'N'; 
        /// <summary>
        /// Use for DateTimes where you just need the time
        /// </summary>
        public const char Time = 'T';    // DateTime
        /// Use for DateTimes
        public const char DateTime = 'D'; // DateTime
        /// <summary>
        /// Use for Chars, Strings, objects - as ToString(), and structs - as 
        /// </summary>
        public const char Text = 'C'; 
    }
}
