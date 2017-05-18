using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    public partial class AngleControl
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(AngleControl));
            this._lblText = new Label();
            this._nudAngle = new NumericUpDown();
            this._anglePicker1 = new AnglePicker();
            ((ISupportInitialize)(this._nudAngle)).BeginInit();
            this.SuspendLayout();
            //
            // lblText
            //
            this._lblText.AccessibleDescription = null;
            this._lblText.AccessibleName = null;
            resources.ApplyResources(this._lblText, "_lblText");
            this._lblText.Font = null;
            this._lblText.Name = "_lblText";
            //
            // nudAngle
            //
            this._nudAngle.AccessibleDescription = null;
            this._nudAngle.AccessibleName = null;
            resources.ApplyResources(this._nudAngle, "_nudAngle");
            this._nudAngle.Maximum = new decimal(new int[] { 360, 0, 0, 0 });
            this._nudAngle.Minimum = new decimal(new int[] { -360, 0, 0, -2147483648 });
            this._nudAngle.Name = "_nudAngle";
            this._nudAngle.ValueChanged += new EventHandler(this.NudAngleValueChanged);
            //
            // anglePicker1
            //
            this._anglePicker1.AccessibleDescription = null;
            this._anglePicker1.AccessibleName = null;
            resources.ApplyResources(this._anglePicker1, "_anglePicker1");
            this._anglePicker1.Angle = 0;
            this._anglePicker1.BackgroundImage = null;
            // this._anglePicker1.BorderStyle = BorderStyle.None;
            this._anglePicker1.CircleBorderColor = Color.LightGray;
            // this._anglePicker1.CircleBorderStyle = BorderStyle.Fixed3D;
            this._anglePicker1.CircleFillColor = Color.LightGray;
            this._anglePicker1.Clockwise = false;
            this._anglePicker1.Font = null;
            this._anglePicker1.KnobColor = Color.Green;
            this._anglePicker1.KnobVisible = true;
            this._anglePicker1.Name = "_anglePicker1";
            this._anglePicker1.PieFillColor = Color.SteelBlue;
            this._anglePicker1.Snap = 3;
            this._anglePicker1.StartAngle = 0;
            this._anglePicker1.TextAlignment = ContentAlignment.BottomCenter;
            this._anglePicker1.AngleChanged += new EventHandler(this.AnglePicker1AngleChanged);
            //
            // AngleControl
            //
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");

            this.BackColor = SystemColors.Control;
            this.BackgroundImage = null;
            this.Controls.Add(this._nudAngle);
            this.Controls.Add(this._anglePicker1);
            this.Controls.Add(this._lblText);
            this.Font = null;
            this.Name = "AngleControl";
            ((ISupportInitialize)(this._nudAngle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private AnglePicker _anglePicker1;
        private Label _lblText;
        private NumericUpDown _nudAngle;

    }
}