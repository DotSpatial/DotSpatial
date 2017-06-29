// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/4/2009 12:00:07 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// IGeoGroup
    /// </summary>
    public interface IMapGroup : IGroup, IMapLayer
    {
        #region Properties

        /// <summary>
        /// Gets the GeoLayerCollection for members contained by this group.
        /// </summary>
        IMapLayerCollection Layers
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the map frame for this group.
        /// </summary>
        IMapFrame ParentMapFrame
        {
            get;
        }

        #endregion
    }
}