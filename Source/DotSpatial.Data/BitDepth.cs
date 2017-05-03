// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/18/2010 3:49:21 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// BitDepth
    /// </summary>
    public enum BitDepth : byte
    {
        /// <summary>
        /// One bit per band pixel
        /// </summary>
        One = 1,

        /// <summary>
        /// Two bits per band pixel
        /// </summary>
        Two = 2,

        /// <summary>
        /// Four bits per band pixel
        /// </summary>
        Four = 4,

        /// <summary>
        /// Eight bits per band pixel (normal)
        /// </summary>
        Eight = 8,

        /// <summary>
        /// Sixteen bits per band pixel
        /// </summary>
        Sixteen = 16
    }
}