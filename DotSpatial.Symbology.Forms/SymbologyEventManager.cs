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
        #region Fields

        private FeatureLayerActions _featureLayerActions;
        private ImageLayerActions _imageLayerActions;
        private LayerActions _layerActions;
        private RasterLayerActions _rasterLayerActions;
        private ColorCategoryActions _colorCategoryActions;

        private IWin32Window _owner;

        #endregion

        /// <summary>
        /// The symbology event manager.
        /// </summary>
        public SymbologyEventManager()
        {
            ColorCategoryActions = new ColorCategoryActions();
            FeatureLayerActions = new FeatureLayerActions();
            ImageLayerActions = new ImageLayerActions();
            LayerActions = new LayerActions();
            RasterLayerActions = new RasterLayerActions();
        }

        private void UpdateOwner(IIWin32WindowOwner owner)
        {
            if (owner == null) return;
            owner.Owner = Owner;
        }

        /// <summary>
        /// Allows setting the owner for any dialogs that need to be launched.
        /// </summary>
        public IWin32Window Owner
        {
            get { return _owner; }
            set
            {
                _owner = value;
                UpdateOwner(ColorCategoryActions);
                UpdateOwner(FeatureLayerActions);
                UpdateOwner(RasterLayerActions);
                UpdateOwner(ImageLayerActions);
                UpdateOwner(LayerActions);
            }
        }

        /// <summary>
        /// Gets or sets the custom actions for ColorCategory
        /// </summary>
        public ColorCategoryActions ColorCategoryActions
        {
            get { return _colorCategoryActions; }
            set
            {
                _colorCategoryActions = value;
                UpdateOwner(value);
            }
        }

        /// <summary>
        /// Gets or sets the custom actions for FeatureLayer
        /// </summary>
        public FeatureLayerActions FeatureLayerActions
        {
            get { return _featureLayerActions; }
            set
            {
                _featureLayerActions = value;
                UpdateOwner(value);
            }
        }

        /// <summary>
        /// Gets or sets the custom actions for RasterLayer
        /// </summary>
        public RasterLayerActions RasterLayerActions
        {
            get { return _rasterLayerActions; }
            set
            {
                _rasterLayerActions = value;
                UpdateOwner(value);
            }
        }

        /// <summary>
        /// Gets or sets the actions for ImageLayers
        /// </summary>
        public ImageLayerActions ImageLayerActions
        {
            get { return _imageLayerActions; }
            set
            {
                _imageLayerActions = value;
                UpdateOwner(value);
            }
        }

        /// <summary>
        /// Gets or sets the custom actions for Layer
        /// </summary>
        public LayerActions LayerActions
        {
            get { return _layerActions; }
            set
            {
                _layerActions = value;
                UpdateOwner(value);
            }
        }
    }
}