// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// A form for selecting colors.
    /// </summary>
    public partial class ColorPicker : Form
    {
        #region Fields
        private readonly IColorCategory _rasterCategory;
        private SymbologyStatusStrip _spatialStatusStrip1;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorPicker"/> class.
        /// </summary>
        public ColorPicker()
        {
            InitializeComponent();
            _rasterCategory = null; // this will be reset afterwards, but clear it in case we aren't calling with info.
            dialogButtons1.OkClicked += BtnOkClick;
            dialogButtons1.CancelClicked += BtnCancelClick;
            dialogButtons1.ApplyClicked += DialogButtons1ApplyClicked;
            lblPreview.Paint += LblPreviewPaint;
            grdSlider.GradientChanged += GrdSliderGradientChanged;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorPicker"/> class and sets it up for a specific color break.
        /// </summary>
        /// <param name="category">Category used to set the minimum and maximum color.</param>
        public ColorPicker(IColorCategory category)
            : this()
        {
            grdSlider.MinimumColor = category.LowColor;
            grdSlider.MaximumColor = category.HighColor;
            _rasterCategory = category;
            lblPreview.Invalidate();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorPicker"/> class.
        /// </summary>
        /// <param name="startColor">The minimum color.</param>
        /// <param name="endColor">The maximum color.</param>
        public ColorPicker(Color startColor, Color endColor)
            : this()
        {
            grdSlider.MinimumColor = startColor;
            grdSlider.MaximumColor = endColor;
            _rasterCategory = null;
            lblPreview.Invalidate();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the Apply button is pressed, implying to set the changes using the current colorbreak.
        /// </summary>
        public event EventHandler ChangesApplied;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the end color for this dialog
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the end color for this dialog")]
        public Color HighColor
        {
            get
            {
                return grdSlider.MaximumColor;
            }

            set
            {
                grdSlider.MaximumColor = value;
                if (Visible) lblPreview.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the start color for this dialog
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the start color for this dialog")]
        public Color LowColor
        {
            get
            {
                return grdSlider.MinimumColor;
            }

            set
            {
                grdSlider.MinimumColor = value;
                if (Visible) lblPreview.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the original instance for updating when the apply button is pressed.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IDescriptor Original { get; set; }

        /// <summary>
        /// Gets the IProgressHandler version of the status bar on this form
        /// </summary>
        public IProgressHandler ProgressHandler => _spatialStatusStrip1;

        #endregion

        #region Methods

        /// <summary>
        /// Fires the ChangesApplied event
        /// </summary>
        protected virtual void OnChangesApplied()
        {
            if (_rasterCategory != null)
            {
                _rasterCategory.LowColor = grdSlider.MinimumColor;
                _rasterCategory.HighColor = grdSlider.MaximumColor;
            }

            Original.CopyProperties(_rasterCategory);
            ChangesApplied?.Invoke(this, EventArgs.Empty);
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;

            OnChangesApplied();
            Close();
        }

        private void DialogButtons1ApplyClicked(object sender, EventArgs e)
        {
            OnChangesApplied();
        }

        private void GrdSliderGradientChanged(object sender, EventArgs e)
        {
            lblPreview.Invalidate();
        }

        private void LblPreviewPaint(object sender, PaintEventArgs e)
        {
            UpdatePreview(e);
        }

        private void UpdatePreview(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (grdSlider == null || lblPreview == null) return;

            LinearGradientBrush br = new LinearGradientBrush(lblPreview.ClientRectangle, grdSlider.MinimumColor, grdSlider.MaximumColor, LinearGradientMode.Horizontal);
            g.FillRectangle(br, e.ClipRectangle);
            br.Dispose();
        }

        #endregion
    }
}