using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms.Elements
{
    internal partial class PolygonElementOut
    {
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            btnAddData = new Button();
            textBox1 = new TextBox();
            GroupBox.SuspendLayout();
            SuspendLayout();

            // groupBox1
            GroupBox.Controls.Add(textBox1);
            GroupBox.Controls.Add(btnAddData);
            GroupBox.Controls.SetChildIndex(StatusLabel, 0);
            GroupBox.Controls.SetChildIndex(btnAddData, 0);
            GroupBox.Controls.SetChildIndex(textBox1, 0);

            // lblStatus
            StatusLabel.Location = new Point(12, 20);

            // btnAddData
            btnAddData.Image = Images.AddLayer;
            btnAddData.Location = new Point(460, 14);
            btnAddData.Name = "btnAddData";
            btnAddData.Size = new Size(26, 26);
            btnAddData.TabIndex = 5;
            btnAddData.UseVisualStyleBackColor = true;
            btnAddData.Click += BtnAddDataClick;

            // textBox1
            textBox1.Location = new Point(44, 17);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(410, 20);
            textBox1.TabIndex = 6;

            // PolygonElementOut
            AutoScaleDimensions = new SizeF(6F, 13F);
            Name = "PolygonElementOut";
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

        private Button btnAddData;
        private TextBox textBox1;


    }
}
