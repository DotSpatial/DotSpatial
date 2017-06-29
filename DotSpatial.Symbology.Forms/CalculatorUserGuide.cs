// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The core assembly for the DotSpatial 6.0 distribution.
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
        /// Creates a new instance of the calculator user guide form.
        /// </summary>
        public CalculatorUserGuide()
        {
            InitializeComponent();
        }

        private void CalculatorUserGuideLoad(object sender, EventArgs e)
        {
            richTextBox1.Text = "\n1) use always double click on the Fields or Function or opertationtion in order to add that in the Expression." +
                    "when you clik, It will put space front and back automaticaly( ).\n\n" + "2) use space to use constant or number ex. _3_  or  (_3_*_4_)_-_2  \n\n" +
                    "3) when you use function, you do need to close the bracket.\n\n" +
                    "eg. Abs(-222.34 )\n" + "or  3 * pow(Area, 2 )  this is equal to 3*pow(Area, 2)\n";
        }

        private void BtnCloseClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}