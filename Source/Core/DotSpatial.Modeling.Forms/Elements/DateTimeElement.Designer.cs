using System;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms.Elements
{
    internal partial class DateTimeElement
    {
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this._dateTimePicker2 = new DateTimePicker();
            this.GroupBox.SuspendLayout();
            this.SuspendLayout();

            // GroupBox1
            this.GroupBox.Controls.Add(this._dateTimePicker2);
            this.GroupBox.Controls.SetChildIndex(this._dateTimePicker2, 0);

            // _dateTimePicker2
            this._dateTimePicker2.Format = DateTimePickerFormat.Short;
            this._dateTimePicker2.Location = new Point(55, 15);
            this._dateTimePicker2.Name = "_dateTimePicker2";
            this._dateTimePicker2.Size = new Size(200, 20);
            this._dateTimePicker2.TabIndex = 2;
            this._dateTimePicker2.ValueChanged += new EventHandler(this.DateTimePicker2ValueChanged);

            // DateTimeElement
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.Name = "DateTimeElement";
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

    
    }
}
