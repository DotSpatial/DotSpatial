// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// IRandomizable.
    /// </summary>
    public interface IRandomizable
    {
        /// <summary>
        /// This method will set the values for this class with random values that are
        /// within acceptable parameters for this class.
        /// </summary>
        /// <param name="generator">An existing random number generator so that the random seed can be controlled.</param>
        void Randomize(Random generator);
    }
}