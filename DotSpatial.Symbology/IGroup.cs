// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 2:18:42 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Group
    /// </summary>
    public interface IGroup : ILayer, IList<ILayer>
    {
        #region Events

        /// <summary>
        /// This occurs when a new layer is added either to this group, or one of the child groups within this group.
        /// </summary>
        event EventHandler<LayerEventArgs> LayerAdded;

        /// <summary>
        /// This occurs when a layer is removed from this group.
        /// </summary>
        event EventHandler<LayerEventArgs> LayerRemoved;

        #endregion

        #region Methods

        /// <summary>
        /// Resume events will resume events on the layers if all the suspensions are
        /// canceled out.
        /// </summary>
        void ResumeEvents();

        /// <summary>
        /// Adds one more increment of suspension which will prevent events from firing
        /// for the layers.
        /// </summary>
        void SuspendEvents();

        /// <summary>
        /// Gets the layers cast as ILayer without any information about the actual drawing methods.
        /// This is useful for handling methods that my come from various types of maps.
        /// </summary>
        /// <returns>An enumerable collection of ILayer</returns>
        IList<ILayer> GetLayers();

        /// <summary>
        /// Returns the number of data layers, not counting groups.  If recursive is true, then layers that are within
        /// groups will be counted, even though the groups themselves are not.
        /// </summary>
        /// <param name="recursive">Boolean, if true forces checking even the number of child members.</param>
        /// <returns>An integer representing the total number of layers in this collection and its children.</returns>
        int GetLayerCount(bool recursive);

        /// <summary>
        /// Returns a snapshot image of this group
        /// </summary>
        /// <param name="imgWidth">Width in pixels of the returned image (height is determined by the number of layers in the group)</param>
        /// <returns>Bitmap of the group and sublayers (expanded)</returns>
        Bitmap LegendSnapShot(int imgWidth);

        #endregion

        #region Properties

        /// <summary>
        /// Gets whether or not the events are suspended on the layer collection
        /// </summary>
        bool EventsSuspended
        {
            get;
        }

        /// <summary>
        /// Gets the integer handle for this group
        /// </summary>
        int Handle
        {
            get;
        }

        /// <summary>
        /// Gets or sets the icon
        /// </summary>
        Image Icon
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the integer count of layers.  This can also be accessed through Layers.Count.
        /// </summary>
        int LayerCount
        {
            get;
        }

        /// <summary>
        /// Gets or sets the visibility of the layers within this group.
        /// Note: When reading this property, it returns true if any layer is visible within
        /// this group
        /// </summary>
        bool LayersVisible
        {
            get;

            set;
        }

        ///// <summary>
        ///// Gets the legend that this group belongs to... regardless of how deep the item is.
        ///// </summary>
        //ILegend Legend
        //{
        //    get;
        //}

        /// <summary>
        /// Gets the parent group of this group.
        /// </summary>
        IGroup ParentGroup
        {
            get;
        }

        /// <summary>
        /// gets or sets the locked property, which prevents the user from changing the visual state
        /// except layer by layer
        /// </summary>
        bool StateLocked
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the layer handle of the specified layer
        /// </summary>
        /// <param name="positionInGroup">0 based index into list of layers</param>
        /// <returns>Layer's handle on success, -1 on failure</returns>
        int LayerHandle(int positionInGroup);

        #endregion
    }
}