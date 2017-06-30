// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/15/2009 4:37:11 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// ScaleBarPositions
    /// </summary>
    [Obsolete("Do not use it. This enum is not used in DotSpatial anymore.")] // Marked in 1.7
    public enum ScaleBarPosition
    {
        /// <summary>
        /// The scalebar should appear in the lower left corner
        /// </summary>
        LowerLeft,
        /// <summary>
        /// The scalebar should appear in the lower right corner
        /// </summary>
        LowerRight,
        /// <summary>
        /// The scalebar should appear in the upper left corner
        /// </summary>
        UpperLeft,
        /// <summary>
        /// The scalebar should apepar int eh upper right corner
        /// </summary>
        UpperRight,
    }
}