using System;

namespace DotSpatial.Topology.Utilities.RToolsUtil
{
    /// <summary>
    /// Bitwise enumeration for character types.
    /// </summary>
    [Flags]
    public enum CharTypeBits : byte
    {
        /// <summary>word characters (usually alpha, digits, and domain specific)</summary>
        Word = 1,
        /// <summary># or something for line comments</summary>
        Comment = 2,
        /// <summary>whitespace</summary>
        Whitespace = 4,
        /// <summary>' or " type</summary>
        Quote = 8,
        /// <summary>usually 0 to 9</summary>
        Digit = 16,
        /// <summary>usually 0 to 9, a-f and A-F</summary>
        HexDigit = 32,
        /// <summary>eof char</summary>
        Eof = 64
    }

}
