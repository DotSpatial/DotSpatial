using System;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms.Elements
{
    public partial class OpenFileElement
    {
        #region Windows Form Designer generated code
       private void InitializeComponent()
        {
            this._btnAddData = new Button();
            this._comboFile = new ComboBox();
            this.GroupBox.SuspendLayout();
            this.SuspendLayout();

            // GroupBox
            this.GroupBox.Controls.Add(this._btnAddData);
            this.GroupBox.Controls.Add(this._comboFile);
            this.GroupBox.Controls.SetChildIndex(this._comboFile, 0);
            this.GroupBox.Controls.SetChildIndex(this._btnAddData, 0);

            // btnAddData
            this._btnAddData.Image = Images.AddLayer;
            this._btnAddData.Location = new Point(450, 10);
            this._btnAddData.Name = "_btnAddData";
            this._btnAddData.Size = new Size(26, 26);
            this._btnAddData.TabIndex = 9;
            this._btnAddData.UseVisualStyleBackColor = true;
            this._btnAddData.Click += new EventHandler(this.BtnAddDataClick);

            // comboFile
            this._comboFile.DropDownStyle = ComboBoxStyle.DropDownList;
            this._comboFile.FormattingEnabled = true;
            this._comboFile.Location = new Point(34, 12);
            this._comboFile.Name = "_comboFile";
            this._comboFile.Size = new Size(410, 21);
            this._comboFile.TabIndex = 10;
            this._comboFile.SelectedValueChanged += new EventHandler(this.ComboFileSelectedValueChanged);

            // OpenFileElement
            this.AutoScaleDimensions = new SizeF(6F, 13F);

            this.Name = "OpenFileElement";
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
        private ComboBox _comboFile;

    }
}
