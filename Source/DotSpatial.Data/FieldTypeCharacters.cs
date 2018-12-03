// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

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

        /// <summary>
        /// Use for DateTimes.
        /// </summary>
        public const char DateTime = 'D'; // DateTime

        /// <summary>
        /// Use for Chars, Strings, objects.
        /// </summary>
        public const char Text = 'C';
    }
}
