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
// The Initial Developer of this Original Code is Brian Marchionni. Created 3/18/2010
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// Used by Segment.ClosestPointTo() to detail how the input point interacts with the line segment
    /// </summary>
    public enum EndPointInteraction
    {
        /// <summary>
        /// The vertex found is on the line segment and between P1 and P2
        /// </summary>
        OnLine,
        /// <summary>
        /// The vertex found is beyond the end of P1
        /// </summary>
        PastP1,
        /// <summary>
        /// The vertex found is beyond the end of P2
        /// </summary>
        PastP2,
        /// <summary>
        /// P1 equals P2 so the segment cannot be extended into an infinite line the closest vertex is P1/P2
        /// </summary>
        P1equalsP2
    }
}