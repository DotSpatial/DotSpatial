// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  Contains the forms for symbology layers and symbol categories.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Data;

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


        public ImageCategoryControl()
        {
            InitializeComponent();

        }
        public void Initialize(ILayer layer)
        { Initialize(layer as IImageLayer); }

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
            //_originalLayer.WriteBitmap(mwProgressBar1);
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
