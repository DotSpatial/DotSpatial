using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    public partial class OutlineControl
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(OutlineControl));
            this.grpOutline = new GroupBox();
            this.btnEditOutline = new Button();
            this.cbOutlineColor = new ColorButton();
            this.sldOutlineOpacity = new RampSlider();
            this.label2 = new Label();
            this.dbxOutlineWidth = new DoubleBox();
            this.chkUseOutline = new CheckBox();
            this.grpOutline.SuspendLayout();
            this.SuspendLayout();
            //
            // grpOutline
            //
            this.grpOutline.AccessibleDescription = null;
            this.grpOutline.AccessibleName = null;
            resources.ApplyResources(this.grpOutline, "grpOutline");
            this.grpOutline.BackgroundImage = null;
            this.grpOutline.Controls.Add(this.btnEditOutline);
            this.grpOutline.Controls.Add(this.cbOutlineColor);
            this.grpOutline.Controls.Add(this.sldOutlineOpacity);
            this.grpOutline.Controls.Add(this.label2);
            this.grpOutline.Controls.Add(this.dbxOutlineWidth);
            this.grpOutline.Controls.Add(this.chkUseOutline);
            this.grpOutline.Font = null;
            this.grpOutline.Name = "grpOutline";
            this.grpOutline.TabStop = false;
            //
            // btnEditOutline
            //
            this.btnEditOutline.AccessibleDescription = null;
            this.btnEditOutline.AccessibleName = null;
            resources.ApplyResources(this.btnEditOutline, "btnEditOutline");
            this.btnEditOutline.BackgroundImage = null;
            this.btnEditOutline.Font = null;
            this.btnEditOutline.Name = "btnEditOutline";
            this.btnEditOutline.UseVisualStyleBackColor = true;
            this.btnEditOutline.Click += new EventHandler(this.BtnEditOutlineClick);
            //
            // cbOutlineColor
            //
            this.cbOutlineColor.AccessibleDescription = null;
            this.cbOutlineColor.AccessibleName = null;
            resources.ApplyResources(this.cbOutlineColor, "cbOutlineColor");
            this.cbOutlineColor.BackgroundImage = null;
            this.cbOutlineColor.BevelRadius = 4;
            this.cbOutlineColor.Color = Color.Blue;
            this.cbOutlineColor.Font = null;
            this.cbOutlineColor.LaunchDialogOnClick = true;
            this.cbOutlineColor.Name = "cbOutlineColor";
            this.cbOutlineColor.RoundingRadius = 10;
            this.cbOutlineColor.ColorChanged += new EventHandler(this.CbOutlineColorColorChanged);
            //
            // sldOutlineOpacity
            //
            this.sldOutlineOpacity.AccessibleDescription = null;
            this.sldOutlineOpacity.AccessibleName = null;
            resources.ApplyResources(this.sldOutlineOpacity, "sldOutlineOpacity");
            this.sldOutlineOpacity.BackgroundImage = null;
            this.sldOutlineOpacity.ColorButton = null;
            this.sldOutlineOpacity.FlipRamp = false;
            this.sldOutlineOpacity.FlipText = false;
            this.sldOutlineOpacity.Font = null;
            this.sldOutlineOpacity.InvertRamp = false;
            this.sldOutlineOpacity.Maximum = 1;
            this.sldOutlineOpacity.MaximumColor = Color.CornflowerBlue;
            this.sldOutlineOpacity.Minimum = 0;
            this.sldOutlineOpacity.MinimumColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.sldOutlineOpacity.Name = "sldOutlineOpacity";
            this.sldOutlineOpacity.NumberFormat = null;
            this.sldOutlineOpacity.Orientation = Orientation.Horizontal;
            this.sldOutlineOpacity.RampRadius = 8F;
            this.sldOutlineOpacity.RampText = "Opacity";
            this.sldOutlineOpacity.RampTextAlignment = ContentAlignment.BottomCenter;
            this.sldOutlineOpacity.RampTextBehindRamp = true;
            this.sldOutlineOpacity.RampTextColor = Color.Black;
            this.sldOutlineOpacity.RampTextFont = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.sldOutlineOpacity.ShowMaximum = true;
            this.sldOutlineOpacity.ShowMinimum = true;
            this.sldOutlineOpacity.ShowTicks = true;
            this.sldOutlineOpacity.ShowValue = false;
            this.sldOutlineOpacity.SliderColor = Color.SteelBlue;
            this.sldOutlineOpacity.SliderRadius = 4F;
            this.sldOutlineOpacity.TickColor = Color.DarkGray;
            this.sldOutlineOpacity.TickSpacing = 5F;
            this.sldOutlineOpacity.Value = 0;
            this.sldOutlineOpacity.ValueChanged += new EventHandler(this.SldOutlineOpacityValueChanged);
            //
            // label2
            //
            this.label2.AccessibleDescription = null;
            this.label2.AccessibleName = null;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Font = null;
            this.label2.Name = "label2";
            //
            // dbxOutlineWidth
            //
            this.dbxOutlineWidth.AccessibleDescription = null;
            this.dbxOutlineWidth.AccessibleName = null;
            resources.ApplyResources(this.dbxOutlineWidth, "dbxOutlineWidth");
            this.dbxOutlineWidth.BackColorInvalid = Color.Salmon;
            this.dbxOutlineWidth.BackColorRegular = Color.Empty;
            this.dbxOutlineWidth.BackgroundImage = null;
            this.dbxOutlineWidth.Caption = "Width:";
            this.dbxOutlineWidth.Font = null;
            this.dbxOutlineWidth.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
                                               "ating point value.";
            this.dbxOutlineWidth.IsValid = true;
            this.dbxOutlineWidth.Name = "dbxOutlineWidth";
            this.dbxOutlineWidth.NumberFormat = null;
            this.dbxOutlineWidth.RegularHelp = "Enter a double precision floating point value.";
            this.dbxOutlineWidth.Value = 0;
            this.dbxOutlineWidth.TextChanged += new EventHandler(this.DbxOutlineWidthTextChanged);
            //
            // chkUseOutline
            //
            this.chkUseOutline.AccessibleDescription = null;
            this.chkUseOutline.AccessibleName = null;
            resources.ApplyResources(this.chkUseOutline, "chkUseOutline");
            this.chkUseOutline.BackgroundImage = null;
            this.chkUseOutline.Font = null;
            this.chkUseOutline.Name = "chkUseOutline";
            this.chkUseOutline.UseVisualStyleBackColor = true;
            this.chkUseOutline.CheckedChanged += new EventHandler(this.ChkUseOutlineCheckedChanged);
            //
            // OutlineControl
            //
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");

            this.BackgroundImage = null;
            this.Controls.Add(this.grpOutline);
            this.Font = null;
            this.Name = "OutlineControl";
            this.grpOutline.ResumeLayout(false);
            this.grpOutline.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private Button btnEditOutline;
        private ColorButton cbOutlineColor;
        private CheckBox chkUseOutline;
        private DoubleBox dbxOutlineWidth;
        private GroupBox grpOutline;
        private Label label2;
        private RampSlider sldOutlineOpacity;
    }
}