
namespace DotSpatial.Symbology.Forms
{
    public partial class GradientControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GradientControl));
            this._gradientSlider1 = new DotSpatial.Symbology.Forms.GradientSlider();
            this._leverMinimum = new DotSpatial.Symbology.Forms.ColorLever();
            this._leverMaximum = new DotSpatial.Symbology.Forms.ColorLever();
            this.SuspendLayout();
            //
            // _gradientSlider1
            //
            this._gradientSlider1.LeftHandle.Color = System.Drawing.Color.SteelBlue;
            this._gradientSlider1.LeftHandle.Position = 0.2F;
            this._gradientSlider1.LeftHandle.RoundingRadius = 4;
            this._gradientSlider1.LeftHandle.Visible = true;
            this._gradientSlider1.LeftHandle.Width = 10;
            resources.ApplyResources(this._gradientSlider1, "_gradientSlider1");
            this._gradientSlider1.Maximum = 1F;
            this._gradientSlider1.MaximumColor = System.Drawing.Color.Blue;
            this._gradientSlider1.Minimum = 0F;
            this._gradientSlider1.MinimumColor = System.Drawing.Color.Lime;
            this._gradientSlider1.Name = "_gradientSlider1";
            this._gradientSlider1.RightHandle.Color = System.Drawing.Color.SteelBlue;
            this._gradientSlider1.RightHandle.Position = 0.8F;
            this._gradientSlider1.RightHandle.RoundingRadius = 4;
            this._gradientSlider1.RightHandle.Visible = true;
            this._gradientSlider1.RightHandle.Width = 10;
            this._gradientSlider1.PositionChanging += new System.EventHandler(this.GradientSlider1PositionChanging);
            this._gradientSlider1.PositionChanged += new System.EventHandler(this.GradientSlider1PositionChanged);
            //
            // _leverMinimum
            //
            this._leverMinimum.Angle = 0D;
            this._leverMinimum.BackColor = System.Drawing.SystemColors.Control;
            this._leverMinimum.BarLength = 5;
            this._leverMinimum.BarWidth = 5;
            this._leverMinimum.BorderWidth = 5;
            this._leverMinimum.Color = System.Drawing.Color.FromArgb(0, 0, 255, 0);
            this._leverMinimum.Flip = true;
            this._leverMinimum.KnobColor = System.Drawing.Color.SteelBlue;
            this._leverMinimum.KnobRadius = 7;
            resources.ApplyResources(this._leverMinimum, "_leverMinimum");
            this._leverMinimum.Name = "_leverMinimum";
            this._leverMinimum.Opacity = 0F;
            this._leverMinimum.ColorChanged += new System.EventHandler(this.LeverMinimumColorChanged);
            this._leverMinimum.ColorChanging += new System.EventHandler(this.LeverMinimumColorChanging);
            //
            // _leverMaximum
            //
            this._leverMaximum.Angle = 0D;
            this._leverMaximum.BackColor = System.Drawing.SystemColors.Control;
            this._leverMaximum.BarLength = 5;
            this._leverMaximum.BarWidth = 5;
            this._leverMaximum.BorderWidth = 5;
            this._leverMaximum.Color = System.Drawing.Color.FromArgb(0, 0, 255);
            this._leverMaximum.Flip = false;
            this._leverMaximum.KnobColor = System.Drawing.Color.SteelBlue;
            this._leverMaximum.KnobRadius = 7;
            resources.ApplyResources(this._leverMaximum, "_leverMaximum");
            this._leverMaximum.Name = "_leverMaximum";
            this._leverMaximum.Opacity = 1F;
            this._leverMaximum.ColorChanged += new System.EventHandler(this.LeverMaximumColorChanged);
            this._leverMaximum.ColorChanging += new System.EventHandler(this.LeverMaximumColorChanging);
            //
            // GradientControl
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this._gradientSlider1);
            this.Controls.Add(this._leverMinimum);
            this.Controls.Add(this._leverMaximum);
            this.Name = "GradientControl";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);
        }

        #endregion
    }
}