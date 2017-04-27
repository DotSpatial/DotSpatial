// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  Contains the forms for symbology layers and symbol categories.
// ********************************************************************************************************
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Windows.Forms;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology.Forms
{
    public partial class ImageCategoryControl : UserControl, ICategoryControl
    {
        /// <summary>
        /// Occurs when the apply changes option has been triggered.
        /// </summary>
        public event EventHandler ChangesApplied;

        #region Private Variables

        private bool _ignoreRefresh;
        private IImageLayer _newLayer;
        private IImageLayer _originalLayer;
        private IImageSymbolizer _symbolizer;

        #endregion

        /// <summary>
        /// Initialize new instance of <see cref="ImageCategoryControl"/>.
        /// </summary>
        public ImageCategoryControl()
        {
            InitializeComponent();

        }

        /// <inheritdoc />
        public void Initialize(ILayer layer)
        {
            Initialize(layer as IImageLayer);
        }

        /// <summary>
        /// Initialize new instance of <see cref="ImageCategoryControl"/> with given layer.
        /// </summary>
        public ImageCategoryControl(IImageLayer layer)
        {
            InitializeComponent();
            Initialize(layer);
        }


        private void rsOpacity_ValueChanged(object sender, EventArgs e)
        {
            if (_ignoreRefresh) return;
            _newLayer.Symbolizer.Opacity = Convert.ToSingle(rsOpacity.Value);
        }

        /// <summary>
        /// Sets up the Table to work with the specified layer
        /// </summary>
        /// <param name="layer"></param>
        public void Initialize(IImageLayer layer)
        {
            if (layer.Symbolizer == null) layer.Symbolizer = new ImageSymbolizer();
            _originalLayer = layer;
            _symbolizer = layer.Symbolizer;
            _newLayer = layer.Copy();

            _ignoreRefresh = true;
            rsOpacity.Value = _symbolizer.Opacity;
            _ignoreRefresh = false;
        }



        /// <summary>
        /// Fires the apply changes situation externally, forcing the Table to
        /// write its values to the original layer.
        /// </summary>
        public void ApplyChanges()
        {
            OnApplyChanges();
        }

        /// <summary>
        /// Applies the changes that have been specified in this control
        /// </summary>
        protected virtual void OnApplyChanges()
        {
            _originalLayer.Symbolizer = _newLayer.Symbolizer.Copy();
            // _originalLayer.WriteBitmap(mwProgressBar1);
            if (ChangesApplied != null) ChangesApplied(_originalLayer, EventArgs.Empty);
        }

        /// <summary>
        /// Cancel the action.
        /// </summary>
        public void Cancel()
        {
            OnCancel();
        }

        /// <summary>
        /// Event that fires when the action is canceled.
        /// </summary>
        protected virtual void OnCancel()
        {
            _originalLayer.Symbolizer = _symbolizer;
        }

    }
}
