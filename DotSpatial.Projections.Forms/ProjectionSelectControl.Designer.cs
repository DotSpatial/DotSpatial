namespace DotSpatial.Projections.Forms
{
    partial class ProjectionSelectControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpType = new System.Windows.Forms.GroupBox();
            this.radGeographic = new System.Windows.Forms.RadioButton();
            this.radProjected = new System.Windows.Forms.RadioButton();
            this.cmbMinorCategory = new System.Windows.Forms.ComboBox();
            this.cmbMajorCategory = new System.Windows.Forms.ComboBox();
            this.nudEpsgCode = new System.Windows.Forms.NumericUpDown();
            this.btnFromEpsgCode = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chbEsri = new System.Windows.Forms.CheckBox();
            this.tbEsriProj4 = new System.Windows.Forms.TextBox();
            this.btnUseESRI = new System.Windows.Forms.Button();
            this.grpType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEpsgCode)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpType
            // 
            this.grpType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpType.Controls.Add(this.radGeographic);
            this.grpType.Controls.Add(this.radProjected);
            this.grpType.Location = new System.Drawing.Point(6, 19);
            this.grpType.Name = "grpType";
            this.grpType.Size = new System.Drawing.Size(281, 48);
            this.grpType.TabIndex = 4;
            this.grpType.TabStop = false;
            this.grpType.Text = "Projection Type";
            // 
            // radGeographic
            // 
            this.radGeographic.AutoSize = true;
            this.radGeographic.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radGeographic.Location = new System.Drawing.Point(100, 21);
            this.radGeographic.Name = "radGeographic";
            this.radGeographic.Size = new System.Drawing.Size(80, 17);
            this.radGeographic.TabIndex = 1;
            this.radGeographic.Text = "&Geographic";
            this.radGeographic.UseVisualStyleBackColor = true;
            // 
            // radProjected
            // 
            this.radProjected.AutoSize = true;
            this.radProjected.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radProjected.Location = new System.Drawing.Point(24, 21);
            this.radProjected.Name = "radProjected";
            this.radProjected.Size = new System.Drawing.Size(70, 17);
            this.radProjected.TabIndex = 0;
            this.radProjected.Text = "&Projected";
            this.radProjected.UseVisualStyleBackColor = true;
            this.radProjected.CheckedChanged += new System.EventHandler(this.radProjected_CheckedChanged);
            // 
            // cmbMinorCategory
            // 
            this.cmbMinorCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbMinorCategory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbMinorCategory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbMinorCategory.FormattingEnabled = true;
            this.cmbMinorCategory.Location = new System.Drawing.Point(6, 100);
            this.cmbMinorCategory.Name = "cmbMinorCategory";
            this.cmbMinorCategory.Size = new System.Drawing.Size(282, 21);
            this.cmbMinorCategory.TabIndex = 6;
            this.cmbMinorCategory.SelectedIndexChanged += new System.EventHandler(this.cmbMinorCategory_SelectedIndexChanged);
            // 
            // cmbMajorCategory
            // 
            this.cmbMajorCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbMajorCategory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbMajorCategory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbMajorCategory.FormattingEnabled = true;
            this.cmbMajorCategory.Location = new System.Drawing.Point(6, 73);
            this.cmbMajorCategory.Name = "cmbMajorCategory";
            this.cmbMajorCategory.Size = new System.Drawing.Size(281, 21);
            this.cmbMajorCategory.TabIndex = 5;
            this.cmbMajorCategory.SelectedIndexChanged += new System.EventHandler(this.cmbMajorCategory_SelectedIndexChanged);
            // 
            // nudEpsgCode
            // 
            this.nudEpsgCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nudEpsgCode.Location = new System.Drawing.Point(9, 19);
            this.nudEpsgCode.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudEpsgCode.Name = "nudEpsgCode";
            this.nudEpsgCode.Size = new System.Drawing.Size(200, 20);
            this.nudEpsgCode.TabIndex = 3;
            // 
            // btnFromEpsgCode
            // 
            this.btnFromEpsgCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFromEpsgCode.Location = new System.Drawing.Point(215, 19);
            this.btnFromEpsgCode.Name = "btnFromEpsgCode";
            this.btnFromEpsgCode.Size = new System.Drawing.Size(72, 23);
            this.btnFromEpsgCode.TabIndex = 8;
            this.btnFromEpsgCode.Text = "Find";
            this.btnFromEpsgCode.UseVisualStyleBackColor = true;
            this.btnFromEpsgCode.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cmbMinorCategory);
            this.groupBox1.Controls.Add(this.cmbMajorCategory);
            this.groupBox1.Controls.Add(this.grpType);
            this.groupBox1.Location = new System.Drawing.Point(9, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(293, 133);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Predefined";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnFromEpsgCode);
            this.groupBox2.Controls.Add(this.nudEpsgCode);
            this.groupBox2.Location = new System.Drawing.Point(9, 154);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(293, 55);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "From EPGS code";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.chbEsri);
            this.groupBox3.Controls.Add(this.tbEsriProj4);
            this.groupBox3.Controls.Add(this.btnUseESRI);
            this.groupBox3.Location = new System.Drawing.Point(9, 215);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(293, 146);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "From ESRI or Proj4 string";
            // 
            // chbEsri
            // 
            this.chbEsri.AutoSize = true;
            this.chbEsri.Location = new System.Drawing.Point(9, 20);
            this.chbEsri.Name = "chbEsri";
            this.chbEsri.Size = new System.Drawing.Size(132, 17);
            this.chbEsri.TabIndex = 10;
            this.chbEsri.Text = "ESRI (otherwise Proj4)";
            this.chbEsri.UseVisualStyleBackColor = true;
            this.chbEsri.CheckedChanged += new System.EventHandler(this.chbEsri_CheckedChanged);
            // 
            // tbEsriProj4
            // 
            this.tbEsriProj4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbEsriProj4.Location = new System.Drawing.Point(6, 50);
            this.tbEsriProj4.Multiline = true;
            this.tbEsriProj4.Name = "tbEsriProj4";
            this.tbEsriProj4.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbEsriProj4.Size = new System.Drawing.Size(281, 90);
            this.tbEsriProj4.TabIndex = 9;
            // 
            // btnUseESRI
            // 
            this.btnUseESRI.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUseESRI.Location = new System.Drawing.Point(215, 16);
            this.btnUseESRI.Name = "btnUseESRI";
            this.btnUseESRI.Size = new System.Drawing.Size(72, 23);
            this.btnUseESRI.TabIndex = 8;
            this.btnUseESRI.Text = "Parse";
            this.btnUseESRI.UseVisualStyleBackColor = true;
            this.btnUseESRI.Click += new System.EventHandler(this.button1_Click);
            // 
            // ProjectionSelectControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "ProjectionSelectControl";
            this.Size = new System.Drawing.Size(309, 364);
            this.grpType.ResumeLayout(false);
            this.grpType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEpsgCode)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpType;
        private System.Windows.Forms.RadioButton radGeographic;
        private System.Windows.Forms.RadioButton radProjected;
        private System.Windows.Forms.ComboBox cmbMinorCategory;
        private System.Windows.Forms.ComboBox cmbMajorCategory;
        private System.Windows.Forms.NumericUpDown nudEpsgCode;
        private System.Windows.Forms.Button btnFromEpsgCode;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox tbEsriProj4;
        private System.Windows.Forms.Button btnUseESRI;
        private System.Windows.Forms.CheckBox chbEsri;
    }
}
