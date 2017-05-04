// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 9/22/2009 11:21:01 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// LabelAlignmentControl
    /// </summary>
    [DefaultEvent("ValueChanged")]
    [DefaultProperty("Value")]
    [ToolboxItem(false)]
    public partial class LabelAlignmentControl : UserControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelAlignmentControl"/> class.
        /// </summary>
        public LabelAlignmentControl()
        {
            InitializeComponent();
            Height = 25;
            labelAlignmentPicker1.Visible = false;
            labelAlignmentPicker1.ValueChanged += LabelAlignmentPicker1ValueChanged;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs after the drop-down has been used to select a value.
        /// </summary>
        public event EventHandler ValueChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the actual value currently being represented in the control.
        /// </summary>
        [Category("Data")]
        [Description("Gets or sets the actual value currently being represented in the control.")]
        public ContentAlignment Value
        {
            get
            {
                if (labelAlignmentPicker1 == null) return ContentAlignment.MiddleCenter;
                return labelAlignmentPicker1.Value;
            }

            set
            {
                labelAlignmentPicker1.Value = value;
                lblAlignmentText.Text = value.ToString();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Occurs when the value is changed and fires the ValueChanged event.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        protected virtual void OnValueChanged(object sender, EventArgs e)
        {
            ValueChanged?.Invoke(sender, e);
        }

        private void BtnDropClick(object sender, EventArgs e)
        {
            labelAlignmentPicker1.Visible = true;
            Height = 112;
        }

        private void LabelAlignmentPicker1ValueChanged(object sender, EventArgs e)
        {
            lblAlignmentText.Text = labelAlignmentPicker1.Value.ToString();
            Height = 25;
            labelAlignmentPicker1.Visible = false;
            OnValueChanged(sender, e);
        }

        #endregion
    }
}