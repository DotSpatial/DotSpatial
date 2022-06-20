// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

namespace DotSpatial.Positioning
{
    /// <summary>
    /// Facilitates the creation of a deep copy of an object.
    /// </summary>
    /// <typeparam name="T">The destination type for the ICloneable interface.</typeparam>
    public interface ICloneable<out T>
    {
        /// <summary>
        /// Creates a deep copy of the object.
        /// </summary>
        /// <returns></returns>
        T Clone();
    }
}