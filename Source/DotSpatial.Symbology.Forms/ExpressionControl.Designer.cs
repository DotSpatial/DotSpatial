using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    partial class ExpressionControl
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(ExpressionControl));
            this.lblFields = new Label();
            this.lblSelectPrecursor = new Label();
            this.rtbExpression = new RichTextBox();
            this.groupBox1 = new GroupBox();
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.label5 = new Label();
            this.label6 = new Label();
            this.label7 = new Label();
            this.label8 = new Label();
            this.label9 = new Label();
            this.label4 = new Label();
            this.label3 = new Label();
            this.label10 = new Label();
            this.label2 = new Label();
            this.label11 = new Label();
            this.label12 = new Label();
            this.label13 = new Label();
            this.groupBox2 = new GroupBox();
            this.label1 = new Label();
            this.btValidate = new Button();
            this.btNewLine = new Button();
            this.lblResult = new Label();
            this.label14 = new Label();
            this.dgvFields = new DataGridView();
            this.dgvcName = new DataGridViewTextBoxColumn();
            this.dgvcType = new DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((ISupportInitialize)(this.dgvFields)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFields
            // 
            resources.ApplyResources(this.lblFields, "lblFields");
            this.lblFields.Name = "lblFields";
            // 
            // lblSelectPrecursor
            // 
            resources.ApplyResources(this.lblSelectPrecursor, "lblSelectPrecursor");
            this.lblSelectPrecursor.Name = "lblSelectPrecursor";
            // 
            // rtbExpression
            // 
            this.rtbExpression.AcceptsTab = true;
            resources.ApplyResources(this.rtbExpression, "rtbExpression");
            this.rtbExpression.Name = "rtbExpression";
            this.rtbExpression.KeyDown += new KeyEventHandler(this.rtbExpression_KeyDown);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label9, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.label10, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label11, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label12, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label13, 1, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btValidate
            // 
            resources.ApplyResources(this.btValidate, "btValidate");
            this.btValidate.Name = "btValidate";
            this.btValidate.UseVisualStyleBackColor = true;
            this.btValidate.Click += new EventHandler(this.btValidate_Click);
            // 
            // btNewLine
            // 
            resources.ApplyResources(this.btNewLine, "btNewLine");
            this.btNewLine.Name = "btNewLine";
            this.btNewLine.UseVisualStyleBackColor = true;
            this.btNewLine.Click += new EventHandler(this.btNewLine_Click);
            // 
            // lblResult
            // 
            resources.ApplyResources(this.lblResult, "lblResult");
            this.lblResult.Name = "lblResult";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // dgvFields
            // 
            this.dgvFields.AllowUserToAddRows = false;
            this.dgvFields.AllowUserToDeleteRows = false;
            this.dgvFields.AllowUserToResizeColumns = false;
            this.dgvFields.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dgvFields, "dgvFields");
            this.dgvFields.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvFields.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFields.Columns.AddRange(new DataGridViewColumn[] {
            this.dgvcName,
            this.dgvcType});
            this.dgvFields.EditMode = DataGridViewEditMode.EditProgrammatically;
            this.dgvFields.MultiSelect = false;
            this.dgvFields.Name = "dgvFields";
            this.dgvFields.ReadOnly = true;
            this.dgvFields.RowHeadersVisible = false;
            this.dgvFields.ShowEditingIcon = false;
            this.dgvFields.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.dgvFields_CellMouseDoubleClick);
            // 
            // dgvcName
            // 
            this.dgvcName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.dgvcName, "dgvcName");
            this.dgvcName.Name = "dgvcName";
            this.dgvcName.ReadOnly = true;
            // 
            // dgvcType
            // 
            this.dgvcType.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.dgvcType, "dgvcType");
            this.dgvcType.Name = "dgvcType";
            this.dgvcType.ReadOnly = true;
            // 
            // ExpressionControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.dgvFields);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.btNewLine);
            this.Controls.Add(this.btValidate);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblFields);
            this.Controls.Add(this.lblSelectPrecursor);
            this.Controls.Add(this.rtbExpression);
            this.Name = "ExpressionControl";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((ISupportInitialize)(this.dgvFields)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblFields;
        private Label lblSelectPrecursor;
        private RichTextBox rtbExpression;
        private GroupBox groupBox1;
        private TableLayoutPanel tableLayoutPanel1;
        private Label label3;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
        private Label label4;
        private GroupBox groupBox2;
        private Label label1;
        private Label label2;
        private Label label11;
        private Label label12;
        private Label label13;
        private Button btValidate;
        private Button btNewLine;
        private Label lblResult;
        private Label label14;
        private DataGridView dgvFields;
        private DataGridViewTextBoxColumn dgvcName;
        private DataGridViewTextBoxColumn dgvcType;
    }
}
