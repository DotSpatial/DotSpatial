using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    public partial class ProgressCancelDialog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressCancelDialog));
            this.mwProgressBar1 = new DotSpatial.Symbology.Forms.SymbologyProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.lblProgressText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // mwProgressBar1
            //
            resources.ApplyResources(this.mwProgressBar1, "mwProgressBar1");
            this.mwProgressBar1.FontColor = System.Drawing.Color.Black;
            this.mwProgressBar1.Name = "mwProgressBar1";
            this.mwProgressBar1.ShowMessage = true;
            //
            // button1
            //
            resources.ApplyResources(this.button1, "button1");
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1Click);
            //
            // lblProgressText
            //
            resources.ApplyResources(this.lblProgressText, "lblProgressText");
            this.lblProgressText.Name = "lblProgressText";
            //
            // ProgressCancelDialog
            //
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.lblProgressText);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.mwProgressBar1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressCancelDialog";
            this.ShowIcon = false;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Button button1;
        private Label lblProgressText;
        private SymbologyProgressBar mwProgressBar1;
    }
}