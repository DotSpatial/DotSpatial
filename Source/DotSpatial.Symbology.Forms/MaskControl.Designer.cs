namespace DotSpatial.Symbology.Forms
{
    partial class MaskControl
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.cb_UseMask = new System.Windows.Forms.CheckBox();
            this.tv_layers = new System.Windows.Forms.TreeView();
            this.groupBox_Layers = new System.Windows.Forms.GroupBox();
            this.groupBox_Margins = new System.Windows.Forms.GroupBox();
            this.doubleBox_TopMargin = new DotSpatial.Projections.Forms.DoubleBox();
            this.doubleBox_BottomMargin = new DotSpatial.Projections.Forms.DoubleBox();
            this.doubleBox_RightMargin = new DotSpatial.Projections.Forms.DoubleBox();
            this.doubleBox_LeftMargin = new DotSpatial.Projections.Forms.DoubleBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox_Layers.SuspendLayout();
            this.groupBox_Margins.SuspendLayout();
            this.SuspendLayout();
            // 
            // cb_UseMask
            // 
            this.cb_UseMask.AutoSize = true;
            this.cb_UseMask.Location = new System.Drawing.Point(14, 10);
            this.cb_UseMask.Name = "cb_UseMask";
            this.cb_UseMask.Size = new System.Drawing.Size(73, 17);
            this.cb_UseMask.TabIndex = 0;
            this.cb_UseMask.Text = "Use mask";
            this.cb_UseMask.UseVisualStyleBackColor = true;
            this.cb_UseMask.CheckedChanged += new System.EventHandler(this.cb_UseMask_CheckedChanged);
            // 
            // tv_layers
            // 
            this.tv_layers.CheckBoxes = true;
            this.tv_layers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tv_layers.Location = new System.Drawing.Point(3, 16);
            this.tv_layers.Name = "tv_layers";
            this.tv_layers.Size = new System.Drawing.Size(545, 227);
            this.tv_layers.TabIndex = 2;
            this.tv_layers.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tv_layers_AfterCheck);
            // 
            // groupBox_Layers
            // 
            this.groupBox_Layers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_Layers.Controls.Add(this.tv_layers);
            this.groupBox_Layers.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_Layers.Location = new System.Drawing.Point(10, 167);
            this.groupBox_Layers.Name = "groupBox_Layers";
            this.groupBox_Layers.Size = new System.Drawing.Size(551, 246);
            this.groupBox_Layers.TabIndex = 3;
            this.groupBox_Layers.TabStop = false;
            this.groupBox_Layers.Text = "Layers to use the mask on";
            // 
            // groupBox_Margins
            // 
            this.groupBox_Margins.Controls.Add(this.doubleBox_TopMargin);
            this.groupBox_Margins.Controls.Add(this.doubleBox_BottomMargin);
            this.groupBox_Margins.Controls.Add(this.doubleBox_RightMargin);
            this.groupBox_Margins.Controls.Add(this.doubleBox_LeftMargin);
            this.groupBox_Margins.Controls.Add(this.label1);
            this.groupBox_Margins.Controls.Add(this.label2);
            this.groupBox_Margins.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_Margins.Location = new System.Drawing.Point(14, 34);
            this.groupBox_Margins.Name = "groupBox_Margins";
            this.groupBox_Margins.Size = new System.Drawing.Size(317, 127);
            this.groupBox_Margins.TabIndex = 4;
            this.groupBox_Margins.TabStop = false;
            this.groupBox_Margins.Text = "Margins";
            // 
            // doubleBox_TopMargin
            // 
            this.doubleBox_TopMargin.BackColorInvalid = System.Drawing.Color.Salmon;
            this.doubleBox_TopMargin.BackColorRegular = System.Drawing.Color.Empty;
            this.doubleBox_TopMargin.Caption = "Top";
            this.doubleBox_TopMargin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.doubleBox_TopMargin.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
    "ating point value.";
            this.doubleBox_TopMargin.IsValid = true;
            this.doubleBox_TopMargin.Location = new System.Drawing.Point(42, 19);
            this.doubleBox_TopMargin.Name = "doubleBox_TopMargin";
            this.doubleBox_TopMargin.NumberFormat = null;
            this.doubleBox_TopMargin.RegularHelp = "Enter a double precision floating point value.";
            this.doubleBox_TopMargin.Size = new System.Drawing.Size(140, 27);
            this.doubleBox_TopMargin.TabIndex = 7;
            this.doubleBox_TopMargin.Value = 0D;
            // 
            // doubleBox_BottomMargin
            // 
            this.doubleBox_BottomMargin.BackColorInvalid = System.Drawing.Color.Salmon;
            this.doubleBox_BottomMargin.BackColorRegular = System.Drawing.Color.Empty;
            this.doubleBox_BottomMargin.Caption = "Bottom";
            this.doubleBox_BottomMargin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.doubleBox_BottomMargin.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
    "ating point value.";
            this.doubleBox_BottomMargin.IsValid = true;
            this.doubleBox_BottomMargin.Location = new System.Drawing.Point(26, 90);
            this.doubleBox_BottomMargin.Name = "doubleBox_BottomMargin";
            this.doubleBox_BottomMargin.NumberFormat = null;
            this.doubleBox_BottomMargin.RegularHelp = "Enter a double precision floating point value.";
            this.doubleBox_BottomMargin.Size = new System.Drawing.Size(155, 27);
            this.doubleBox_BottomMargin.TabIndex = 7;
            this.doubleBox_BottomMargin.Value = 0D;
            // 
            // doubleBox_RightMargin
            // 
            this.doubleBox_RightMargin.BackColorInvalid = System.Drawing.Color.Salmon;
            this.doubleBox_RightMargin.BackColorRegular = System.Drawing.Color.Empty;
            this.doubleBox_RightMargin.Caption = "Right";
            this.doubleBox_RightMargin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.doubleBox_RightMargin.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
    "ating point value.";
            this.doubleBox_RightMargin.IsValid = true;
            this.doubleBox_RightMargin.Location = new System.Drawing.Point(34, 66);
            this.doubleBox_RightMargin.Name = "doubleBox_RightMargin";
            this.doubleBox_RightMargin.NumberFormat = null;
            this.doubleBox_RightMargin.RegularHelp = "Enter a double precision floating point value.";
            this.doubleBox_RightMargin.Size = new System.Drawing.Size(148, 27);
            this.doubleBox_RightMargin.TabIndex = 7;
            this.doubleBox_RightMargin.Value = 0D;
            // 
            // doubleBox_LeftMargin
            // 
            this.doubleBox_LeftMargin.BackColorInvalid = System.Drawing.Color.Salmon;
            this.doubleBox_LeftMargin.BackColorRegular = System.Drawing.Color.Empty;
            this.doubleBox_LeftMargin.Caption = "Left";
            this.doubleBox_LeftMargin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.doubleBox_LeftMargin.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
    "ating point value.";
            this.doubleBox_LeftMargin.IsValid = true;
            this.doubleBox_LeftMargin.Location = new System.Drawing.Point(42, 43);
            this.doubleBox_LeftMargin.Name = "doubleBox_LeftMargin";
            this.doubleBox_LeftMargin.NumberFormat = null;
            this.doubleBox_LeftMargin.RegularHelp = "Enter a double precision floating point value.";
            this.doubleBox_LeftMargin.Size = new System.Drawing.Size(140, 27);
            this.doubleBox_LeftMargin.TabIndex = 7;
            this.doubleBox_LeftMargin.Value = 0D;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(222, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 33);
            this.label1.TabIndex = 1;
            this.label1.Text = "TEXT";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.AliceBlue;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label2.Location = new System.Drawing.Point(200, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 64);
            this.label2.TabIndex = 2;
            this.label2.Text = "Mask";
            // 
            // MaskControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox_Margins);
            this.Controls.Add(this.groupBox_Layers);
            this.Controls.Add(this.cb_UseMask);
            this.Name = "MaskControl";
            this.Size = new System.Drawing.Size(568, 423);
            this.groupBox_Layers.ResumeLayout(false);
            this.groupBox_Margins.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cb_UseMask;
        private System.Windows.Forms.TreeView tv_layers;
        private System.Windows.Forms.GroupBox groupBox_Layers;
        private System.Windows.Forms.GroupBox groupBox_Margins;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Projections.Forms.DoubleBox doubleBox_TopMargin;
        private Projections.Forms.DoubleBox doubleBox_BottomMargin;
        private Projections.Forms.DoubleBox doubleBox_RightMargin;
        private Projections.Forms.DoubleBox doubleBox_LeftMargin;
    }
}
