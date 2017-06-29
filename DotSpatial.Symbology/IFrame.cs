// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// This interface stores a single extent window describing a view, and also contains
    /// the list of all the layers associated with that view.  The layers are ordered.
    /// </summary>
    public interface IFrame : IGroup
    {
        #region Events

        /// <summary>
        /// Occurs when some items should no longer render, and the map needs a refresh.
        /// </summary>
        event EventHandler UpdateMap;

        /// <summary>
        /// Occurs after zooming to a specific location on the map and causes a camera recent.
        /// </summary>
        event EventHandler<ExtentArgs> ViewExtentsChanged;

        #endregion

        #region Methods

        /// <summary>
        /// This will create a new layer from the featureset and add it.
        /// </summary>
        /// <param name="featureSet">Any valid IFeatureSet that does not yet have drawing characteristics</param>
        void Add(IFeatureSet featureSet);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a boolean that controls whether or not a newly added layer
        /// will also force a zoom to that layer.  If this is true, then nothing
        /// will happen.  Otherwise, adding layers to this frame or a group in this
        /// frame will set the extent.
        /// </summary>
        bool ExtentsInitialized { get; set; }

        /// <summary>
        /// Drawing layers are tracked separately, and do not appear in the legend.
        /// </summary>
        List<ILayer> DrawingLayers { get; set; }

        /// <summary>
        /// Gets or sets the currently active layer.
        /// </summary>
        ILayer SelectedLayer { get; }

        /// <summary>
        /// Controls the smoothing mode.  Default or None will have faster performance
        /// at the cost of quality.
        /// </summary>
        SmoothingMode SmoothingMode { get; set; }

        /// <summary>
        /// This is the geographic envelope in view.
        /// </summary>
        Extent ViewExtents { get; set; }

        #endregion
    }
}