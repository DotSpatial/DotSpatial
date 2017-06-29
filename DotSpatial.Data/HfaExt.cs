// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/25/2010 4:23:30 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;

namespace DotSpatial.Data
{
    /// <summary>
    /// Extension Methods useful for the DotSpatial.Data.Img content
    /// </summary>
    public static class HfaExt
    {
        /// <summary>
        /// A common task seems to be parsing the text up to a certain delimiter
        /// starting from a certain index, and then advancing the string pointer.
        /// This advances the index, extracting the string up to the point of the
        /// specified delimeter, or the end of the string, and advancing the string
        /// past the delimeter itself.  If the index is equal to the length,
        /// then you are at the end of the string.
        /// </summary>
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
        /// Given a string, this reads digits into a numeric value.
        /// </summary>
        /// <param name="input">The string to read the integer from</param>
        /// <returns>An integer read from the string</returns>
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
        /// This variant of the ExtractTo algorithm effectively splits the string into teh content
        /// before the delimeter and after the delimeter, not counting the delimeter.
        /// </summary>
        /// <param name="input">The string input</param>
        /// <param name="delimeter">The string delimeter</param>
        /// <param name="remainder">the string remainder</param>
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
        /// This method returns a substring based on the next occurance of the
        /// specified delimeter.
        /// </summary>
        /// <param name="input">The string input</param>
        /// <param name="delimeter">The delimeter</param>
        public static string SkipTo(this string input, string delimeter)
        {
            int index = input.IndexOf(delimeter) + 1;
            return input.Substring(index, input.Length - index);
        }
    }
}