namespace DotSpatial.Symbology.Forms
{
    partial class ImageCategoryControl
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.rsOpacity = new DotSpatial.Symbology.Forms.RampSlider();
            this.SuspendLayout();
            // 
            // rsOpacity
            // 
            this.rsOpacity.ColorButton = null;
            this.rsOpacity.FlipRamp = false;
            this.rsOpacity.FlipText = false;
            this.rsOpacity.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.rsOpacity.InvertRamp = false;
            this.rsOpacity.Location = new System.Drawing.Point(12, 3);
            this.rsOpacity.Maximum = 1D;
            this.rsOpacity.MaximumColor = System.Drawing.Color.Green;
            this.rsOpacity.Minimum = 0D;
            this.rsOpacity.MinimumColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.rsOpacity.Name = "rsOpacity";
            this.rsOpacity.NumberFormat = "#.00";
            this.rsOpacity.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.rsOpacity.RampRadius = 9F;
            this.rsOpacity.RampText = "Opacity";
            this.rsOpacity.RampTextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rsOpacity.RampTextBehindRamp = true;
            this.rsOpacity.RampTextColor = System.Drawing.Color.Black;
            this.rsOpacity.RampTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rsOpacity.ShowMaximum = false;
            this.rsOpacity.ShowMinimum = false;
            this.rsOpacity.ShowTicks = false;
            this.rsOpacity.ShowValue = false;
            this.rsOpacity.Size = new System.Drawing.Size(125, 22);
            this.rsOpacity.SliderColor = System.Drawing.Color.Blue;
            this.rsOpacity.SliderRadius = 4F;
            this.rsOpacity.TabIndex = 1;
            this.rsOpacity.Text = "Opacity";
            this.rsOpacity.TickColor = System.Drawing.Color.DarkGray;
            this.rsOpacity.TickSpacing = 5F;
            this.rsOpacity.Value = 1D;
            this.rsOpacity.ValueChanged += new System.EventHandler(this.RsOpacityValueChanged);
            // 
            // ImageCategoryControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rsOpacity);
            this.Name = "ImageCategoryControl";
            this.ResumeLayout(false);

        }

        #endregion

        private RampSlider rsOpacity;

    }
}
