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
// The Original Code is DotSpatial.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created February 2008.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// 9/21/09: Chris Wilson - aligned controls, removed min/max buttons, added help button, border
//          now fixeddialog, showontaskbar set to false
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// A form for selecting colors
    /// </summary>
    public class ColorPicker : Form
    {
        #region Events

        /// <summary>
        /// Occurs when the Apply button is pressed, implying to set the changes using the current colorbreak.
        /// </summary>
        public event EventHandler ChangesApplied;

        #endregion

        #region Private Variables

        // Designer variables
        private readonly IColorCategory _rasterCategory;
        private IDescriptor _original;
        private SymbologyStatusStrip _spatialStatusStrip1;
        private DialogButtons dialogButtons1;
        private GradientControl grdSlider;
        private GroupBox grpPreview;
        private HelpProvider helpProvider1;
        private Label lblPreview;
        private ToolStripProgressBar statusProgress;
        private ToolStripStatusLabel statusText;
        private TableLayoutPanel tableLayoutPanel1;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of this form.
        /// </summary>
        public ColorPicker()
        {
            InitializeComponent();
            _rasterCategory = null; // this will be reset afterwards, but clear it in case we aren't calling with info.
            dialogButtons1.OkClicked += BtnOkClick;
            dialogButtons1.CancelClicked += BtnCancelClick;
            dialogButtons1.ApplyClicked += dialogButtons1_ApplyClicked;
            lblPreview.Paint += LblPreviewPaint;
            grdSlider.GradientChanged += GrdSliderGradientChanged;
        }

        /// <summary>
        /// Constructs a new instance and sets it up for a specific color break
        /// </summary>
        public ColorPicker(IColorCategory category)
            : this()
        {
            grdSlider.MinimumColor = category.LowColor;
            grdSlider.MaximumColor = category.HighColor;
            _rasterCategory = category;
            lblPreview.Invalidate();
        }

        /// <summary>
        /// Constructs a new instance and sets up the colors, but won;t allow
        /// </summary>
        /// <param name="startColor"></param>
        /// <param name="endColor"></param>
        public ColorPicker(Color startColor, Color endColor)
            : this()
        {
            grdSlider.MinimumColor = startColor;
            grdSlider.MaximumColor = endColor;
            _rasterCategory = null;
            lblPreview.Invalidate();
        }

        private void dialogButtons1_ApplyClicked(object sender, EventArgs e)
        {
            OnChangesApplied();
        }

        private void GrdSliderGradientChanged(object sender, EventArgs e)
        {
            lblPreview.Invalidate();
        }

        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorPicker));
            this.grpPreview = new System.Windows.Forms.GroupBox();
            this.lblPreview = new System.Windows.Forms.Label();
            this.dialogButtons1 = new DotSpatial.Symbology.Forms.DialogButtons();
            this.grdSlider = new DotSpatial.Symbology.Forms.GradientControl();
            this._spatialStatusStrip1 = new DotSpatial.Symbology.Forms.SymbologyStatusStrip();
            this.statusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.grpPreview.SuspendLayout();
            this._spatialStatusStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            //
            // grpPreview
            //
            resources.ApplyResources(this.grpPreview, "grpPreview");
            this.grpPreview.Controls.Add(this.lblPreview);
            this.helpProvider1.SetHelpString(this.grpPreview, resources.GetString("grpPreview.HelpString"));
            this.grpPreview.Name = "grpPreview";
            this.helpProvider1.SetShowHelp(this.grpPreview, ((bool)(resources.GetObject("grpPreview.ShowHelp"))));
            this.grpPreview.TabStop = false;
            //
            // lblPreview
            //
            this.lblPreview.BackColor = System.Drawing.Color.White;
            this.lblPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lblPreview, "lblPreview");
            this.lblPreview.Name = "lblPreview";
            this.helpProvider1.SetShowHelp(this.lblPreview, ((bool)(resources.GetObject("lblPreview.ShowHelp"))));
            //
            // dialogButtons1
            //
            resources.ApplyResources(this.dialogButtons1, "dialogButtons1");
            this.dialogButtons1.Name = "dialogButtons1";
            this.helpProvider1.SetShowHelp(this.dialogButtons1, ((bool)(resources.GetObject("dialogButtons1.ShowHelp"))));
            //
            // grdSlider
            //
            resources.ApplyResources(this.grdSlider, "grdSlider");
            this.grdSlider.EndValue = 0.8F;
            this.helpProvider1.SetHelpString(this.grdSlider, resources.GetString("grdSlider.HelpString"));
            this.grdSlider.MaximumColor = System.Drawing.Color.Blue;
            this.grdSlider.MinimumColor = System.Drawing.Color.Lime;
            this.grdSlider.Name = "grdSlider";
            this.helpProvider1.SetShowHelp(this.grdSlider, ((bool)(resources.GetObject("grdSlider.ShowHelp"))));
            this.grdSlider.SlidersEnabled = true;
            this.grdSlider.StartValue = 0.2F;
            //
            // _spatialStatusStrip1
            //
            this._spatialStatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                                                                                                  this.statusText,
                                                                                                  this.statusProgress});
            resources.ApplyResources(this._spatialStatusStrip1, "_spatialStatusStrip1");
            this._spatialStatusStrip1.Name = "_spatialStatusStrip1";
            this.helpProvider1.SetShowHelp(this._spatialStatusStrip1, ((bool)(resources.GetObject("_spatialStatusStrip1.ShowHelp"))));
            //
            // statusText
            //
            this.statusText.Name = "statusText";
            resources.ApplyResources(this.statusText, "statusText");
            this.statusText.Spring = true;
            //
            // statusProgress
            //
            this.statusProgress.Name = "statusProgress";
            resources.ApplyResources(this.statusProgress, "statusProgress");
            //
            // tableLayoutPanel1
            //
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.dialogButtons1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.grdSlider, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.grpPreview, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.helpProvider1.SetShowHelp(this.tableLayoutPanel1, ((bool)(resources.GetObject("tableLayoutPanel1.ShowHelp"))));
            //
            // ColorPicker
            //
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this._spatialStatusStrip1);
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ColorPicker";
            this.helpProvider1.SetShowHelp(this, ((bool)(resources.GetObject("$this.ShowHelp"))));
            this.ShowInTaskbar = false;
            this.grpPreview.ResumeLayout(false);
            this._spatialStatusStrip1.ResumeLayout(false);
            this._spatialStatusStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the original instance for updating when the apply button is pressed.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IDescriptor Original
        {
            get { return _original; }
            set { _original = value; }
        }

        /// <summary>
        /// Gets or sets the start color for this dialog
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the start color for this dialog")]
        public Color LowColor
        {
            get { return grdSlider.MinimumColor; }
            set
            {
                grdSlider.MinimumColor = value;
                if (Visible) lblPreview.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the end color for this dialog
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the end color for this dialog")]
        public Color HighColor
        {
            get { return grdSlider.MaximumColor; }
            set
            {
                grdSlider.MaximumColor = value;
                if (Visible) lblPreview.Invalidate();
            }
        }

        /// <summary>
        /// Gets the IProgressHandler version of the status bar on this form
        /// </summary>
        public IProgressHandler ProgressHandler
        {
            get
            {
                return _spatialStatusStrip1;
            }
        }

        #endregion

        #region Event Handlers

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

        private void LblPreviewPaint(object sender, PaintEventArgs e)
        {
            UpdatePreview(e);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
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
            _original.CopyProperties(_rasterCategory);
            if (ChangesApplied != null)
            {
                ChangesApplied(this, EventArgs.Empty);
            }
        }
    }
}