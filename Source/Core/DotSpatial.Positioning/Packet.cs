// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System;
using System.Globalization;

namespace DotSpatial.Positioning
{
    /// <summary>
    /// Represents a base class for designing GPS data packets.
    /// </summary>
    public abstract class Packet : IFormattable
    {
        #region Abstract Properties

        /// <summary>
        /// Returns whether the pack data is well-formed.
        /// </summary>
        public abstract bool IsValid { get; }

        #endregion Abstract Properties

        #region Abstract Methods

        /// <summary>
        /// Converts the packet into an array of bytes.
        /// </summary>
        /// <returns></returns>
        public abstract byte[] ToByteArray();

        #endregion Abstract Methods

        #region Overrides

        /// <summary>
        /// Returns a string representation of the packet
        /// </summary>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }

        #endregion Overrides

        #region IFormattable Members

        /// <summary>
        /// Returns a string representing the packet
        /// </summary>
        /// <param name="format">The format to use.-or- A null reference (Nothing in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable"/> implementation.</param>
        /// <param name="formatProvider">The provider to use to format the value.-or- A null reference (Nothing in Visual Basic) to obtain the numeric format information from the current locale setting of the operating system.</param>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        public virtual string ToString(string format, IFormatProvider formatProvider)
        {
            return base.ToString();
        }

        #endregion IFormattable Members
    }
}