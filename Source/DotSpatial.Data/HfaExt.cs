// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;

namespace DotSpatial.Data
{
    /// <summary>
    /// Extension Methods useful for the DotSpatial.Data.Img content.
    /// </summary>
    public static class HfaExt
    {
        #region Methods

        /// <summary>
        /// Given a string, this reads digits into a numeric value.
        /// </summary>
        /// <param name="input">The string to read the integer from.</param>
        /// <returns>An integer read from the string.</returns>
        public static int ExtractInteger(this string input)
        {
            List<char> number = new List<char>();
            int start = 0;
            while (input.Length > start && char.IsDigit(input[start]))
            {
                number.Add(input[start]);
                start++;
            }

            return int.Parse(new string(number.ToArray()));
        }

        /// <summary>
        /// A common task seems to be parsing the text up to a certain delimiter
        /// starting from a certain index, and then advancing the string pointer.
        /// This advances the index, extracting the string up to the point of the
        /// specified delimeter, or the end of the string, and advancing the string
        /// past the delimeter itself. If the index is equal to the length,
        /// then you are at the end of the string.
        /// </summary>
        /// <param name="input">Text to extract the string from.</param>
        /// <param name="start">The start index.</param>
        /// <param name="delimeter">The delimeter used as and end point.</param>
        /// <returns>The extracted string.</returns>
        public static string ExtractTo(this string input, ref int start, string delimeter)
        {
            int origialStart = start;
            if (input.Contains(delimeter))
            {
                start = input.IndexOf(delimeter);
            }
            else
            {
                start = input.Length - 1;
            }

            int length = start - origialStart;
            start += 1; // advance one unit past the delimeter
            return input.Substring(start, length);
        }

        /// <summary>
        /// This variant of the ExtractTo algorithm effectively splits the string into the content
        /// before the delimeter and after the delimeter, not counting the delimeter.
        /// </summary>
        /// <param name="input">The string input.</param>
        /// <param name="delimeter">The string delimeter.</param>
        /// <param name="remainder">the string remainder.</param>
        /// <returns>The first extracted string.</returns>
        public static string ExtractTo(this string input, string delimeter, out string remainder)
        {
            remainder = null;
            if (input.Contains(delimeter))
            {
                int i = input.IndexOf(delimeter);
                string result = input.Substring(0, i);
                if (i < input.Length - 1)
                {
                    remainder = input.Substring(i + 1, input.Length - (i + 1));
                }

                return result;
            }

            return input;
        }

        /// <summary>
        /// This method returns a substring based on the next occurance of the specified delimeter.
        /// </summary>
        /// <param name="input">The string input.</param>
        /// <param name="delimeter">The delimeter.</param>
        /// <returns>The substring.</returns>
        public static string SkipTo(this string input, string delimeter)
        {
            int index = input.IndexOf(delimeter) + 1;
            return input.Substring(index, input.Length - index);
        }

        #endregion
    }
}