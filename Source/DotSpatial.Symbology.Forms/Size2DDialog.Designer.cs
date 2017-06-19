
namespace DotSpatial.Symbology.Forms
{
    public partial class Size2DDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Size2DDialog));
            this.dialogButtons1 = new DotSpatial.Symbology.Forms.DialogButtons();
            this.dbxHeight = new DotSpatial.Symbology.Forms.DoubleBox();
            this.dbxWidth = new DotSpatial.Symbology.Forms.DoubleBox();
            this.SuspendLayout();
            // 
            // dialogButtons1
            // 
            resources.ApplyResources(this.dialogButtons1, "dialogButtons1");
            this.dialogButtons1.Name = "dialogButtons1";
            // 
            // dbxHeight
            // 
            resources.ApplyResources(this.dbxHeight, "dbxHeight");
            this.dbxHeight.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbxHeight.BackColorRegular = System.Drawing.Color.Empty;
            this.dbxHeight.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision floating point value.";
            this.dbxHeight.IsValid = true;
            this.dbxHeight.Name = "dbxHeight";
            this.dbxHeight.NumberFormat = null;
            this.dbxHeight.RegularHelp = "Enter a double precision floating point value.";
            this.dbxHeight.Value = 0D;
            this.dbxHeight.TextChanged += new System.EventHandler(this.DbxHeightTextChanged);
            // 
            // dbxWidth
            // 
            resources.ApplyResources(this.dbxWidth, "dbxWidth");
            this.dbxWidth.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbxWidth.BackColorRegular = System.Drawing.Color.Empty;
            this.dbxWidth.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision floating point value.";
            this.dbxWidth.IsValid = true;
            this.dbxWidth.Name = "dbxWidth";
            this.dbxWidth.NumberFormat = null;
            this.dbxWidth.RegularHelp = "Enter a double precision floating point value.";
            this.dbxWidth.Value = 0D;
            this.dbxWidth.TextChanged += new System.EventHandler(this.DbxWidthTextChanged);
            // 
            // Size2DDialog
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.dialogButtons1);
            this.Controls.Add(this.dbxHeight);
            this.Controls.Add(this.dbxWidth);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Size2DDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
        }

        #endregion

        private DoubleBox dbxHeight;
        private DoubleBox dbxWidth;
        private DialogButtons dialogButtons1;
    }
}