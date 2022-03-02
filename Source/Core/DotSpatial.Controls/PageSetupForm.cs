// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A dialog that allows users to modify the size of the layout paper and margins.
    /// </summary>
    public partial class PageSetupForm : Form
    {
        #region Fields
        private readonly PrinterSettings _printerSettings;
        private double _bottom;
        private GroupBox _groupBox2;
        private double _left;
        private RadioButton _rdbLandscape;
        private RadioButton _rdbPortrait;
        private double _right;
        private double _top;
        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PageSetupForm"/> class.
        /// </summary>
        /// <param name="settings">PrinterSettings used for printing.</param>
        public PageSetupForm(PrinterSettings settings)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            // Store the printer settings
            _printerSettings = settings;

            // Gets the list of available paper sizes
            comboPaperSizes.SuspendLayout();
            PrinterSettings.PaperSizeCollection paperSizes = settings.PaperSizes;
            foreach (PaperSize ps in paperSizes)
                comboPaperSizes.Items.Add(ps.PaperName);
            comboPaperSizes.SelectedItem = settings.DefaultPageSettings.PaperSize.PaperName;
            if (comboPaperSizes.SelectedIndex == -1) comboPaperSizes.SelectedIndex = 1;
            comboPaperSizes.ResumeLayout();

            // Gets the paper orientation
            if (settings.DefaultPageSettings.Landscape)
                _rdbLandscape.Checked = true;
            else
                _rdbPortrait.Checked = true;

            // Gets the margins
            _left = settings.DefaultPageSettings.Margins.Left / 100.0;
            txtBoxLeft.Text = $@"{_left:0.00}";
            _top = settings.DefaultPageSettings.Margins.Top / 100.0;
            txtBoxTop.Text = $@"{_top:0.00}";
            _bottom = settings.DefaultPageSettings.Margins.Bottom / 100.0;
            txtBoxBottom.Text = $@"{_bottom:0.00}";
            _right = settings.DefaultPageSettings.Margins.Right / 100.0;
            txtBoxRight.Text = $@"{_right:0.00}";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the printerSettings to the new settings and sets the result to OK.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        public void OkButtonClick(object sender, EventArgs e)
        {
            _printerSettings.DefaultPageSettings.PaperSize = _printerSettings.PaperSizes[comboPaperSizes.SelectedIndex];
            _printerSettings.DefaultPageSettings.Margins.Left = Convert.ToInt32(_left * 100);
            _printerSettings.DefaultPageSettings.Margins.Top = Convert.ToInt32(_top * 100);
            _printerSettings.DefaultPageSettings.Margins.Bottom = Convert.ToInt32(_bottom * 100);
            _printerSettings.DefaultPageSettings.Margins.Right = Convert.ToInt32(_right * 100);
            _printerSettings.DefaultPageSettings.Landscape = _rdbLandscape.Checked;
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Takes a string and returns true if it can be converted to a double.
        /// </summary>
        /// <param name="input">String that should be converted to double.</param>
        /// <returns>True, if string can be converted.</returns>
        private static bool IsValidMargin(string input)
        {
            try
            {
                Convert.ToDouble(input);
            }
            catch
            {
                return false;
            }

            if (Convert.ToDouble(input) < 0) return false;
            return true;
        }

        /// <summary>
        /// Disgards the settings and sets dialogresult to cancel.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void CancelButtonClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ComboPaperSizesSelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePaperSize();
        }

        private void RdbLandscapeCheckedChanged(object sender, EventArgs e)
        {
            UpdatePaperSize();
        }

        private void RdbPortraitCheckedChanged(object sender, EventArgs e)
        {
            UpdatePaperSize();
        }

        private void TxtBoxBottomLeave(object sender, EventArgs e)
        {
            if (IsValidMargin(txtBoxBottom.Text))
                _bottom = Convert.ToDouble(txtBoxBottom.Text);
            txtBoxBottom.Text = $@"{_bottom:0.00}";
        }

        private void TxtBoxLeftLeave(object sender, EventArgs e)
        {
            if (IsValidMargin(txtBoxLeft.Text))
                _left = Convert.ToDouble(txtBoxLeft.Text);
            txtBoxLeft.Text = $@"{_left:0.00}";
        }

        private void TxtBoxRightLeave(object sender, EventArgs e)
        {
            if (IsValidMargin(txtBoxRight.Text))
                _right = Convert.ToDouble(txtBoxRight.Text);
            txtBoxRight.Text = $@"{_right:0.00}";
        }

        private void TxtBoxTopLeave(object sender, EventArgs e)
        {
            if (IsValidMargin(txtBoxTop.Text))
                _top = Convert.ToDouble(txtBoxTop.Text);
            txtBoxTop.Text = $@"{_top:0.00}";
        }

        private void UpdatePaperSize()
        {
            if (comboPaperSizes.SelectedIndex == -1 && comboPaperSizes.Items.Count > 0) comboPaperSizes.SelectedIndex = 0;
            lblPaperDimension.Text = comboPaperSizes.SelectedIndex > -1 ? (_printerSettings.PaperSizes[comboPaperSizes.SelectedIndex].Width / 100) + "\" x " + (_printerSettings.PaperSizes[comboPaperSizes.SelectedIndex].Height / 100) + "\"" : string.Empty;
        }

        #endregion
    }
}