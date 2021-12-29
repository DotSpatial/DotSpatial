// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// 2D size: Height, Width.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(Size2DConverter))]
    public class Size2D : Descriptor
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Size2D"/> class.
        /// </summary>
        public Size2D()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Size2D"/> class.
        /// </summary>
        /// <param name="width">The double width.</param>
        /// <param name="height">The double height.</param>
        public Size2D(double width, double height)
        {
            Width = width;
            Height = height;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        [Serialize("Height")]
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        [Serialize("Width")]
        public double Width { get; set; }

        #endregion

        #region Operators

        /// <summary>
        /// Determines if the height and width are both equal.
        /// </summary>
        /// <param name="a">First size to check.</param>
        /// <param name="b">Second size to check.</param>
        /// <returns>True, if both are equal.</returns>
        public static bool operator ==(Size2D a, Size2D b)
        {
            if ((object)a == null && (object)b == null) return true;
            if ((object)a == null || (object)b == null) return false;

            return a.Width == b.Width && a.Height == b.Height;
        }

        /// <summary>
        /// Determiens if the height and width are not equal.
        /// </summary>
        /// <param name="a">First size to check.</param>
        /// <param name="b">Second size to check.</param>
        /// <returns>True, if both are not equal.</returns>
        public static bool operator !=(Size2D a, Size2D b)
        {
            return !(a == b);
        }
        #endregion

        #region Methods

        /// <summary>
        /// Tests for equality against an object.
        /// </summary>
        /// <param name="obj">Size2D to check against.</param>
        /// <returns>True, if both are equal.</returns>
        public override bool Equals(object obj)
        {
            var other = obj as Size2D;
            if ((object)other == null) return false;

            return Equals(other);
        }

        /// <summary>
        /// Tests for equality against another size.
        /// </summary>
        /// <param name="size">the size to compare this size to.</param>
        /// <returns>boolean, true if the height and width are the same in each case.</returns>
        public bool Equals(Size2D size)
        {
            if ((object)size == null) return false;

            return size.Width == Width && size.Height == Height;
        }

        /// <summary>
        /// Returns the basic hash code for the object.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Gets the string equivalent of this object.
        /// </summary>
        /// <returns>String, containing width and height.</returns>
        public override string ToString()
        {
            return Width + ", " + Height;
        }

        /// <summary>
        /// Generates random doubles for the size from 1 to 100 for both the width and height.
        /// </summary>
        /// <param name="generator">The random generator.</param>
        protected override void OnRandomize(Random generator)
        {
            Width = generator.NextDouble() * 100;
            Height = generator.NextDouble() * 100;
        }

        #endregion
    }
}