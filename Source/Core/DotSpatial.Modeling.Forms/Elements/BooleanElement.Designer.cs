using System;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms.Elements
{
    public partial class BooleanElement
    {
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.checkBox1 = new CheckBox();
            this.GroupBox.SuspendLayout();
            this.SuspendLayout();

            // groupBox1
            this.GroupBox.Controls.Add(this.checkBox1);
            this.GroupBox.Text = "Caption";
            this.GroupBox.Controls.SetChildIndex(this.StatusLabel, 0);
            this.GroupBox.Controls.SetChildIndex(this.checkBox1, 0);

            // lblStatus
            this.StatusLabel.Location = new Point(12, 20);

            // checkBox1
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new Point(44, 19);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(80, 17);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckStateChanged += new EventHandler(this.CheckBox1CheckStateChanged);
            this.checkBox1.Click += new EventHandler(this.CheckBox1Click);

            // BooleanElement
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.Name = "BooleanElement";
            this.GroupBox.ResumeLayout(false);
            this.GroupBox.PerformLayout();
            this.ResumeLayout(false);
        }
        #endregion

        /// <summary>
        /// Disposes of the resources (other than memory) used by the <see cref="T:System.Windows.Forms.Form"/>.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private CheckBox checkBox1;
    }
}
