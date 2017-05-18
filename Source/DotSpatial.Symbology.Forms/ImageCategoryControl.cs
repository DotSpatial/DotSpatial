// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Windows.Forms;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// ImageCategoryControl
    /// </summary>
    public partial class ImageCategoryControl : UserControl, ICategoryControl
    {
        #region Fields

        private bool _ignoreRefresh;
        private IImageLayer _newLayer;
        private IImageLayer _originalLayer;
        private IImageSymbolizer _symbolizer;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageCategoryControl"/> class.
        /// </summary>
        public ImageCategoryControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageCategoryControl"/> class with the given layer.
        /// </summary>
        /// <param name="layer">The layer.</param>
        public ImageCategoryControl(IImageLayer layer)
        {
            InitializeComponent();
            Initialize(layer);
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the apply changes option has been triggered.
        /// </summary>
        public event EventHandler ChangesApplied;

        #endregion

        #region Methods

        /// <summary>
        /// Fires the apply changes situation externally, forcing the Table to
        /// write its values to the original layer.
        /// </summary>
        public void ApplyChanges()
        {
            OnApplyChanges();
        }

        /// <summary>
        /// Cancel the action.
        /// </summary>
        public void Cancel()
        {
            OnCancel();
        }

        /// <inheritdoc />
        public void Initialize(ILayer layer)
        {
            Initialize(layer as IImageLayer);
        }

        /// <summary>
        /// Sets up the Table to work with the specified layer.
        /// </summary>
        /// <param name="layer">The layer.</param>
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
        /// Applies the changes that have been specified in this control
        /// </summary>
        protected virtual void OnApplyChanges()
        {
            _originalLayer.Symbolizer = _newLayer.Symbolizer.Copy();
            ChangesApplied?.Invoke(_originalLayer, EventArgs.Empty);
        }

        /// <summary>
        /// Event that fires when the action is canceled.
        /// </summary>
        protected virtual void OnCancel()
        {
            _originalLayer.Symbolizer = _symbolizer;
        }

        private void RsOpacityValueChanged(object sender, EventArgs e)
        {
            if (_ignoreRefresh) return;
            _newLayer.Symbolizer.Opacity = Convert.ToSingle(rsOpacity.Value);
        }

        #endregion
    }
}