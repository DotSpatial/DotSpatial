using System;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms.Elements
{
    public partial class IntElement
    {
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.txtValue = new TextBox();
            this.GroupBox.SuspendLayout();
            this.SuspendLayout();

            // groupBox1
            this.GroupBox.Controls.Add(this.txtValue);
            this.GroupBox.Text = "Caption";
            this.GroupBox.Controls.SetChildIndex(this.txtValue, 0);
            this.GroupBox.Controls.SetChildIndex(this.StatusLabel, 0);

            // lblStatus
            this.StatusLabel.Location = new Point(12, 20);

            // textBox1
            this.txtValue.Location = new Point(44, 17);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new Size(440, 20);
            this.txtValue.TabIndex = 3;
            this.txtValue.TextChanged += new EventHandler(this.TextBox1TextChanged);
            this.txtValue.Click += new EventHandler(this.TextBox1Click);

            // IntElement
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.Name = "IntElement";
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

        private TextBox txtValue;
    }
}
