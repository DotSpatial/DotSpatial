namespace DotSpatial.Controls
{
    partial class OptionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.chkZoomOutFartherThanMaxExtent = new System.Windows.Forms.CheckBox();
            this.btOk = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // chkZoomOutFartherThanMaxExtent
            // 
            resources.ApplyResources(this.chkZoomOutFartherThanMaxExtent, "chkZoomOutFartherThanMaxExtent");
            this.chkZoomOutFartherThanMaxExtent.Name = "chkZoomOutFartherThanMaxExtent";
            this.toolTip1.SetToolTip(this.chkZoomOutFartherThanMaxExtent, resources.GetString("chkZoomOutFartherThanMaxExtent.ToolTip"));
            this.chkZoomOutFartherThanMaxExtent.UseVisualStyleBackColor = true;
            // 
            // btOk
            // 
            resources.ApplyResources(this.btOk, "btOk");
            this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOk.Name = "btOk";
            this.toolTip1.SetToolTip(this.btOk, resources.GetString("btOk.ToolTip"));
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // btCancel
            // 
            resources.ApplyResources(this.btCancel, "btCancel");
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Name = "btCancel";
            this.toolTip1.SetToolTip(this.btCancel, resources.GetString("btCancel.ToolTip"));
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // OptionsForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.chkZoomOutFartherThanMaxExtent);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkZoomOutFartherThanMaxExtent;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}