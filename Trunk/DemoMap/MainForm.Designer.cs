// ********************************************************************************************************
// Product Name: TestViewer.exe
// Description:  A very basic demonstration of the controls.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Initial Developer of this Original Code is Ted Dunsford. Created during refactoring 2010.
// ********************************************************************************************************

using System.ComponentModel;
using DotSpatial.Controls;

namespace DemoMap
{
    /// <summary>
    /// Form
    /// </summary>
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.appManager = new DotSpatial.Controls.AppManager();
            this.SuspendLayout();
            // 
            // appManager
            // 
            this.appManager.Directories = ((System.Collections.Generic.List<string>)(resources.GetObject("appManager.Directories")));
            this.appManager.DockManager = null;
            this.appManager.HeaderControl = null;
            this.appManager.Legend = null;
            this.appManager.Map = null;
            this.appManager.ProgressHandler = null;
            this.appManager.ShowExtensionsDialogMode = DotSpatial.Controls.ShowExtensionsDialogMode.Default;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(857, 506);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "DemoMap";
            this.ResumeLayout(false);

        }

        #endregion

        private AppManager appManager;
    }
}