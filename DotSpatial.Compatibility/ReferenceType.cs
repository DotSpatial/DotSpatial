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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 4:15:41 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// ReferenceTypes
    /// </summary>
    public enum ReferenceType
    {
        /// <summary>
        /// The coordinates are drawn in screen coordinates on the layer, and stay fixed as the map
        /// zooms and pans
        /// </summary>
        Screen,

        /// <summary>
        /// The drawing layer is geographically referenced and will move with the other spatially
        /// referenced map content.
        /// </summary>
        Geographic
    }
}