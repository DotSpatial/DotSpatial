namespace DotSpatial.Plugins.ShapeEditor
{
    partial class SnapSettingsDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.cbPerformSnap = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbPerformSnap
            // 
            this.cbPerformSnap.AutoSize = true;
            this.cbPerformSnap.Checked = true;
            this.cbPerformSnap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPerformSnap.Location = new System.Drawing.Point(12, 12);
            this.cbPerformSnap.Name = "cbPerformSnap";
            this.cbPerformSnap.Size = new System.Drawing.Size(110, 17);
            this.cbPerformSnap.TabIndex = 0;
            this.cbPerformSnap.Text = "Perform Snapping";
            this.cbPerformSnap.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(197, 70);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // SnapSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 105);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.cbPerformSnap);
            this.Name = "SnapSettings";
            this.Text = "Snap Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbPerformSnap;
        private System.Windows.Forms.Button btnSave;
    }
}