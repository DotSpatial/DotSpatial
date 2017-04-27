using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms.Elements
{
    internal partial class ProjectionElement
    {
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            _lblProjection = new Label();
            _cmdSelect = new Button();
            SuspendLayout();

            // groupBox1
            GroupBox.Controls.Add(_lblProjection);
            GroupBox.Controls.Add(_cmdSelect);
            GroupBox.Controls.SetChildIndex(_cmdSelect, 0);
            GroupBox.Controls.SetChildIndex(_lblProjection, 0);
            GroupBox.Controls.SetChildIndex(StatusLabel, 0);

            // lblProjection
            _lblProjection.Anchor = (AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right;
            _lblProjection.BackColor = Color.White;
            _lblProjection.BorderStyle = BorderStyle.Fixed3D;
            _lblProjection.Location = new Point(39, 16);
            _lblProjection.Name = "_lblProjection";
            _lblProjection.Size = new Size(405, 20);
            _lblProjection.TabIndex = 2;
            _lblProjection.Text = ModelingMessageStrings.ProjectionElement_PressButtonToSelectProjection;

            // cmdSelect
            _cmdSelect.Location = new Point(450, 15);
            _cmdSelect.Name = "_cmdSelect";
            _cmdSelect.Size = new Size(36, 23);
            _cmdSelect.TabIndex = 3;
            _cmdSelect.Text = ModelingMessageStrings.SelectButtonText;
            _cmdSelect.UseVisualStyleBackColor = true;
            _cmdSelect.Click += CmdSelectClick;

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

        private Button _cmdSelect;
        private Label _lblProjection;
    }
}
