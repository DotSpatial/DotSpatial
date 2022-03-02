// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Data
{
    /// <summary>
    /// A simple structure that contains the elements of a shapefile that must exist.
    /// </summary>
    public struct ShapeHeader
    {
        /// <summary>
        /// Gets or sets the content length.
        /// </summary>
        public int ContentLength { get; set; }

        /// <summary>
        /// Gets or sets the offset in 16-bit words.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Gets the offset in bytes.
        /// </summary>
        public int ByteOffset => Offset * 2;

        /// <summary>
        /// Gets the length in bytes.
        /// </summary>
        public int ByteLength => ContentLength * 2;
    }
}