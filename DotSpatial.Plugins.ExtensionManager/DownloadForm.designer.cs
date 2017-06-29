namespace DotSpatial.Plugins.ExtensionManager
{
    partial class DownloadForm
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
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.uxDownloadStatus = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(1, 65);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(364, 23);
            this.progressBar.TabIndex = 0;
            // 
            // uxDownloadStatus
            // 
            this.uxDownloadStatus.Location = new System.Drawing.Point(1, 2);
            this.uxDownloadStatus.Name = "uxDownloadStatus";
            this.uxDownloadStatus.ReadOnly = true;
            this.uxDownloadStatus.Size = new System.Drawing.Size(364, 57);
            this.uxDownloadStatus.TabIndex = 1;
            this.uxDownloadStatus.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(280, 95);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // DownloadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 130);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.uxDownloadStatus);
            this.Controls.Add(this.progressBar);
            this.Name = "DownloadForm";
            this.Text = "Download progress";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.RichTextBox uxDownloadStatus;
        private System.Windows.Forms.Button button1;

    }
}