using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms.Elements
{
    internal partial class StringElement
    {
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            textBox1 = new TextBox();
            GroupBox.SuspendLayout();
            SuspendLayout();

            // groupBox1
            GroupBox.Controls.Add(textBox1);
            GroupBox.Text = "Caption";
            GroupBox.Controls.SetChildIndex(textBox1, 0);
            GroupBox.Controls.SetChildIndex(StatusLabel, 0);

            // lblStatus
            StatusLabel.Location = new Point(12, 20);

            // textBox1
            textBox1.Location = new Point(44, 17);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(440, 20);
            textBox1.TabIndex = 3;
            textBox1.TextChanged += TextBox1TextChanged;
            textBox1.Click += TextBox1Click;

            // StringElement
            AutoScaleDimensions = new SizeF(6F, 13F);
            Name = "StringElement";
            GroupBox.ResumeLayout(false);
            GroupBox.PerformLayout();
            ResumeLayout(false);
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
