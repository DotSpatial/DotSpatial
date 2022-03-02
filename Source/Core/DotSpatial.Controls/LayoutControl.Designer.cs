using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    partial class LayoutControl
    {
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

        #region Windows Form Designer generated code


        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(LayoutControl));
            this._hScrollBarPanel = new Panel();
            this._hScrollBar = new HScrollBar();
            this._vScrollBar = new VScrollBar();
            this._contextMenuRight = new ContextMenuStrip();
            this._cMnuMoveUp = new ToolStripMenuItem();
            this._cMnuMoveDown = new ToolStripMenuItem();
            this._menuItem2 = new ToolStripMenuItem();
            this._cMnuSelAli = new ToolStripMenuItem();
            this._cMnuSelLeft = new ToolStripMenuItem();
            this._cMnuSelRight = new ToolStripMenuItem();
            this._cMnuSelTop = new ToolStripMenuItem();
            this._cMnuSelBottom = new ToolStripMenuItem();
            this._cMnuSelHor = new ToolStripMenuItem();
            this._cMnuSelVert = new ToolStripMenuItem();
            this._cMnuMarAli = new ToolStripMenuItem();
            this._cMnuMargLeft = new ToolStripMenuItem();
            this._cMnuMargRight = new ToolStripMenuItem();
            this._cMnuMargTop = new ToolStripMenuItem();
            this._cMnuMargBottom = new ToolStripMenuItem();
            this._cMnuMargHor = new ToolStripMenuItem();
            this._cMnuMargVert = new ToolStripMenuItem();
            this._menuItem19 = new ToolStripMenuItem();
            this._cMnuSelFit = new ToolStripMenuItem();
            this._cMnuSelWidth = new ToolStripMenuItem();
            this._cMnuSelHeight = new ToolStripMenuItem();
            this._cMnuMarFit = new ToolStripMenuItem();
            this._cMnuMargWidth = new ToolStripMenuItem();
            this._cMnuMargHeight = new ToolStripMenuItem();
            this._menuItem4 = new ToolStripMenuItem();
            this._cMnuDelete = new ToolStripMenuItem();
            this._hScrollBarPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // _hScrollBarPanel
            //
            this._hScrollBarPanel.Controls.Add(this._hScrollBar);
            resources.ApplyResources(this._hScrollBarPanel, "_hScrollBarPanel");
            this._hScrollBarPanel.Name = "_hScrollBarPanel";
            //
            // _hScrollBar
            //
            resources.ApplyResources(this._hScrollBar, "_hScrollBar");
            this._hScrollBar.Name = "_hScrollBar";
            this._hScrollBar.Scroll += new ScrollEventHandler(this.HScrollBarScroll);
            //
            // _vScrollBar
            //
            resources.ApplyResources(this._vScrollBar, "_vScrollBar");
            this._vScrollBar.Name = "_vScrollBar";
            this._vScrollBar.Scroll += new ScrollEventHandler(this.VScrollBarScroll);
            //
            // _contextMenuRight
            //
            this._contextMenuRight.Items.AddRange(new ToolStripMenuItem[] {
                                                                         this._cMnuMoveUp,
                                                                         this._cMnuMoveDown,
                                                                         this._menuItem2,
                                                                         this._cMnuSelAli,
                                                                         this._cMnuMarAli,
                                                                         this._menuItem19,
                                                                         this._cMnuSelFit,
                                                                         this._cMnuMarFit,
                                                                         this._menuItem4,
                                                                         this._cMnuDelete});
            //
            // _cMnuMoveUp
            //
            this._cMnuMoveUp.Text = MessageStrings.LayoutMenuStripSelectMoveUp;
            this._cMnuMoveUp.Click += new EventHandler(this.CMnuMoveUpClick);
            //
            // _cMnuMoveDown
            //
            this._cMnuMoveDown.Text = MessageStrings.LayoutMenuStripSelectMoveDown;
            this._cMnuMoveDown.Click += new EventHandler(this.CMnuMoveDownClick);
            //
            // _menuItem2
            //
            resources.ApplyResources(this._menuItem2, "_menuItem2");
            //
            // _cMnuSelAli
            //
            this._cMnuSelAli.DropDownItems.AddRange(new ToolStripMenuItem[] {
                                                                   this._cMnuSelLeft,
                                                                   this._cMnuSelRight,
                                                                   this._cMnuSelTop,
                                                                   this._cMnuSelBottom,
                                                                   this._cMnuSelHor,
                                                                   this._cMnuSelVert});
            this._cMnuSelAli.Text = MessageStrings.LayoutCmnuSelectionAlignment;
            //
            // _cMnuSelLeft
            //
            this._cMnuSelLeft.Text = MessageStrings.LayoutCmnuLeft;
            this._cMnuSelLeft.Click += new EventHandler(this.CMnuSelLeftClick);
            //
            // _cMnuSelRight
            //
            this._cMnuSelRight.Text = MessageStrings.LayoutCmnuRight;
            this._cMnuSelRight.Click += new EventHandler(this.CMnuSelRightClick);
            //
            // _cMnuSelTop
            //
            this._cMnuSelTop.Text = MessageStrings.LayoutCmnuTop;
            this._cMnuSelTop.Click += new EventHandler(this.CMnuSelTopClick);
            //
            // _cMnuSelBottom
            //
            this._cMnuSelBottom.Text = MessageStrings.LayoutCmnuBottom;
            this._cMnuSelBottom.Click += new EventHandler(this.CMnuSelBottomClick);
            //
            // _cMnuSelHor
            //
            this._cMnuSelHor.Text = MessageStrings.LayoutCmnuHor;
            this._cMnuSelHor.Click += new EventHandler(this.CMnuSelHorClick);
            //
            // _cMnuSelVert
            //
            this._cMnuSelVert.Text = MessageStrings.LayoutCmnuVert;
            this._cMnuSelVert.Click += new EventHandler(this.CMnuSelVertClick);
            //
            // _cMnuMarAli
            //
            this._cMnuMarAli.DropDownItems.AddRange(new ToolStripMenuItem[] {
                                                                   this._cMnuMargLeft,
                                                                   this._cMnuMargRight,
                                                                   this._cMnuMargTop,
                                                                   this._cMnuMargBottom,
                                                                   this._cMnuMargHor,
                                                                   this._cMnuMargVert});
            this._cMnuMarAli.Text = MessageStrings.LayoutCmnuMargAlign;
            //
            // _cMnuMargLeft
            //
            this._cMnuMargLeft.Text = MessageStrings.LayoutCmnuLeft;
            this._cMnuMargLeft.Click += new EventHandler(this.CMnuMargLeftClick);
            //
            // _cMnuMargRight
            //
            this._cMnuMargRight.Text = MessageStrings.LayoutCmnuRight;
            this._cMnuMargRight.Click += new EventHandler(this.CMnuMargRightClick);
            //
            // _cMnuMargTop
            //
            this._cMnuMargTop.Text = MessageStrings.LayoutCmnuTop;
            this._cMnuMargTop.Click += new EventHandler(this.CMnuMargTopClick);
            //
            // _cMnuMargBottom
            //
            this._cMnuMargBottom.Text = MessageStrings.LayoutCmnuBottom;
            this._cMnuMargBottom.Click += new EventHandler(this.CMnuMargBottomClick);
            //
            // _cMnuMargHor
            //
            this._cMnuMargHor.Text = MessageStrings.LayoutCmnuHor;
            this._cMnuMargHor.Click += new EventHandler(this.CMnuMargHorClick);
            //
            // _cMnuMargVert
            //
            this._cMnuMargVert.Text = MessageStrings.LayoutCmnuVert;
            this._cMnuMargVert.Click += new EventHandler(this.CMnuMargVertClick);
            //
            // _menuItem19
            //
            resources.ApplyResources(this._menuItem19, "_menuItem19");
            //
            // _cMnuSelFit
            //
            this._cMnuSelFit.DropDownItems.AddRange(new ToolStripMenuItem[] {
                                                                   this._cMnuSelWidth,
                                                                   this._cMnuSelHeight});
            this._cMnuSelFit.Text = MessageStrings.LayoutCmnuSelectionFit;
            //
            // _cMnuSelWidth
            //
            this._cMnuSelWidth.Text = MessageStrings.LayoutCmnuWidth;
            this._cMnuSelWidth.Click += new EventHandler(this.CMnuSelWidthClick);
            //
            // _cMnuSelHeight
            //
            this._cMnuSelHeight.Text = MessageStrings.LayoutCmnuHeight;
            this._cMnuSelHeight.Click += new EventHandler(this.CMnuSelHeightClick);
            //
            // _cMnuMarFit
            //
            this._cMnuMarFit.DropDownItems.AddRange(new ToolStripMenuItem[] {
                                                                   this._cMnuMargWidth,
                                                                   this._cMnuMargHeight});
            this._cMnuMarFit.Text = MessageStrings.LayoutCmnuMarginFit;
            //
            // _cMnuMargWidth
            //
            this._cMnuMargWidth.Text = MessageStrings.LayoutCmnuWidth;
            this._cMnuMargWidth.Click += new EventHandler(this.CMnuMargWidthClick);
            //
            // _cMnuMargHeight
            //
            this._cMnuMargHeight.Text = MessageStrings.LayoutCmnuHeight;
            this._cMnuMargHeight.Click += new EventHandler(this.CMnuMargHeightClick);
            //
            // _menuItem4
            //
            resources.ApplyResources(this._menuItem4, "_menuItem4");
            //
            // _cMnuDelete
            //
            this._cMnuDelete.Text = MessageStrings.LayoutMenuStripSelectDelete;
            this._cMnuDelete.Click += new EventHandler(this.CMnuDeleteClick);
            //
            // LayoutControl
            //
            resources.ApplyResources(this, "$this");
            //
            this.Controls.Add(this._vScrollBar);
            this.Controls.Add(this._hScrollBarPanel);
            this.Name = "LayoutControl";
            this.MouseMove += new MouseEventHandler(this.LayoutControlMouseMove);
            this.KeyUp += new KeyEventHandler(this.LayoutControlKeyUp);
            this.MouseDown += new MouseEventHandler(this.LayoutControlMouseDown);
            this.Resize += new EventHandler(this.LayoutControlResize);
            this.MouseUp += new MouseEventHandler(this.LayoutControlMouseUp);
            this._hScrollBarPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion Windows Form Designer generated code

        private ToolStripMenuItem _cMnuDelete;
        private ToolStripMenuItem _cMnuMarAli;
        private ToolStripMenuItem _cMnuMarFit;
        private ToolStripMenuItem _cMnuMargBottom;
        private ToolStripMenuItem _cMnuMargHeight;
        private ToolStripMenuItem _cMnuMargHor;
        private ToolStripMenuItem _cMnuMargLeft;
        private ToolStripMenuItem _cMnuMargRight;
        private ToolStripMenuItem _cMnuMargTop;
        private ToolStripMenuItem _cMnuMargVert;
        private ToolStripMenuItem _cMnuMargWidth;
        private ToolStripMenuItem _cMnuMoveDown;
        private ToolStripMenuItem _cMnuMoveUp;
        private ToolStripMenuItem _cMnuSelAli;
        private ToolStripMenuItem _cMnuSelBottom;
        private ToolStripMenuItem _cMnuSelFit;
        private ToolStripMenuItem _cMnuSelHeight;
        private ToolStripMenuItem _cMnuSelHor;
        private ToolStripMenuItem _cMnuSelLeft;
        private ToolStripMenuItem _cMnuSelRight;
        private ToolStripMenuItem _cMnuSelTop;
        private ToolStripMenuItem _cMnuSelVert;
        private ToolStripMenuItem _cMnuSelWidth;
        private ContextMenuStrip _contextMenuRight;
        private HScrollBar _hScrollBar;
        private Panel _hScrollBarPanel;
        private LayoutDocToolStrip _layoutDocToolStrip;
        private LayoutInsertToolStrip _layoutInsertToolStrip;
        private LayoutListBox _layoutListBox;
        private LayoutMapToolStrip _layoutMapToolStrip;
        private LayoutMenuStrip _layoutMenuStrip;
        private LayoutPropertyGrid _layoutPropertyGrip;
        private LayoutZoomToolStrip _layoutZoomToolStrip;
        private ToolStripMenuItem _menuItem19;
        private ToolStripMenuItem _menuItem2;
        private ToolStripMenuItem _menuItem4;
        private VScrollBar _vScrollBar;
    }
}
