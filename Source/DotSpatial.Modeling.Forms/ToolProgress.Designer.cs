using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms
{
    public partial class ToolProgress
    {
        #region Windows Form Designer generated code
        private readonly IContainer components = null;
        System.ComponentModel.ComponentResourceManager resources;
        private Button _btnCancel;
        private Label _lblTool;
        private ProgressBar _progressBarTool;
        private TextBox _txtBoxStatus;

        private void InitializeComponent()
        {
            resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolProgress));
            this._progressBarTool = new System.Windows.Forms.ProgressBar();
            this._lblTool = new System.Windows.Forms.Label();
            this._txtBoxStatus = new System.Windows.Forms.TextBox();
            this._btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _progressBarTool
            // 
            resources.ApplyResources(this._progressBarTool, "_progressBarTool");
            this._progressBarTool.Name = "_progressBarTool";
            // 
            // _lblTool
            // 
            resources.ApplyResources(this._lblTool, "_lblTool");
            this._lblTool.Name = "_lblTool";
            // 
            // _txtBoxStatus
            // 
            resources.ApplyResources(this._txtBoxStatus, "_txtBoxStatus");
            this._txtBoxStatus.Name = "_txtBoxStatus";
            // 
            // _btnCancel
            // 
            resources.ApplyResources(this._btnCancel, "_btnCancel");
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            this._btnCancel.Click += new System.EventHandler(this.BtnCancelClick);
            // 
            // ToolProgress
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._txtBoxStatus);
            this.Controls.Add(this._lblTool);
            this.Controls.Add(this._progressBarTool);
            this.Name = "ToolProgress";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// Disposes of the resources (other than memory) used by the <see cref="T:System.Windows.Forms.Form"/>.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
