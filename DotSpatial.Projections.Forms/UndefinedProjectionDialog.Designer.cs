namespace DotSpatial.Projections.Forms
{
    partial class UndefinedProjectionDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UndefinedProjectionDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.lblOriginal = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.radLatLong = new System.Windows.Forms.RadioButton();
            this.radSelectedTransform = new System.Windows.Forms.RadioButton();
            this.lblSelectedTransform = new System.Windows.Forms.Label();
            this.btnSelect = new System.Windows.Forms.Button();
            this.radMapFrame = new System.Windows.Forms.RadioButton();
            this.lblMapProjection = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.radDoNothing = new System.Windows.Forms.RadioButton();
            this.chkAlways = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // lblOriginal
            // 
            resources.ApplyResources(this.lblOriginal, "lblOriginal");
            this.lblOriginal.AutoEllipsis = true;
            this.lblOriginal.Name = "lblOriginal";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // radLatLong
            // 
            resources.ApplyResources(this.radLatLong, "radLatLong");
            this.radLatLong.Name = "radLatLong";
            this.radLatLong.UseVisualStyleBackColor = true;
            // 
            // radSelectedTransform
            // 
            resources.ApplyResources(this.radSelectedTransform, "radSelectedTransform");
            this.radSelectedTransform.Name = "radSelectedTransform";
            this.radSelectedTransform.UseVisualStyleBackColor = true;
            // 
            // lblSelectedTransform
            // 
            resources.ApplyResources(this.lblSelectedTransform, "lblSelectedTransform");
            this.lblSelectedTransform.AutoEllipsis = true;
            this.lblSelectedTransform.Name = "lblSelectedTransform";
            // 
            // btnSelect
            // 
            resources.ApplyResources(this.btnSelect, "btnSelect");
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // radMapFrame
            // 
            resources.ApplyResources(this.radMapFrame, "radMapFrame");
            this.radMapFrame.Checked = true;
            this.radMapFrame.Name = "radMapFrame";
            this.radMapFrame.TabStop = true;
            this.radMapFrame.UseVisualStyleBackColor = true;
            // 
            // lblMapProjection
            // 
            resources.ApplyResources(this.lblMapProjection, "lblMapProjection");
            this.lblMapProjection.AutoEllipsis = true;
            this.lblMapProjection.Name = "lblMapProjection";
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // radDoNothing
            // 
            resources.ApplyResources(this.radDoNothing, "radDoNothing");
            this.radDoNothing.Name = "radDoNothing";
            this.radDoNothing.UseVisualStyleBackColor = true;
            // 
            // chkAlways
            // 
            resources.ApplyResources(this.chkAlways, "chkAlways");
            this.chkAlways.Name = "chkAlways";
            this.chkAlways.UseVisualStyleBackColor = true;
            // 
            // UndefinedProjectionDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkAlways);
            this.Controls.Add(this.radDoNothing);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.lblMapProjection);
            this.Controls.Add(this.radMapFrame);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.lblSelectedTransform);
            this.Controls.Add(this.radSelectedTransform);
            this.Controls.Add(this.radLatLong);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblOriginal);
            this.Controls.Add(this.label1);
            this.Name = "UndefinedProjectionDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblOriginal;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radLatLong;
        private System.Windows.Forms.RadioButton radSelectedTransform;
        private System.Windows.Forms.Label lblSelectedTransform;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.RadioButton radMapFrame;
        private System.Windows.Forms.Label lblMapProjection;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RadioButton radDoNothing;
        private System.Windows.Forms.CheckBox chkAlways;
    }
}