using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    public partial class VerticalScrollControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            _scrVertical = new VScrollBar();
            SuspendLayout();
            //
            // scrVertical
            //
            _scrVertical.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom)
                                 | AnchorStyles.Right;
            _scrVertical.Location = new Point(170, 0);
            _scrVertical.Name = "_scrVertical";
            _scrVertical.Size = new Size(17, 425);
            _scrVertical.TabIndex = 0;
            _scrVertical.Scroll += ScrVerticalScroll;
            //
            // VerticalScrollControl
            //
            Controls.Add(_scrVertical);
            Name = "ScrollingControl";
            Size = new Size(187, 428);
            ResumeLayout(false);
        }

        #endregion
    }
}