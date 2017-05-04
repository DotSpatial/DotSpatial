// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Projections library.
//
// ********************************************************************************************************
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

        private ColorCategoryActions _colorCategoryActions;

        private FeatureLayerActions _featureLayerActions;
        private ImageLayerActions _imageLayerActions;
        private LayerActions _layerActions;

        private IWin32Window _owner;
        private RasterLayerActions _rasterLayerActions;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbologyEventManager"/> class.
        /// </summary>
        public SymbologyEventManager()
        {
            ColorCategoryActions = new ColorCategoryActions();
            FeatureLayerActions = new FeatureLayerActions();
            ImageLayerActions = new ImageLayerActions();
            LayerActions = new LayerActions();
            RasterLayerActions = new RasterLayerActions();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the custom actions for ColorCategory
        /// </summary>
        public ColorCategoryActions ColorCategoryActions
        {
            get
            {
                return _colorCategoryActions;
            }

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
            get
            {
                return _featureLayerActions;
            }

            set
            {
                _featureLayerActions = value;
                UpdateOwner(value);
            }
        }

        /// <summary>
        /// Gets or sets the actions for ImageLayers
        /// </summary>
        public ImageLayerActions ImageLayerActions
        {
            get
            {
                return _imageLayerActions;
            }

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
            get
            {
                return _layerActions;
            }

            set
            {
                _layerActions = value;
                UpdateOwner(value);
            }
        }

        /// <summary>
        /// Gets or sets owner for any dialogs that need to be launched.
        /// </summary>
        public IWin32Window Owner
        {
            get
            {
                return _owner;
            }

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
        /// Gets or sets the custom actions for RasterLayer
        /// </summary>
        public RasterLayerActions RasterLayerActions
        {
            get
            {
                return _rasterLayerActions;
            }

            set
            {
                _rasterLayerActions = value;
                UpdateOwner(value);
            }
        }

        #endregion

        #region Methods

        private void UpdateOwner(IIWin32WindowOwner owner)
        {
            if (owner == null) return;
            owner.Owner = Owner;
        }

        #endregion
    }
}