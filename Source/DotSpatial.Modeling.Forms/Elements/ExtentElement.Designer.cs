using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms.Elements
{
    internal partial class ExtentElement
    {
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            lblExtent = new Label();
            cmdSelect = new Button();
            SuspendLayout();

            // groupBox1
            GroupBox.Controls.Add(lblExtent);
            GroupBox.Controls.Add(cmdSelect);
            GroupBox.Controls.SetChildIndex(cmdSelect, 0);
            GroupBox.Controls.SetChildIndex(lblExtent, 0);
            GroupBox.Controls.SetChildIndex(StatusLabel, 0);

            // lblProjection
            lblExtent.Anchor = (AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right;
            lblExtent.BackColor = Color.White;
            lblExtent.BorderStyle = BorderStyle.Fixed3D;
            lblExtent.Location = new Point(39, 16);
            lblExtent.Name = "lblExtent";
            lblExtent.Size = new Size(405, 20);
            lblExtent.TabIndex = 2;
            lblExtent.Text = ModelingMessageStrings.ExtentElement_Press_button;

            // cmdSelect
            cmdSelect.Location = new Point(450, 15);
            cmdSelect.Name = "cmdSelect";
            cmdSelect.Size = new Size(36, 23);
            cmdSelect.TabIndex = 3;
            cmdSelect.Text = "...";
            cmdSelect.UseVisualStyleBackColor = true;
            cmdSelect.Click += CmdSelectClick;

            // ProjectionElement
            AutoScaleDimensions = new SizeF(6F, 13F);
            Name = "ProjectionElement";
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

        private Button cmdSelect;
        private Label lblExtent;
    }
}
