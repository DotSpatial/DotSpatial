// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using DotSpatial.Controls;

namespace FlashShape
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.appManager1 = new DotSpatial.Controls.AppManager();
            this.btnFlashFirstShape = new System.Windows.Forms.Button();
            this.btnFlashFirstSelectedShape = new System.Windows.Forms.Button();
            this.btnFlashLastShape = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // appManager1
            // 
            this.appManager1.Directories = ((System.Collections.Generic.List<string>)(resources.GetObject("appManager1.Directories")));
            this.appManager1.DockManager = null;
            this.appManager1.HeaderControl = null;
            this.appManager1.Legend = null;
            this.appManager1.Map = null;
            this.appManager1.ProgressHandler = null;
            // 
            // btnFlashFirstShape
            // 
            this.btnFlashFirstShape.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFlashFirstShape.Location = new System.Drawing.Point(12, 365);
            this.btnFlashFirstShape.Name = "btnFlashFirstShape";
            this.btnFlashFirstShape.Size = new System.Drawing.Size(118, 23);
            this.btnFlashFirstShape.TabIndex = 0;
            this.btnFlashFirstShape.Text = "Flash First Shape";
            this.btnFlashFirstShape.UseVisualStyleBackColor = true;
            this.btnFlashFirstShape.Click += new System.EventHandler(this.btnFlashFirstShape_Click);
            // 
            // btnFlashFirstSelectedShape
            // 
            this.btnFlashFirstSelectedShape.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFlashFirstSelectedShape.Location = new System.Drawing.Point(12, 438);
            this.btnFlashFirstSelectedShape.Name = "btnFlashFirstSelectedShape";
            this.btnFlashFirstSelectedShape.Size = new System.Drawing.Size(118, 23);
            this.btnFlashFirstSelectedShape.TabIndex = 1;
            this.btnFlashFirstSelectedShape.Text = "Flash First Selected";
            this.btnFlashFirstSelectedShape.UseVisualStyleBackColor = true;
            this.btnFlashFirstSelectedShape.Click += new System.EventHandler(this.btnFlashFirstSelectedShape_Click);
            // 
            // btnFlashLastShape
            // 
            this.btnFlashLastShape.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFlashLastShape.Location = new System.Drawing.Point(12, 394);
            this.btnFlashLastShape.Name = "btnFlashLastShape";
            this.btnFlashLastShape.Size = new System.Drawing.Size(118, 23);
            this.btnFlashLastShape.TabIndex = 2;
            this.btnFlashLastShape.Text = "Flash Last Shape";
            this.btnFlashLastShape.UseVisualStyleBackColor = true;
            this.btnFlashLastShape.Click += new System.EventHandler(this.btnFlashLastShape_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(779, 503);
            this.Controls.Add(this.btnFlashLastShape);
            this.Controls.Add(this.btnFlashFirstSelectedShape);
            this.Controls.Add(this.btnFlashFirstShape);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        private AppManager appManager1;

        #endregion

        private Button btnFlashFirstShape;
        private Button btnFlashFirstSelectedShape;
        private Button btnFlashLastShape;
    }
}