// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The core assembly for the DotSpatial 6.0 distribution.
// ********************************************************************************************************
//
// The Original Code is DotSpatial.dll
//
// The Initial Developer of this Original Code is Kandasamy Prasanna. Created 09/18/2009
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
//  Name               Date                Comments
// ---------------------------------------------------------------------------------------------
// Ted Dunsford     |  9/19/2009    |  Added some xml comments
// ********************************************************************************************************

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