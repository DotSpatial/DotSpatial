using System;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms.Elements
{
    public partial class SaveFileElement
    {
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.txtDataTable = new TextBox();
            this.btnAddData = new Button();
            this.GroupBox.SuspendLayout();
            this.SuspendLayout();

            // GroupBox1
            this.GroupBox.Controls.Add(this.btnAddData);
            this.GroupBox.Controls.SetChildIndex(this.btnAddData, 0);

            // txtDataTable
            this.txtDataTable.Location = new Point(45, 13);
            this.txtDataTable.Name = "txtDataTable";
            this.txtDataTable.Size = new Size(356, 20);
            this.txtDataTable.TabIndex = 9;

            // btnAddData
            this.btnAddData.Image = Images.AddLayer;
            this.btnAddData.Location = new Point(432, 10);
            this.btnAddData.Name = "btnAddData";
            this.btnAddData.Size = new Size(26, 26);
            this.btnAddData.TabIndex = 8;
            this.btnAddData.UseVisualStyleBackColor = true;
            this.btnAddData.Click += new EventHandler(this.BtnAddDataClick);

            // SaveFileElement
            this.AutoScaleDimensions = new SizeF(6F, 13F);

            this.Controls.Add(this.txtDataTable);
            this.Name = "SaveFileElement";
            this.Controls.SetChildIndex(this.GroupBox, 0);
            this.Controls.SetChildIndex(this.txtDataTable, 0);
            this.GroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
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
        private TextBox txtDataTable;
    }
}
