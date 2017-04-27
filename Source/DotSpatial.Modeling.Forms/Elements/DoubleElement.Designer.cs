using System;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms.Elements
{
    public partial class DoubleElement
    {
        #region Windows Form Designer generated code
        private void InitializeializeializeComponent()
        {
            this.textBox1 = new TextBox();
            this.GroupBox.SuspendLayout();
            this.SuspendLayout();

            // groupBox1
            this.GroupBox.Controls.Add(this.textBox1);
            this.GroupBox.Size = new Size(492, 46);
            this.GroupBox.Text = "Caption";
            this.GroupBox.Controls.SetChildIndex(this.StatusLabel, 0);
            this.GroupBox.Controls.SetChildIndex(this.textBox1, 0);

            // lblStatus
            this.StatusLabel.Location = new Point(12, 20);

            // textBox1
            this.textBox1.Location = new Point(44, 17);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(440, 20);
            this.textBox1.TabIndex = 3;
            this.textBox1.TextChanged += new EventHandler(this.TextBox1TextChanged);
            this.textBox1.Click += new EventHandler(this.TextBox1Click);

            // DoubleElement
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.Name = "DoubleElement";
            this.Size = new Size(492, 46);
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

        private TextBox textBox1;
    }
}
