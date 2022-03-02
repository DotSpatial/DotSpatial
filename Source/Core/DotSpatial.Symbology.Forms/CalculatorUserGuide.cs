// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// This form is strictly designed to show some helpful information about using a calculator.
    /// </summary>
    public partial class CalculatorUserGuide : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatorUserGuide"/> class.
        /// </summary>
        public CalculatorUserGuide()
        {
            InitializeComponent();
        }

        private void CalculatorUserGuideLoad(object sender, EventArgs e)
        {
            richTextBox1.Text = SymbologyFormsMessageStrings.CalculatorUserGuide_RichTextboxText;
        }

        private void BtnCloseClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}