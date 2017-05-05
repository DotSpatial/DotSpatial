// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/20/2008 9:29:29 AM
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
    /// FontBox
    /// </summary>
    [DefaultProperty("Value")]
    public partial class FontBox : UserControl
    {
        #region Fields

        private readonly FontDialog _fontDialog;
        private Font _value;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FontBox"/> class.
        /// </summary>
        public FontBox()
        {
            InitializeComponent();
            _fontDialog = new FontDialog();
            txtFont.Text = Value.ToString();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the label text for this control.
        /// </summary>
        [Category("Text")]
        [Description("Gets or sets the label text for this control.")]
        public string LabelText
        {
            get
            {
                return lblFont.Text;
            }

            set
            {
                lblFont.Text = value;
                Reset();
            }
        }

        /// <summary>
        /// Gets or sets the font that this control should be using.
        /// </summary>
        public Font Value
        {
            get
            {
                return _value ?? (_value = new Font("Microsoft Sans Serif", 9));
            }

            set
            {
                _value = value;
                if (_value != null)
                {
                    txtFont.Text = _value.ToString();
                }
            }
        }

        #endregion

        #region Methods

        private void CmdShowDialogClick(object sender, EventArgs e)
        {
            if (_fontDialog.ShowDialog(ParentForm) != DialogResult.OK) return;

            Value = _fontDialog.Font;
        }

        /// <summary>
        /// Changes the starting location of the color drop down based on the current text.
        /// </summary>
        private void Reset()
        {
            txtFont.Left = lblFont.Width + 5;
            txtFont.Width = cmdShowDialog.Left - txtFont.Left - 10;
        }

        #endregion
    }
}