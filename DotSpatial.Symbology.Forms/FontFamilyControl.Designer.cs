// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created before 2010 refactoring.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// A control that is specifically designed to allow choosing a font family name
    /// </summary>
    partial class FontFamilyControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FontFamilyControl));
            this.ffdNames = new DotSpatial.Symbology.Forms.FontFamilyDropDown();
            this.SuspendLayout();
            // 
            // ffdNames
            // 
            resources.ApplyResources(this.ffdNames, "ffdNames");
            this.ffdNames.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ffdNames.FormattingEnabled = true;
            this.ffdNames.Name = "ffdNames";
            // 
            // FontFamilyControl
            // 
            this.AutoScaleMode = this.AutoScaleMode;
            this.Controls.Add(this.ffdNames);
            this.Name = "FontFamilyControl";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);

        }

        #endregion

        private FontFamilyDropDown ffdNames;
    }
}