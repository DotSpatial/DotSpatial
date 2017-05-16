// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// IDescriptor
    /// </summary>
    public interface IDescriptor : IMatchable, IRandomizable, ICloneable
    {
        #region Methods

        /// <summary>
        /// This copies the public descriptor properties from the specified object to this object.
        /// </summary>
        /// <param name="other">An object that has properties that match the public properties on this object.</param>
        void CopyProperties(object other);

        #endregion
    }
}