using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    public partial class Modeler
    {
        #region Component Designer generated code

        /// <summary>
        /// Required designer variable.
        /// </summary>
        protected IContainer components;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Modeler));
            this._panelHScroll = new System.Windows.Forms.Panel();
            this._horScroll = new System.Windows.Forms.HScrollBar();
            this._verScroll = new System.Windows.Forms.VScrollBar();
            this._contMenuRc = new System.Windows.Forms.ContextMenu();
            this._menuItem1 = new System.Windows.Forms.MenuItem();
            this._panelHScroll.SuspendLayout();
            this.SuspendLayout();
            //
            // _panelHScroll
            //
            resources.ApplyResources(this._panelHScroll, "_panelHScroll");
            this._panelHScroll.BackColor = System.Drawing.SystemColors.Control;
            this._panelHScroll.Controls.Add(this._horScroll);
            this._panelHScroll.Name = "_panelHScroll";
            //
            // _horScroll
            //
            resources.ApplyResources(this._horScroll, "_horScroll");
            this._horScroll.Name = "_horScroll";
            //
            // _verScroll
            //
            resources.ApplyResources(this._verScroll, "_verScroll");
            this._verScroll.Name = "_verScroll";
            //
            // _contMenuRc
            //
            this._contMenuRc.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                        this._menuItem1});
            resources.ApplyResources(this._contMenuRc, "_contMenuRc");
            //
            // _menuItem1
            //
            resources.ApplyResources(this._menuItem1, "_menuItem1");
            this._menuItem1.Index = 0;
            //
            // Modeler
            //
            resources.ApplyResources(this, "$this");
            this.AllowDrop = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.Controls.Add(this._verScroll);
            this.Controls.Add(this._panelHScroll);
            this.Name = "Modeler";
            this._panelHScroll.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private ContextMenu _contMenuRc;
        private HScrollBar _horScroll;
        private MenuItem _menuItem1;
        private Panel _panelHScroll;
        private VScrollBar _verScroll;

    }
}
