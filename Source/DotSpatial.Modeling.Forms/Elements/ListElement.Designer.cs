using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms.Elements
{
    internal partial class ListElement
    {
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            comboBox1 = new ComboBox();
            GroupBox.SuspendLayout();
            SuspendLayout();

            // groupBox1
            GroupBox.Controls.Add(comboBox1);
            GroupBox.Size = new Size(492, 46);
            GroupBox.Text = "Caption";
            GroupBox.Controls.SetChildIndex(comboBox1, 0);
            GroupBox.Controls.SetChildIndex(StatusLabel, 0);

            // lblStatus
            StatusLabel.Location = new Point(12, 20);

            // comboBox1
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(44, 17);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(440, 21);
            comboBox1.TabIndex = 5;
            comboBox1.SelectedValueChanged += ComboBox1SelectedValueChanged;
            comboBox1.Click += ComboBox1Click;

            // ListElement
            AutoScaleDimensions = new SizeF(6F, 13F);
            Name = "ListElement";
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

        private ComboBox comboBox1;


    }
}
