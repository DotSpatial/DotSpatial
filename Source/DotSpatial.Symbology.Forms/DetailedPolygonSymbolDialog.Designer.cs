using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    public partial class DetailedPolygonSymbolDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DetailedPolygonSymbolDialog));
            this._panel1 = new System.Windows.Forms.Panel();
            this._dialogButtons1 = new DotSpatial.Symbology.Forms.DialogButtons();
            this._detailedPolygonSymbolControl1 = new DotSpatial.Symbology.Forms.DetailedPolygonSymbolControl();
            this._panel1.SuspendLayout();
            this.SuspendLayout();
            //
            // _panel1
            //
            this._panel1.Controls.Add(this._dialogButtons1);
            resources.ApplyResources(this._panel1, "_panel1");
            this._panel1.Name = "_panel1";
            //
            // _dialogButtons1
            //
            resources.ApplyResources(this._dialogButtons1, "_dialogButtons1");
            this._dialogButtons1.Name = "_dialogButtons1";
            //
            // _detailedPolygonSymbolControl1
            //
            resources.ApplyResources(this._detailedPolygonSymbolControl1, "_detailedPolygonSymbolControl1");
            this._detailedPolygonSymbolControl1.Name = "_detailedPolygonSymbolControl1";
            //
            // DetailedPolygonSymbolDialog
            //
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this._detailedPolygonSymbolControl1);
            this.Controls.Add(this._panel1);
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DetailedPolygonSymbolDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this._panel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private DetailedPolygonSymbolControl _detailedPolygonSymbolControl1;
        private DialogButtons _dialogButtons1;
        private Panel _panel1;
    }
}