using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms.Elements
{
    internal partial class RasterElement
    {
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            btnAddData = new Button();
            comboRaster = new ComboBox();
            GroupBox.SuspendLayout();
            SuspendLayout();

            // groupBox1
            GroupBox.Controls.Add(comboRaster);
            GroupBox.Controls.Add(btnAddData);
            GroupBox.Size = new Size(492, 46);
            GroupBox.Text = "Caption";
            GroupBox.Controls.SetChildIndex(btnAddData, 0);
            GroupBox.Controls.SetChildIndex(comboRaster, 0);
            GroupBox.Controls.SetChildIndex(StatusLabel, 0);

            // btnAddData
            btnAddData.Image = Images.AddLayer;
            btnAddData.Location = new Point(460, 14);
            btnAddData.Name = "btnAddData";
            btnAddData.Size = new Size(26, 26);
            btnAddData.TabIndex = 4;
            btnAddData.UseVisualStyleBackColor = true;
            btnAddData.Click += BtnAddDataClick;

            // comboFeatures
            comboRaster.DropDownStyle = ComboBoxStyle.DropDownList;
            comboRaster.FormattingEnabled = true;
            comboRaster.Location = new Point(44, 17);
            comboRaster.Name = "comboFeatures";
            comboRaster.Size = new Size(410, 21);
            comboRaster.TabIndex = 5;
            comboRaster.SelectedValueChanged += ComboRasterSelectedValueChanged;
            comboRaster.Click += DialogElementClick;

            // RasterElement
            AutoScaleDimensions = new SizeF(6F, 13F);
            Name = "RasterElement";
            Size = new Size(492, 46);
            GroupBox.ResumeLayout(false);
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
        private ComboBox comboRaster;

    }
}
