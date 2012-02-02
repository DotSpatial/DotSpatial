// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Projections library.
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
// The Original Code is DotSpatial.dll for the DotSpatial project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in September, 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// This class is the System.Windows.Forms interpretor of symbology events being fired.
    /// </summary>
    public class SymbologyEventManager
    {
        private FeatureLayerEventReceiver _featureLayerEventReceiver = new FeatureLayerEventReceiver();
        private ImageLayerEventReceiver _imageLayerEventReceiver = new ImageLayerEventReceiver();
        private LayerEventReceiver _layerEventReceiver = new LayerEventReceiver();
        private RasterLayerEventReceiver _rasterLayerEventReceiver = new RasterLayerEventReceiver();

        /// <summary>
        /// Allows setting the owner for any dialogs that need to be launched.
        /// </summary>
        public IWin32Window Owner { get; set; }

        /// <summary>
        /// Gets or sets the custom handler to use for FeatureLayer events
        /// </summary>
        public FeatureLayerEventReceiver FeatureLayerEventReceiver
        {
            get { return _featureLayerEventReceiver; }
            set
            {
                _featureLayerEventReceiver = value;
                _featureLayerEventReceiver.Owner = Owner;
            }
        }

        /// <summary>
        /// Gets or sets the custom handler to use for FeatureLayer events
        /// </summary>
        public RasterLayerEventReceiver RasterLayerEventReceiver
        {
            get { return _rasterLayerEventReceiver; }
            set
            {
                _rasterLayerEventReceiver = value;
                _rasterLayerEventReceiver.Owner = Owner;
            }
        }

        /// <summary>
        /// Gets or sets the custom handler to use for FeatureLayer events
        /// </summary>
        public ImageLayerEventReceiver ImageLayerEventReceiver
        {
            get { return _imageLayerEventReceiver; }
            set
            {
                _imageLayerEventReceiver = value;
                _imageLayerEventReceiver.Owner = Owner;
            }
        }

        /// <summary>
        /// Gets or sets the custom handler to use for FeatureLayer events
        /// </summary>
        public LayerEventReceiver LayerEventReceiver
        {
            get { return _layerEventReceiver; }
            set
            {
                _layerEventReceiver = value;
                _layerEventReceiver.Owner = Owner;
            }
        }
    }
}