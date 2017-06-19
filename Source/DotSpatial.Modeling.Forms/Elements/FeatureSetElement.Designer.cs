using System;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms.Elements
{
    public partial class FeatureSetElement
    {
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this._btnAddData = new Button();
            this._comboFeatures = new ComboBox();
            this.GroupBox.SuspendLayout();
            this.SuspendLayout();

            // groupBox1
            this.GroupBox.Controls.Add(this._comboFeatures);
            this.GroupBox.Controls.Add(this._btnAddData);
            this.GroupBox.Size = new Size(492, 46);
            this.GroupBox.Text = "Caption";
            this.GroupBox.Controls.SetChildIndex(this._btnAddData, 0);
            this.GroupBox.Controls.SetChildIndex(this._comboFeatures, 0);
            this.GroupBox.Controls.SetChildIndex(this.StatusLabel, 0);

            // btnAddData
            this._btnAddData.Image = Images.AddLayer;
            this._btnAddData.Location = new Point(460, 14);
            this._btnAddData.Name = "_btnAddData";
            this._btnAddData.Size = new Size(26, 26);
            this._btnAddData.TabIndex = 4;
            this._btnAddData.UseVisualStyleBackColor = true;
            this._btnAddData.Click += new EventHandler(this.BtnAddDataClick);

            // comboFeatures
            this._comboFeatures.DropDownStyle = ComboBoxStyle.DropDownList;
            this._comboFeatures.FormattingEnabled = true;
            this._comboFeatures.Location = new Point(44, 17);
            this._comboFeatures.Name = "_comboFeatures";
            this._comboFeatures.Size = new Size(410, 21);
            this._comboFeatures.TabIndex = 5;
            this._comboFeatures.SelectedValueChanged += new EventHandler(this.ComboFeatureSelectedValueChanged);
            this._comboFeatures.Click += new EventHandler(DialogElementClick);

            // FeatureSetElement
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.Name = "FeatureSetElement";
            this.Size = new Size(492, 46);
            this.GroupBox.ResumeLayout(false);
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

        private Button _btnAddData;
        private ComboBox _comboFeatures;
    }
}
