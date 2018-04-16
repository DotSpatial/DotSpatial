namespace DotSpatial.Symbology.Forms
{
    partial class ExpressionDialog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExpressionDialog));
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.listViewFields = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TB_Simple = new System.Windows.Forms.RichTextBox();
            this.B_Preview = new System.Windows.Forms.Button();
            this.TabControl = new System.Windows.Forms.TabControl();
            this.TabSimple = new System.Windows.Forms.TabPage();
            this.TabAdvanced = new System.Windows.Forms.TabPage();
            this.B_Export = new System.Windows.Forms.Button();
            this.B_Import = new System.Windows.Forms.Button();
            this.TB_Advanced = new System.Windows.Forms.RichTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.richTextBoxViewer = new System.Windows.Forms.RichTextBox();
            this.PanelPreview = new System.Windows.Forms.Panel();
            this.TT_Help = new System.Windows.Forms.ToolTip(this.components);
            this.B_Help = new System.Windows.Forms.Button();
            this.TabControl.SuspendLayout();
            this.TabSimple.SuspendLayout();
            this.TabAdvanced.SuspendLayout();
            this.PanelPreview.SuspendLayout();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.BackColor = System.Drawing.Color.DarkGray;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(258, 6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(456, 22);
            this.label6.TabIndex = 22;
            this.label6.Text = "Expression";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.DarkGray;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(246, 22);
            this.label1.TabIndex = 23;
            this.label1.Text = "Fields";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // listViewFields
            // 
            this.listViewFields.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listViewFields.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listViewFields.GridLines = true;
            this.listViewFields.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewFields.Location = new System.Drawing.Point(6, 29);
            this.listViewFields.Name = "listViewFields";
            this.listViewFields.Size = new System.Drawing.Size(246, 376);
            this.listViewFields.TabIndex = 16;
            this.listViewFields.UseCompatibleStateImageBehavior = false;
            this.listViewFields.View = System.Windows.Forms.View.Details;
            this.listViewFields.DoubleClick += new System.EventHandler(this.listViewFields_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 229;
            // 
            // TB_Simple
            // 
            this.TB_Simple.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TB_Simple.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_Simple.Location = new System.Drawing.Point(4, 4);
            this.TB_Simple.Name = "TB_Simple";
            this.TB_Simple.Size = new System.Drawing.Size(468, 494);
            this.TB_Simple.TabIndex = 1;
            this.TB_Simple.Text = "";
            this.TB_Simple.TextChanged += new System.EventHandler(this.TB_Simple_TextChanged);
            this.TB_Simple.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.TB_Simple_PreviewKeyDown);
            // 
            // B_Preview
            // 
            this.B_Preview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.B_Preview.Image = global::DotSpatial.Symbology.Forms.Properties.Resources.zoom_16x16;
            this.B_Preview.Location = new System.Drawing.Point(6, 411);
            this.B_Preview.Name = "B_Preview";
            this.B_Preview.Size = new System.Drawing.Size(82, 24);
            this.B_Preview.TabIndex = 20;
            this.B_Preview.Text = "Preview";
            this.B_Preview.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.B_Preview.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.B_Preview.UseVisualStyleBackColor = true;
            this.B_Preview.Click += new System.EventHandler(this.B_Preview_Click);
            // 
            // TabControl
            // 
            this.TabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TabControl.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.TabControl.Controls.Add(this.TabSimple);
            this.TabControl.Controls.Add(this.TabAdvanced);
            this.TabControl.ItemSize = new System.Drawing.Size(212, 18);
            this.TabControl.Location = new System.Drawing.Point(258, 31);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(485, 529);
            this.TabControl.TabIndex = 28;
            // 
            // TabSimple
            // 
            this.TabSimple.Controls.Add(this.TB_Simple);
            this.TabSimple.Location = new System.Drawing.Point(4, 22);
            this.TabSimple.Name = "TabSimple";
            this.TabSimple.Padding = new System.Windows.Forms.Padding(3);
            this.TabSimple.Size = new System.Drawing.Size(477, 503);
            this.TabSimple.TabIndex = 0;
            this.TabSimple.Text = "Simple";
            this.TabSimple.UseVisualStyleBackColor = true;
            // 
            // TabAdvanced
            // 
            this.TabAdvanced.BackColor = System.Drawing.Color.LightGray;
            this.TabAdvanced.Controls.Add(this.B_Export);
            this.TabAdvanced.Controls.Add(this.B_Import);
            this.TabAdvanced.Controls.Add(this.TB_Advanced);
            this.TabAdvanced.Location = new System.Drawing.Point(4, 22);
            this.TabAdvanced.Name = "TabAdvanced";
            this.TabAdvanced.Padding = new System.Windows.Forms.Padding(3);
            this.TabAdvanced.Size = new System.Drawing.Size(477, 503);
            this.TabAdvanced.TabIndex = 1;
            this.TabAdvanced.Text = "Advanced ";
            // 
            // B_Export
            // 
            this.B_Export.Image = global::DotSpatial.Symbology.Forms.Properties.Resources.B_Export16;
            this.B_Export.Location = new System.Drawing.Point(1, 26);
            this.B_Export.Name = "B_Export";
            this.B_Export.Size = new System.Drawing.Size(28, 25);
            this.B_Export.TabIndex = 9;
            this.B_Export.UseVisualStyleBackColor = true;
            this.B_Export.Click += new System.EventHandler(this.B_Export_Click);
            // 
            // B_Import
            // 
            this.B_Import.Image = global::DotSpatial.Symbology.Forms.Properties.Resources.B_Import16;
            this.B_Import.Location = new System.Drawing.Point(1, 1);
            this.B_Import.Name = "B_Import";
            this.B_Import.Size = new System.Drawing.Size(28, 25);
            this.B_Import.TabIndex = 9;
            this.B_Import.UseVisualStyleBackColor = true;
            this.B_Import.Click += new System.EventHandler(this.B_Import_Click);
            // 
            // TB_Advanced
            // 
            this.TB_Advanced.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TB_Advanced.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TB_Advanced.Location = new System.Drawing.Point(29, 3);
            this.TB_Advanced.Name = "TB_Advanced";
            this.TB_Advanced.Size = new System.Drawing.Size(442, 494);
            this.TB_Advanced.TabIndex = 2;
            this.TB_Advanced.Text = "def Main():\n   \n \n  return \"no value\"";
            this.TB_Advanced.TextChanged += new System.EventHandler(this.TB_Advanced_TextChanged);
            this.TB_Advanced.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.TB_Advanced_PreviewKeyDown);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.BackColor = System.Drawing.Color.DarkGray;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(90, 412);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(162, 22);
            this.label5.TabIndex = 27;
            this.label5.Text = "Preview";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // richTextBoxViewer
            // 
            this.richTextBoxViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.richTextBoxViewer.Location = new System.Drawing.Point(3, 1);
            this.richTextBoxViewer.Name = "richTextBoxViewer";
            this.richTextBoxViewer.Size = new System.Drawing.Size(193, 79);
            this.richTextBoxViewer.TabIndex = 19;
            this.richTextBoxViewer.Text = "";
            this.richTextBoxViewer.Visible = false;
            this.richTextBoxViewer.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.richTextBoxViewer_PreviewKeyDown);
            // 
            // PanelPreview
            // 
            this.PanelPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.PanelPreview.BackColor = System.Drawing.Color.White;
            this.PanelPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PanelPreview.Controls.Add(this.richTextBoxViewer);
            this.PanelPreview.Location = new System.Drawing.Point(6, 437);
            this.PanelPreview.Name = "PanelPreview";
            this.PanelPreview.Size = new System.Drawing.Size(246, 123);
            this.PanelPreview.TabIndex = 29;
            this.PanelPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelPreview_Paint);
            // 
            // TT_Help
            // 
            this.TT_Help.AutoPopDelay = 50000;
            this.TT_Help.InitialDelay = 500;
            this.TT_Help.ReshowDelay = 100;
            this.TT_Help.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.TT_Help.ToolTipTitle = "Help";
            // 
            // B_Help
            // 
            this.B_Help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Help.Image = global::DotSpatial.Symbology.Forms.Properties.Resources.help_16x16;
            this.B_Help.Location = new System.Drawing.Point(717, 5);
            this.B_Help.Name = "B_Help";
            this.B_Help.Size = new System.Drawing.Size(26, 24);
            this.B_Help.TabIndex = 2;
            this.TT_Help.SetToolTip(this.B_Help, resources.GetString("B_Help.ToolTip"));
            this.B_Help.UseVisualStyleBackColor = true;
            // 
            // ExpressionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.B_Help);
            this.Controls.Add(this.PanelPreview);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listViewFields);
            this.Controls.Add(this.B_Preview);
            this.Controls.Add(this.TabControl);
            this.Controls.Add(this.label5);
            this.Name = "ExpressionDialog";
            this.Size = new System.Drawing.Size(751, 572);
            this.TabControl.ResumeLayout(false);
            this.TabSimple.ResumeLayout(false);
            this.TabAdvanced.ResumeLayout(false);
            this.PanelPreview.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView listViewFields;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.RichTextBox TB_Simple;
        private System.Windows.Forms.Button B_Preview;
        private System.Windows.Forms.TabControl TabControl;
        private System.Windows.Forms.TabPage TabSimple;
        private System.Windows.Forms.TabPage TabAdvanced;
        private System.Windows.Forms.Button B_Export;
        private System.Windows.Forms.Button B_Import;
        private System.Windows.Forms.RichTextBox TB_Advanced;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RichTextBox richTextBoxViewer;
        private System.Windows.Forms.Panel PanelPreview;
        private System.Windows.Forms.ToolTip TT_Help;
        private System.Windows.Forms.Button B_Help;
    }
}
