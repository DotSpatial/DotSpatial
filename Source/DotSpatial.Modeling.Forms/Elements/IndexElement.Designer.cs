using System;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms.Elements
{
    internal partial class IndexElement
    {
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.textBox1 = new TextBox();
            this.btnSelect = new Button();
            this.GroupBox.SuspendLayout();
            this.SuspendLayout();

            // groupBox1
            this.GroupBox.Controls.Add(this.textBox1);
            this.GroupBox.Controls.Add(this.btnSelect);
            this.GroupBox.Size = new Size(492, 46);
            this.GroupBox.Text = "Caption";
            this.GroupBox.Controls.SetChildIndex(this.StatusLabel, 0);
            this.GroupBox.Controls.SetChildIndex(this.textBox1, 0);
            this.GroupBox.Controls.SetChildIndex(this.btnSelect, 0);

            // lblStatus
            this.StatusLabel.Location = new Point(12, 20);

            // btnSelect
            this.btnSelect.Image = Images.AddLayer;
            this.btnSelect.Location = new Point(460, 14);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new Size(26, 26);
            this.btnSelect.TabIndex = 4;
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new EventHandler(this.BtnSelectClick);

            // textBox1
            this.textBox1.Location = new Point(44, 17);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(400, 20);
            this.textBox1.TabIndex = 3;

            // this.textBox1.TextChanged += new EventHandler(this.textBox1_TextChanged);
            this.textBox1.Click += new EventHandler(this.TextBox1Click);

            // IndexElement
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.Name = "IndexElement";
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

        private Button btnSelect;
        private TextBox textBox1;
    }
}
