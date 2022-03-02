using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms
{
    public partial class ModelerMenuStrip
    {
        #region Windows Form Designer generated code
        private IContainer components = null;

        private void InitializeComponent()
        {
            components = new Container();

            this.SuspendLayout();

            // toolStripMenuItem1
            _toolStripMenuFile = new ToolStripMenuItem();
            _toolStripMenuFile.Size = new Size(113, 20);
            _toolStripMenuFile.Text = "File";

            this.Items.Add(_toolStripMenuFile);

            this.ResumeLayout();
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

        private ToolStripMenuItem _toolStripMenuFile;

    }
}
