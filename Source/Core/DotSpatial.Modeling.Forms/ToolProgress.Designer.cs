using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms
{
    public partial class ToolProgress
    {
        #region Windows Form Designer generated code
        private readonly IContainer components = null;
        private Button _btnCancel;
        private Label _lblTool;
        private ProgressBar _progressBarTool;
        private TextBox _txtBoxStatus;

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(ToolProgress));
            this._progressBarTool = new ProgressBar();
            this._lblTool = new Label();
            this._txtBoxStatus = new TextBox();
            this._btnCancel = new Button();
            this.SuspendLayout();

            // _progressBarTool
            this._progressBarTool.Anchor = (AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right));
            this._progressBarTool.Location = new System.Drawing.Point(12, 25);
            this._progressBarTool.Name = "_progressBarTool";
            this._progressBarTool.Size = new System.Drawing.Size(494, 23);
            this._progressBarTool.TabIndex = 0;

            // _lblTool
            this._lblTool.AutoSize = true;
            this._lblTool.Location = new System.Drawing.Point(12, 9);
            this._lblTool.Name = "_lblTool";
            this._lblTool.Size = new System.Drawing.Size(72, 13);
            this._lblTool.TabIndex = 3;
            this._lblTool.Text = "Tool Progress";

            // _txtBoxStatus
            this._txtBoxStatus.Anchor = (AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right));
            this._txtBoxStatus.Location = new System.Drawing.Point(13, 54);
            this._txtBoxStatus.Multiline = true;
            this._txtBoxStatus.Name = "_txtBoxStatus";
            this._txtBoxStatus.ScrollBars = ScrollBars.Vertical;
            this._txtBoxStatus.Size = new System.Drawing.Size(494, 323);
            this._txtBoxStatus.TabIndex = 4;

            // _btnCancel
            this._btnCancel.Anchor = (AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right));
            this._btnCancel.Location = new System.Drawing.Point(431, 383);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(75, 23);
            this._btnCancel.TabIndex = 5;
            this._btnCancel.Text = "Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            this._btnCancel.Click += new EventHandler(this.BtnCancelClick);

            // ToolProgress
            this.ClientSize = new System.Drawing.Size(519, 418);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._txtBoxStatus);
            this.Controls.Add(this._lblTool);
            this.Controls.Add(this._progressBarTool);
            this.Icon = (System.Drawing.Icon)(resources.GetObject("$this.Icon"));
            this.Name = "ToolProgress";
            this.Text = "Tool Progress";
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
