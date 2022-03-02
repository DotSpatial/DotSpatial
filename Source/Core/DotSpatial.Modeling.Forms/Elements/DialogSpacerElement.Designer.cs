using System;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms.Elements
{
    public partial class DialogSpacerElement
    {
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this._label1 = new Label();
            this.GroupBox.SuspendLayout();
            this.SuspendLayout();

            // groupBox1
            this.GroupBox.Size = new Size(492, 33);
            this.GroupBox.Visible = false;
            this.GroupBox.Controls.SetChildIndex(this.StatusLabel, 0);

            // lblStatus
            this.StatusLabel.Visible = false;

            // label1
            this.Controls.Add(this._label1);
            this.Controls.SetChildIndex(this._label1, 0);
            this._label1.AutoSize = true;
            this._label1.Font = new Font("Microsoft Sans Serif", 9.75F, (FontStyle)((FontStyle.Bold | FontStyle.Underline)), GraphicsUnit.Point, (byte)(0));
            this._label1.Location = new Point(8, 12);
            this._label1.Name = "_label1";
            this._label1.Size = new Size(67, 16);
            this._label1.TabIndex = 3;
            this._label1.Text = "asdfsdfs";
            this._label1.Click += new EventHandler(this.Label1Click);

            // DialogSpacerElement
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.Name = "DialogSpacerElement";
            this.Size = new Size(492, 33);
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

        private Label _label1;
    }
}
