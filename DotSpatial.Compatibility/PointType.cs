// ********************************************************************************************************
// Product Name: DotSpatial.Compatibility.dll
// Description:  Supports DotSpatial interfaces organized for a MapWindow 4 plugin context.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 12:04:41 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// PointTypes
    /// </summary>
    public enum PointType
    {
        /// <summary>
        /// Circular points
        /// </summary>
        Circle,

        /// <summary>
        /// Diamond
        /// </summary>
        Diamond,

        /// <summary>
        /// Square
        /// </summary>
        Square,

        /// <summary>
        /// Triangle pointed down
        /// </summary>
        TriangleDown,

        /// <summary>
        /// Triangle pointed left
        /// </summary>
        TriangleLeft,

        /// <summary>
        /// Triangle pointed right
        /// </summary>
        TriangleRight,

        /// <summary>
        /// Triangle pointed up
        /// </summary>
        TriangleUp,

        /// <summary>
        /// User defined
        /// </summary>
        UserDefined,
    }
}