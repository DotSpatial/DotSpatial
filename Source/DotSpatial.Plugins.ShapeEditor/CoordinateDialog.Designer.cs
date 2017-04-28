using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DotSpatial.Symbology.Forms;

namespace DotSpatial.Plugins.ShapeEditor
{
    public partial class CoordinateDialog
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
            this.components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(CoordinateDialog));
            this._btnOk = new Button();
            this._btnClose = new Button();
            this._ttHelp = new ToolTip(this.components);
            this._dbxM = new DoubleBox();
            this._dbxZ = new DoubleBox();
            this._dbxY = new DoubleBox();
            this._dbxX = new DoubleBox();
            this.SuspendLayout();

            // _btnOk
            resources.ApplyResources(this._btnOk, "_btnOk");
            this._btnOk.DialogResult = DialogResult.OK;
            this._btnOk.Name = "_btnOk";
            this._ttHelp.SetToolTip(this._btnOk, resources.GetString("_btnOk.ToolTip"));
            this._btnOk.UseVisualStyleBackColor = true;
            this._btnOk.Click += new EventHandler(this.OkButtonClick);

            // _btnClose
            resources.ApplyResources(this._btnClose, "_btnClose");
            this._btnClose.Name = "_btnClose";
            this._ttHelp.SetToolTip(this._btnClose, resources.GetString("_btnClose.ToolTip"));
            this._btnClose.UseVisualStyleBackColor = true;
            this._btnClose.Click += new EventHandler(this.CloseButtonClick);

            // _dbxM
            resources.ApplyResources(this._dbxM, "_dbxM");
            this._dbxM.BackColorInvalid = System.Drawing.Color.Salmon;
            this._dbxM.BackColorRegular = System.Drawing.Color.Empty;
            this._dbxM.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" + "ating point value.";
            this._dbxM.IsValid = true;
            this._dbxM.Name = "_dbxM";
            this._dbxM.NumberFormat = null;
            this._dbxM.RegularHelp = "Enter a double precision floating point value.";
            this._dbxM.Value = 0D;
            this._dbxM.ValidChanged += new EventHandler(this.CoordinateValidChanged);

            // _dbxZ
            resources.ApplyResources(this._dbxZ, "_dbxZ");
            this._dbxZ.BackColorInvalid = System.Drawing.Color.Salmon;
            this._dbxZ.BackColorRegular = System.Drawing.Color.Empty;
            this._dbxZ.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" + "ating point value.";
            this._dbxZ.IsValid = true;
            this._dbxZ.Name = "_dbxZ";
            this._dbxZ.NumberFormat = null;
            this._dbxZ.RegularHelp = "Enter a double precision floating point value.";
            this._dbxZ.Value = 0D;

            // _dbxY
            resources.ApplyResources(this._dbxY, "_dbxY");
            this._dbxY.BackColorInvalid = System.Drawing.Color.Salmon;
            this._dbxY.BackColorRegular = System.Drawing.Color.Empty;
            this._dbxY.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" + "ating point value.";
            this._dbxY.IsValid = true;
            this._dbxY.Name = "_dbxY";
            this._dbxY.NumberFormat = null;
            this._dbxY.RegularHelp = "Enter a double precision floating point value.";
            this._dbxY.Value = 0D;
            this._dbxY.ValidChanged += new EventHandler(this.CoordinateValidChanged);

            // _dbxX
            resources.ApplyResources(this._dbxX, "_dbxX");
            this._dbxX.BackColorInvalid = System.Drawing.Color.Salmon;
            this._dbxX.BackColorRegular = System.Drawing.Color.Empty;
            this._dbxX.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" + "ating point value.";
            this._dbxX.IsValid = true;
            this._dbxX.Name = "_dbxX";
            this._dbxX.NumberFormat = null;
            this._dbxX.RegularHelp = "Enter a double precision floating point value.";
            this._dbxX.Value = 0D;
            this._dbxX.ValidChanged += new EventHandler(this.CoordinateValidChanged);

            // CoordinateDialog
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this._dbxM);
            this.Controls.Add(this._dbxZ);
            this.Controls.Add(this._dbxY);
            this.Controls.Add(this._dbxX);
            this.Controls.Add(this._btnClose);
            this.Controls.Add(this._btnOk);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CoordinateDialog";
            this.ShowIcon = false;
            this.TopMost = true;
            this.ResumeLayout(false);
        }

        #endregion

        private Button _btnClose;
        private Button _btnOk;
        private DoubleBox _dbxM;
        private DoubleBox _dbxX;
        private DoubleBox _dbxY;
        private DoubleBox _dbxZ;
    }
}