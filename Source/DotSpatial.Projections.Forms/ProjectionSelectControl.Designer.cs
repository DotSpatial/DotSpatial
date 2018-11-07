using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Projections.Forms
{
    partial class ProjectionSelectControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectionSelectControl));
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
            resources.ApplyResources(this.grpType, "grpType");
            this.grpType.Controls.Add(this.radGeographic);
            this.grpType.Controls.Add(this.radProjected);
            this.grpType.Name = "grpType";
            this.grpType.TabStop = false;
            // 
            // radGeographic
            // 
            resources.ApplyResources(this.radGeographic, "radGeographic");
            this.radGeographic.Name = "radGeographic";
            this.radGeographic.UseVisualStyleBackColor = true;
            // 
            // radProjected
            // 
            resources.ApplyResources(this.radProjected, "radProjected");
            this.radProjected.Name = "radProjected";
            this.radProjected.UseVisualStyleBackColor = true;
            this.radProjected.CheckedChanged += new System.EventHandler(this.RadProjectedCheckedChanged);
            // 
            // cmbMinorCategory
            // 
            resources.ApplyResources(this.cmbMinorCategory, "cmbMinorCategory");
            this.cmbMinorCategory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbMinorCategory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbMinorCategory.FormattingEnabled = true;
            this.cmbMinorCategory.Name = "cmbMinorCategory";
            this.cmbMinorCategory.SelectedIndexChanged += new System.EventHandler(this.CmbMinorCategorySelectedIndexChanged);
            // 
            // cmbMajorCategory
            // 
            resources.ApplyResources(this.cmbMajorCategory, "cmbMajorCategory");
            this.cmbMajorCategory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbMajorCategory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbMajorCategory.FormattingEnabled = true;
            this.cmbMajorCategory.Name = "cmbMajorCategory";
            this.cmbMajorCategory.SelectedIndexChanged += new System.EventHandler(this.CmbMajorCategorySelectedIndexChanged);
            // 
            // nudEpsgCode
            // 
            resources.ApplyResources(this.nudEpsgCode, "nudEpsgCode");
            this.nudEpsgCode.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudEpsgCode.Name = "nudEpsgCode";
            // 
            // btnFromEpsgCode
            // 
            resources.ApplyResources(this.btnFromEpsgCode, "btnFromEpsgCode");
            this.btnFromEpsgCode.Name = "btnFromEpsgCode";
            this.btnFromEpsgCode.UseVisualStyleBackColor = true;
            this.btnFromEpsgCode.Click += new System.EventHandler(this.BtnFromEpsgCodeClick);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.cmbMinorCategory);
            this.groupBox1.Controls.Add(this.cmbMajorCategory);
            this.groupBox1.Controls.Add(this.grpType);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.btnFromEpsgCode);
            this.groupBox2.Controls.Add(this.nudEpsgCode);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.chbEsri);
            this.groupBox3.Controls.Add(this.tbEsriProj4);
            this.groupBox3.Controls.Add(this.btnUseESRI);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // chbEsri
            // 
            resources.ApplyResources(this.chbEsri, "chbEsri");
            this.chbEsri.Name = "chbEsri";
            this.chbEsri.UseVisualStyleBackColor = true;
            this.chbEsri.CheckedChanged += new System.EventHandler(this.ChbEsriCheckedChanged);
            // 
            // tbEsriProj4
            // 
            resources.ApplyResources(this.tbEsriProj4, "tbEsriProj4");
            this.tbEsriProj4.Name = "tbEsriProj4";
            // 
            // btnUseESRI
            // 
            resources.ApplyResources(this.btnUseESRI, "btnUseESRI");
            this.btnUseESRI.Name = "btnUseESRI";
            this.btnUseESRI.UseVisualStyleBackColor = true;
            this.btnUseESRI.Click += new System.EventHandler(this.BtnUseEsriClick);
            // 
            // ProjectionSelectControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "ProjectionSelectControl";
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

        private GroupBox grpType;
        private RadioButton radGeographic;
        private RadioButton radProjected;
        private ComboBox cmbMinorCategory;
        private ComboBox cmbMajorCategory;
        private NumericUpDown nudEpsgCode;
        private Button btnFromEpsgCode;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private TextBox tbEsriProj4;
        private Button btnUseESRI;
        private CheckBox chbEsri;
    }
}
