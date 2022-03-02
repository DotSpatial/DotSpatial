using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms.Elements
{
    internal partial class PolygonElement
    {    
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            _btnAddData = new Button();
            _comboFeatures = new ComboBox();
            GroupBox.SuspendLayout();
            SuspendLayout();

            // groupBox1
            GroupBox.Controls.Add(_comboFeatures);
            GroupBox.Controls.Add(_btnAddData);
            GroupBox.Size = new Size(492, 46);
            GroupBox.Text = "Caption";
            GroupBox.Controls.SetChildIndex(_btnAddData, 0);
            GroupBox.Controls.SetChildIndex(_comboFeatures, 0);
            GroupBox.Controls.SetChildIndex(StatusLabel, 0);

            // lblStatus
            StatusLabel.Location = new Point(12, 20);

            // btnAddData
            _btnAddData.Image = Images.AddLayer;
            _btnAddData.Location = new Point(460, 14);
            _btnAddData.Name = "_btnAddData";
            _btnAddData.Size = new Size(26, 26);
            _btnAddData.TabIndex = 4;
            _btnAddData.UseVisualStyleBackColor = true;
            _btnAddData.Click += BtnAddDataClick;

            // comboFeatures
            _comboFeatures.DropDownStyle = ComboBoxStyle.DropDownList;
            _comboFeatures.FormattingEnabled = true;
            _comboFeatures.Location = new Point(44, 17);
            _comboFeatures.Name = "_comboFeatures";
            _comboFeatures.Size = new Size(410, 21);
            _comboFeatures.TabIndex = 5;
            _comboFeatures.SelectedValueChanged += ComboFeatureSelectedValueChanged;
            _comboFeatures.Click += DialogElementClick;

            // PolygonElement
            AutoScaleDimensions = new SizeF(6F, 13F);
            Name = "PolygonElement";
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

        private Button _btnAddData;
        private ComboBox _comboFeatures;


    }
}
