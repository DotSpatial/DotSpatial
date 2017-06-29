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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 4:01:58 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// Gets or sets the selection method to use.
    /// <list type="bullet">
    /// <item>Inclusion</item>
    /// <item>Intersection</item>
    /// </list>
    /// Inclusion means that the entire shape must be within the selection bounds in order to select
    /// the shape.  Intersection means that only a portion of the shape must be within the selection
    /// bounds in order for the shape to be selected.
    /// </summary>
    public enum SelectMode
    {
        /// <summary>
        /// The entire contents of the potentially selected item must fall withing the specified extents
        /// </summary>
        Inclusion,

        /// <summary>
        /// The item will be selected if any of the contents of the potentially selected item can be found
        /// in the specified extents.
        /// </summary>
        Intersection,
    }
}