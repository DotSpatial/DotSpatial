using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace DotSpatial.Positioning.Gps
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

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Converts the packet into an array of bytes.
        /// </summary>
        /// <returns></returns>
        public abstract byte[] ToByteArray();

        #endregion

        #region Overrides

        /// <summary>
        /// Returns a string representation of the packet
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }

        #endregion

        #region IFormattable Members

        /// <summary>
        /// Returns a string representing the packet
        /// </summary>
        /// <param name="format"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public virtual string ToString(string format, IFormatProvider formatProvider)
        {
            return base.ToString();
        }

        #endregion
    }
}
