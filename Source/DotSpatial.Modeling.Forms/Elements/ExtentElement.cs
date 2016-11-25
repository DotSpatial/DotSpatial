// ********************************************************************************************************
// Product Name: DotSpatial.dll Alpha
// Description:  A library module for the DotSpatial geospatial framework for .Net.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/25/2009 1:41:29 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;
using DotSpatial.Data;
using DotSpatial.Data.Forms;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Modeling.Forms.Elements
{
    /// <summary>
    /// ProjectionElement
    /// </summary>
    internal class ExtentElement : DialogElement
    {
        #region Private Variables

        private readonly ToolTip _tthelp;
        private Button cmdSelect;
        private Label lblExtent;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ProjectionElement
        /// </summary>
        public ExtentElement(ExtentParam value)
        {
            _tthelp = new ToolTip();
            base.Param = value;
            InitializeComponent();
            if (value == null) return;
            GroupBox.Text = value.Name;
            if (value.Value != null)
            {
                lblExtent.Text = value.Value.ToString();
            }
            value.ValueChanged += ParamValueChanged;
        }

        protected override void ParamValueChanged(Parameter sender)
        {
            if (Param == null) return;
            if (Param.Value == null)
            {
                lblExtent.Text = string.Empty;
                return;
            }
            lblExtent.Text = Param.Value.ToString();
            base.ParamValueChanged(sender);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Parameter that the element represents
        /// </summary>
        public new ExtentParam Param
        {
            get { return (ExtentParam)base.Param; }
            set
            {
                Status = ToolStatus.Empty;
                LightTipText = ModelingMessageStrings.ParameterInvalid;
                base.Param = value;
                if (value == null) return;
                if (value.Value == null) return;
                lblExtent.Text = value.Value.ToString();
                Status = ToolStatus.Ok;
                LightTipText = ModelingMessageStrings.ParameterValid;
                Invalidate();
            }
        }

        #endregion

        private void InitializeComponent()
        {
            lblExtent = new Label();
            cmdSelect = new Button();
            SuspendLayout();
            //
            // groupBox1
            //
            GroupBox.Controls.Add(lblExtent);
            GroupBox.Controls.Add(cmdSelect);
            GroupBox.Controls.SetChildIndex(cmdSelect, 0);
            GroupBox.Controls.SetChildIndex(lblExtent, 0);
            GroupBox.Controls.SetChildIndex(StatusLabel, 0);
            //
            // lblProjection
            //
            lblExtent.Anchor = (AnchorStyles.Top | AnchorStyles.Left)
                                   | AnchorStyles.Right;
            lblExtent.BackColor = Color.White;
            lblExtent.BorderStyle = BorderStyle.Fixed3D;
            lblExtent.Location = new Point(39, 16);
            lblExtent.Name = "lblExtent";
            lblExtent.Size = new Size(405, 20);
            lblExtent.TabIndex = 2;
            lblExtent.Text = ModelingMessageStrings.ExtentElement_Press_button;
            //
            // cmdSelect
            //
            cmdSelect.Location = new Point(450, 15);
            cmdSelect.Name = "cmdSelect";
            cmdSelect.Size = new Size(36, 23);
            cmdSelect.TabIndex = 3;
            cmdSelect.Text = "...";
            cmdSelect.UseVisualStyleBackColor = true;
            cmdSelect.Click += CmdSelectClick;
            //
            // ProjectionElement
            //
            AutoScaleDimensions = new SizeF(6F, 13F);
            Name = "ProjectionElement";
            ResumeLayout(false);
        }

        private void CmdSelectClick(object sender, EventArgs e)
        {
            ExtentDialog dlg = new ExtentDialog();

            Extent ext = null;
            if (Param != null) ext = Param.Value;
            dlg.Extent = ext; // Null clears the dialog, otherwise we use the specified values.

            if (dlg.ShowDialog() != DialogResult.OK) return;
            if (Param == null)
            {
                Param = new ExtentParam("extent", dlg.Extent);
            }
            else
            {
                Param.Value = dlg.Extent;
            }
            lblExtent.Text = Param.Value.ToString();
            _tthelp.SetToolTip(lblExtent, Param.Value.ToString());
        }

        public override void Refresh()
        {
            Status = ToolStatus.Empty;
            LightTipText = ModelingMessageStrings.ParameterInvalid;
            if (Param == null) return;
            if (Param.Value == null) return;
            Status = ToolStatus.Ok;
            LightTipText = ModelingMessageStrings.ParameterValid;
        }
    }
}