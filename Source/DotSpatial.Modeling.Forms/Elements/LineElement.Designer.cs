using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms.Elements
{
    internal partial class LineElement
    {
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            btnAddData = new Button();
            comboFeatures = new ComboBox();
            GroupBox.SuspendLayout();
            SuspendLayout();

            // groupBox1
            GroupBox.Controls.Add(comboFeatures);
            GroupBox.Controls.Add(btnAddData);
            GroupBox.Size = new Size(492, 46);
            GroupBox.Text = "Caption";
            GroupBox.Controls.SetChildIndex(btnAddData, 0);
            GroupBox.Controls.SetChildIndex(comboFeatures, 0);
            GroupBox.Controls.SetChildIndex(StatusLabel, 0);

            // lblStatus
            StatusLabel.Location = new Point(12, 20);

            // btnAddData
            btnAddData.Image = Images.AddLayer;
            btnAddData.Location = new Point(460, 14);
            btnAddData.Name = "btnAddData";
            btnAddData.Size = new Size(26, 26);
            btnAddData.TabIndex = 4;
            btnAddData.UseVisualStyleBackColor = true;
            btnAddData.Click += BtnAddDataClick;

            // comboFeatures
            comboFeatures.DropDownStyle = ComboBoxStyle.DropDownList;
            comboFeatures.FormattingEnabled = true;
            comboFeatures.Location = new Point(44, 17);
            comboFeatures.Name = "comboFeatures";
            comboFeatures.Size = new Size(410, 21);
            comboFeatures.TabIndex = 5;
            comboFeatures.SelectedValueChanged += ComboFeatureSelectedValueChanged;
            comboFeatures.Click += DialogElementClick;

            // LineElement
            AutoScaleDimensions = new SizeF(6F, 13F);
            Name = "LineElement";
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
        private ComboBox comboFeatures;

    }
}
