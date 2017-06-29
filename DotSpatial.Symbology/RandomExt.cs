// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/29/2009 1:01:44 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;
using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Extensions for <see cref="Random"/> class
    /// </summary>
    public static class RandomExt
    {
        /// <summary>
        /// Generates a boolean at random from this generator.
        /// </summary>
        /// <param name="generator">THe random class that generates the random content.</param>
        /// <returns>A boolean that has equal probability of being on or off.</returns>
        public static bool NextBool(this Random generator)
        {
            return (generator.Next(0, 1) == 1);
        }

        /// <summary>
        /// Extends the Random class to also allow it to generate random colors
        /// </summary>
        /// <param name="generator">This random number generator</param>
        public static Color NextColor(this Random generator)
        {
            return Color.FromArgb(generator.Next(0, 255), generator.Next(0, 255), generator.Next(0, 255));
        }

        /// <summary>
        /// Generates a floating point value from 0 to 1
        /// </summary>
        /// <param name="generator">The random class to extend</param>
        /// <returns>A new randomly created floating point value from 0 to 1</returns>
        public static float NextFloat(this Random generator)
        {
            return (float)generator.NextDouble();
        }

        /// <summary>
        /// Generates a random floating point value from 0 to the specified extremeValue, which can
        /// be either positive or negative.
        /// </summary>
        /// <param name="generator">This random class</param>
        /// <param name="extremeValue">The floating point maximum for the number being calculated</param>
        /// <returns>A value ranging from 0 to ma</returns>
        public static float NextFloat(this Random generator, float extremeValue)
        {
            return generator.NextFloat() * extremeValue;
        }

        /// <summary>
        /// Calculates a random floating point value that ranges between (inclusive) the specified minimum and maximum values.
        /// </summary>
        /// <param name="generator">The random class to generate the random value</param>
        /// <param name="minimum">The floating point maximum</param>
        /// <param name="maximum">The floating point minimum</param>
        /// <returns>A floating point value that is greater than or equal to the minimum and less than or equal to the maximum</returns>
        public static float NextFloat(this Random generator, float minimum, float maximum)
        {
            var dbl = generator.NextDouble();
            var spread = maximum - (double)minimum;
            return (float)(dbl * spread + minimum);
        }

        /// <summary>
        /// Given any enumeration of type T, this will return a random instance of that enumeration
        /// </summary>
        /// <typeparam name="T">The type of the enumeration</typeparam>
        /// <param name="generator">This random generator</param>
        /// <returns>The T type to generate</returns>
        public static T NextEnum<T>(this Random generator)
        {
            var names = Enum.GetNames(typeof(T));
            return Global.ParseEnum<T>(names[generator.Next(0, names.Length - 1)]);
        }

        /// <summary>
        /// Generates an array of boolean values that ranges from minLength to maxLength
        /// </summary>
        /// <param name="generator">The Random class for generating random values</param>
        /// <param name="minLength">THe minimum length of the array</param>
        /// <param name="maxLength">The maximum length of the array</param>
        /// <returns>An array of boolean values.</returns>
        public static bool[] NextBoolArray(this Random generator, int minLength, int maxLength)
        {
            var len = generator.Next(minLength, maxLength);
            var result = new bool[len];
            for (var i = 0; i < len; i++)
            {
                result[i] = (generator.Next(0, 1) == 1);
            }
            return result;
        }
    }
}