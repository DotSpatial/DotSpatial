// ********************************************************************************************************
// Product Name: DotSpatial.Tools.ModelerMenuStrip
// Description:  A menu strip designed to work along with the modeler
//
// ********************************************************************************************************
//
// The Original Code is Toolbox.dll for the DotSpatial 4.6/6 ToolManager project
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Apr, 2009.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// A Menu strip that is part of Brian's toolbox
    /// </summary>
    [ToolboxItem(false)]
    public class ModelerMenuStrip : MenuStrip
    {
        ToolStripMenuItem _toolStripMenuFile;

        /// <summary>
        /// Creates a new instance of hte ModelerMenuStrip
        /// </summary>
        public ModelerMenuStrip()
        {
            InitializeComponent();
        }

        #region Component Designer generated code

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new Container();

            this.SuspendLayout();

            //
            // toolStripMenuItem1
            //
            _toolStripMenuFile = new ToolStripMenuItem();
            _toolStripMenuFile.Size = new Size(113, 20);
            _toolStripMenuFile.Text = "File";

            this.Items.Add(_toolStripMenuFile);

            this.ResumeLayout();
        }

        #endregion
    }
}