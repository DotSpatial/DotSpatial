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
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// A class giving information on how to use the calculator functions.
    /// </summary>
    partial class CalculatorUserGuide
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(CalculatorUserGuide));
            this.richTextBox1 = new RichTextBox();
            this.label1 = new Label();
            this.btnClose = new Button();
            this.SuspendLayout();
            //
            // richTextBox1
            //
            this.richTextBox1.AccessibleDescription = null;
            this.richTextBox1.AccessibleName = null;
            resources.ApplyResources(this.richTextBox1, "richTextBox1");
            this.richTextBox1.BackgroundImage = null;
            this.richTextBox1.Name = "richTextBox1";
            //
            // label1
            //
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            //
            // btnClose
            //
            this.btnClose.AccessibleDescription = null;
            this.btnClose.AccessibleName = null;
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.BackgroundImage = null;
            this.btnClose.Font = null;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new EventHandler(this.BtnCloseClick);
            //
            // CalculatorUserGuide
            //
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = ((ContainerControl)this).AutoScaleMode;
            this.BackgroundImage = null;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBox1);
            this.Font = null;
            this.Icon = null;
            this.Name = "CalculatorUserGuide";
            this.ShowIcon = false;
            this.Load += new EventHandler(this.CalculatorUserGuideLoad);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private RichTextBox richTextBox1;
        private Label label1;
        private Button btnClose;
    }
}