using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    public partial class LineSymbolDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LineSymbolDialog));
            this.lblSymbologyType = new System.Windows.Forms.Label();
            this.lblPredefinedSymbol = new System.Windows.Forms.Label();
            this.lblSymbolPreview = new System.Windows.Forms.Label();
            this.btnSymbolDetails = new System.Windows.Forms.Button();
            this.cmbCategories = new System.Windows.Forms.ComboBox();
            this.predefinedLineSymbolControl1 = new DotSpatial.Symbology.Forms.PredefinedLineSymbolControl();
            this.symbolPreview1 = new DotSpatial.Symbology.Forms.SymbolPreview();
            this.dialogButtons1 = new DotSpatial.Symbology.Forms.DialogButtons();
            this.SuspendLayout();
            //
            // lblSymbologyType
            //
            resources.ApplyResources(this.lblSymbologyType, "lblSymbologyType");
            this.lblSymbologyType.Name = "lblSymbologyType";
            //
            // lblPredefinedSymbol
            //
            resources.ApplyResources(this.lblPredefinedSymbol, "lblPredefinedSymbol");
            this.lblPredefinedSymbol.Name = "lblPredefinedSymbol";
            //
            // lblSymbolPreview
            //
            resources.ApplyResources(this.lblSymbolPreview, "lblSymbolPreview");
            this.lblSymbolPreview.Name = "lblSymbolPreview";
            //
            // btnSymbolDetails
            //
            resources.ApplyResources(this.btnSymbolDetails, "btnSymbolDetails");
            this.btnSymbolDetails.Name = "btnSymbolDetails";
            this.btnSymbolDetails.UseVisualStyleBackColor = true;
            this.btnSymbolDetails.Click += new System.EventHandler(this.BtnSymbolDetailsClick);
            //
            // cmbCategories
            //
            resources.ApplyResources(this.cmbCategories, "cmbCategories");
            this.cmbCategories.FormattingEnabled = true;
            this.cmbCategories.Name = "cmbCategories";
            this.cmbCategories.SelectedIndexChanged += new System.EventHandler(this.CmbCategoriesSelectedIndexChanged);
            //
            // predefinedLineSymbolControl1
            //
            resources.ApplyResources(this.predefinedLineSymbolControl1, "predefinedLineSymbolControl1");
            this.predefinedLineSymbolControl1.BackColor = System.Drawing.Color.White;
            this.predefinedLineSymbolControl1.CategoryFilter = String.Empty;
            this.predefinedLineSymbolControl1.CellMargin = 8;
            this.predefinedLineSymbolControl1.CellSize = new System.Drawing.Size(62, 62);
            this.predefinedLineSymbolControl1.ControlRectangle = new System.Drawing.Rectangle(0, 0, 272, 253);
            this.predefinedLineSymbolControl1.DefaultCategoryFilter = "All";
            this.predefinedLineSymbolControl1.DynamicColumns = true;
            this.predefinedLineSymbolControl1.IsInitialized = false;
            this.predefinedLineSymbolControl1.IsSelected = true;
            this.predefinedLineSymbolControl1.Name = "predefinedLineSymbolControl1";
            this.predefinedLineSymbolControl1.SelectedIndex = -1;
            this.predefinedLineSymbolControl1.SelectionBackColor = System.Drawing.Color.LightGray;
            this.predefinedLineSymbolControl1.SelectionForeColor = System.Drawing.Color.White;
            this.predefinedLineSymbolControl1.ShowSymbolNames = true;
            this.predefinedLineSymbolControl1.TextFont = new System.Drawing.Font("Arial", 8F);
            this.predefinedLineSymbolControl1.VerticalScrollEnabled = true;
            //
            // symbolPreview1
            //
            resources.ApplyResources(this.symbolPreview1, "symbolPreview1");
            this.symbolPreview1.BackColor = System.Drawing.Color.White;
            this.symbolPreview1.Name = "symbolPreview1";
            //
            // dialogButtons1
            //
            resources.ApplyResources(this.dialogButtons1, "dialogButtons1");
            this.dialogButtons1.Name = "dialogButtons1";
            //
            // LineSymbolDialog
            //
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.dialogButtons1);
            this.Controls.Add(this.predefinedLineSymbolControl1);
            this.Controls.Add(this.cmbCategories);
            this.Controls.Add(this.symbolPreview1);
            this.Controls.Add(this.btnSymbolDetails);
            this.Controls.Add(this.lblSymbolPreview);
            this.Controls.Add(this.lblPredefinedSymbol);
            this.Controls.Add(this.lblSymbologyType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LineSymbolDialog";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Button btnSymbolDetails;
        private ComboBox cmbCategories;
        private DialogButtons dialogButtons1;
        private Label lblPredefinedSymbol;
        private Label lblSymbolPreview;
        private Label lblSymbologyType;
        private PredefinedLineSymbolControl predefinedLineSymbolControl1;
        private SymbolPreview symbolPreview1;
    }
}