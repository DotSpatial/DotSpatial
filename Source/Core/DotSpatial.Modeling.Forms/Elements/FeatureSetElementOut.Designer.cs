using System;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms.Elements
{
    public partial class FeatureSetElementOut
    {
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.btnAddData = new Button();
            this.textBox1 = new TextBox();
            this.GroupBox.SuspendLayout();
            this.SuspendLayout();

            // groupBox1
            this.GroupBox.Controls.Add(this.textBox1);
            this.GroupBox.Controls.Add(this.btnAddData);
            this.GroupBox.Controls.SetChildIndex(this.StatusLabel, 0);
            this.GroupBox.Controls.SetChildIndex(this.btnAddData, 0);
            this.GroupBox.Controls.SetChildIndex(this.textBox1, 0);

            // lblStatus
            this.StatusLabel.Location = new Point(12, 20);

            // btnAddData
            this.btnAddData.Image = Images.AddLayer;
            this.btnAddData.Location = new Point(460, 14);
            this.btnAddData.Name = "btnAddData";
            this.btnAddData.Size = new Size(26, 26);
            this.btnAddData.TabIndex = 5;
            this.btnAddData.UseVisualStyleBackColor = true;
            this.btnAddData.Click += new EventHandler(this.BtnAddDataClick);

            // textBox1
            this.textBox1.Location = new Point(44, 17);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new Size(410, 20);
            this.textBox1.TabIndex = 6;

            // PolygonElementOut
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.Name = "LineElementOut";
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

        private Button btnAddData;
        private TextBox textBox1;
    }
}
