// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Serialization
{
    /// <summary>
    /// CloneableEM.
    /// </summary>
    public static class CloneableEm
    {
        #region Methods

        /// <summary>
        /// The type parameter T is optional, so the intended use would be like:
        /// ObjectType copy = myObject.Copy();.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="original">The original object.</param>
        /// <returns>A new object of the same type as the type being copied.</returns>
        public static T Copy<T>(this T original)
            where T : class, ICloneable
        {
            return original?.Clone() as T;
        }

        #endregion
    }
}