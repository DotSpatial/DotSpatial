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
using System.Windows.Forms;
using DotSpatial.Data;
using DotSpatial.Data.Forms;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Modeling.Forms.Elements
{
    /// <summary>
    /// ExtentElement
    /// </summary>
    internal partial class ExtentElement : DialogElement
    {
        #region Fields
        private readonly ToolTip _tthelp;
        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtentElement"/> class.
        /// </summary>
        /// <param name="value">The ExtentParam</param>
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

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Parameter that the element represents
        /// </summary>
        public new ExtentParam Param
        {
            get
            {
                return (ExtentParam)base.Param;
            }

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

        #region Methods

        /// <inheritdoc />
        public override void Refresh()
        {
            Status = ToolStatus.Empty;
            LightTipText = ModelingMessageStrings.ParameterInvalid;
            if (Param == null) return;
            if (Param.Value == null) return;
            Status = ToolStatus.Ok;
            LightTipText = ModelingMessageStrings.ParameterValid;
        }

        /// <inheritdoc />
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

        #endregion
    }
}