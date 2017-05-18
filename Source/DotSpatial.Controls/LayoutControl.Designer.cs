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
            this._contextMenuRight = new ContextMenu();
            this._cMnuMoveUp = new MenuItem();
            this._cMnuMoveDown = new MenuItem();
            this._menuItem2 = new MenuItem();
            this._cMnuSelAli = new MenuItem();
            this._cMnuSelLeft = new MenuItem();
            this._cMnuSelRight = new MenuItem();
            this._cMnuSelTop = new MenuItem();
            this._cMnuSelBottom = new MenuItem();
            this._cMnuSelHor = new MenuItem();
            this._cMnuSelVert = new MenuItem();
            this._cMnuMarAli = new MenuItem();
            this._cMnuMargLeft = new MenuItem();
            this._cMnuMargRight = new MenuItem();
            this._cMnuMargTop = new MenuItem();
            this._cMnuMargBottom = new MenuItem();
            this._cMnuMargHor = new MenuItem();
            this._cMnuMargVert = new MenuItem();
            this._menuItem19 = new MenuItem();
            this._cMnuSelFit = new MenuItem();
            this._cMnuSelWidth = new MenuItem();
            this._cMnuSelHeight = new MenuItem();
            this._cMnuMarFit = new MenuItem();
            this._cMnuMargWidth = new MenuItem();
            this._cMnuMargHeight = new MenuItem();
            this._menuItem4 = new MenuItem();
            this._cMnuDelete = new MenuItem();
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
            this._contextMenuRight.MenuItems.AddRange(new MenuItem[] {
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
            this._cMnuMoveUp.Index = 0;
            this._cMnuMoveUp.Text = MessageStrings.LayoutMenuStripSelectMoveUp;
            this._cMnuMoveUp.Click += new EventHandler(this.CMnuMoveUpClick);
            //
            // _cMnuMoveDown
            //
            this._cMnuMoveDown.Index = 1;
            this._cMnuMoveDown.Text = MessageStrings.LayoutMenuStripSelectMoveDown;
            this._cMnuMoveDown.Click += new EventHandler(this.CMnuMoveDownClick);
            //
            // _menuItem2
            //
            this._menuItem2.Index = 2;
            resources.ApplyResources(this._menuItem2, "_menuItem2");
            //
            // _cMnuSelAli
            //
            this._cMnuSelAli.Index = 3;
            this._cMnuSelAli.MenuItems.AddRange(new MenuItem[] {
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
            this._cMnuSelLeft.Index = 0;
            this._cMnuSelLeft.Text = MessageStrings.LayoutCmnuLeft;
            this._cMnuSelLeft.Click += new EventHandler(this.CMnuSelLeftClick);
            //
            // _cMnuSelRight
            //
            this._cMnuSelRight.Index = 1;
            this._cMnuSelRight.Text = MessageStrings.LayoutCmnuRight;
            this._cMnuSelRight.Click += new EventHandler(this.CMnuSelRightClick);
            //
            // _cMnuSelTop
            //
            this._cMnuSelTop.Index = 2;
            this._cMnuSelTop.Text = MessageStrings.LayoutCmnuTop;
            this._cMnuSelTop.Click += new EventHandler(this.CMnuSelTopClick);
            //
            // _cMnuSelBottom
            //
            this._cMnuSelBottom.Index = 3;
            this._cMnuSelBottom.Text = MessageStrings.LayoutCmnuBottom;
            this._cMnuSelBottom.Click += new EventHandler(this.CMnuSelBottomClick);
            //
            // _cMnuSelHor
            //
            this._cMnuSelHor.Index = 4;
            this._cMnuSelHor.Text = MessageStrings.LayoutCmnuHor;
            this._cMnuSelHor.Click += new EventHandler(this.CMnuSelHorClick);
            //
            // _cMnuSelVert
            //
            this._cMnuSelVert.Index = 5;
            this._cMnuSelVert.Text = MessageStrings.LayoutCmnuVert;
            this._cMnuSelVert.Click += new EventHandler(this.CMnuSelVertClick);
            //
            // _cMnuMarAli
            //
            this._cMnuMarAli.Index = 4;
            this._cMnuMarAli.MenuItems.AddRange(new MenuItem[] {
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
            this._cMnuMargLeft.Index = 0;
            this._cMnuMargLeft.Text = MessageStrings.LayoutCmnuLeft;
            this._cMnuMargLeft.Click += new EventHandler(this.CMnuMargLeftClick);
            //
            // _cMnuMargRight
            //
            this._cMnuMargRight.Index = 1;
            this._cMnuMargRight.Text = MessageStrings.LayoutCmnuRight;
            this._cMnuMargRight.Click += new EventHandler(this.CMnuMargRightClick);
            //
            // _cMnuMargTop
            //
            this._cMnuMargTop.Index = 2;
            this._cMnuMargTop.Text = MessageStrings.LayoutCmnuTop;
            this._cMnuMargTop.Click += new EventHandler(this.CMnuMargTopClick);
            //
            // _cMnuMargBottom
            //
            this._cMnuMargBottom.Index = 3;
            this._cMnuMargBottom.Text = MessageStrings.LayoutCmnuBottom;
            this._cMnuMargBottom.Click += new EventHandler(this.CMnuMargBottomClick);
            //
            // _cMnuMargHor
            //
            this._cMnuMargHor.Index = 4;
            this._cMnuMargHor.Text = MessageStrings.LayoutCmnuHor;
            this._cMnuMargHor.Click += new EventHandler(this.CMnuMargHorClick);
            //
            // _cMnuMargVert
            //
            this._cMnuMargVert.Index = 5;
            this._cMnuMargVert.Text = MessageStrings.LayoutCmnuVert;
            this._cMnuMargVert.Click += new EventHandler(this.CMnuMargVertClick);
            //
            // _menuItem19
            //
            this._menuItem19.Index = 5;
            resources.ApplyResources(this._menuItem19, "_menuItem19");
            //
            // _cMnuSelFit
            //
            this._cMnuSelFit.Index = 6;
            this._cMnuSelFit.MenuItems.AddRange(new MenuItem[] {
                                                                   this._cMnuSelWidth,
                                                                   this._cMnuSelHeight});
            this._cMnuSelFit.Text = MessageStrings.LayoutCmnuSelectionFit;
            //
            // _cMnuSelWidth
            //
            this._cMnuSelWidth.Index = 0;
            this._cMnuSelWidth.Text = MessageStrings.LayoutCmnuWidth;
            this._cMnuSelWidth.Click += new EventHandler(this.CMnuSelWidthClick);
            //
            // _cMnuSelHeight
            //
            this._cMnuSelHeight.Index = 1;
            this._cMnuSelHeight.Text = MessageStrings.LayoutCmnuHeight;
            this._cMnuSelHeight.Click += new EventHandler(this.CMnuSelHeightClick);
            //
            // _cMnuMarFit
            //
            this._cMnuMarFit.Index = 7;
            this._cMnuMarFit.MenuItems.AddRange(new MenuItem[] {
                                                                   this._cMnuMargWidth,
                                                                   this._cMnuMargHeight});
            this._cMnuMarFit.Text = MessageStrings.LayoutCmnuMarginFit;
            //
            // _cMnuMargWidth
            //
            this._cMnuMargWidth.Index = 0;
            this._cMnuMargWidth.Text = MessageStrings.LayoutCmnuWidth;
            this._cMnuMargWidth.Click += new EventHandler(this.CMnuMargWidthClick);
            //
            // _cMnuMargHeight
            //
            this._cMnuMargHeight.Index = 1;
            this._cMnuMargHeight.Text = MessageStrings.LayoutCmnuHeight;
            this._cMnuMargHeight.Click += new EventHandler(this.CMnuMargHeightClick);
            //
            // _menuItem4
            //
            this._menuItem4.Index = 8;
            resources.ApplyResources(this._menuItem4, "_menuItem4");
            //
            // _cMnuDelete
            //
            this._cMnuDelete.Index = 9;
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

        private MenuItem _cMnuDelete;
        private MenuItem _cMnuMarAli;
        private MenuItem _cMnuMarFit;
        private MenuItem _cMnuMargBottom;
        private MenuItem _cMnuMargHeight;
        private MenuItem _cMnuMargHor;
        private MenuItem _cMnuMargLeft;
        private MenuItem _cMnuMargRight;
        private MenuItem _cMnuMargTop;
        private MenuItem _cMnuMargVert;
        private MenuItem _cMnuMargWidth;
        private MenuItem _cMnuMoveDown;
        private MenuItem _cMnuMoveUp;
        private MenuItem _cMnuSelAli;
        private MenuItem _cMnuSelBottom;
        private MenuItem _cMnuSelFit;
        private MenuItem _cMnuSelHeight;
        private MenuItem _cMnuSelHor;
        private MenuItem _cMnuSelLeft;
        private MenuItem _cMnuSelRight;
        private MenuItem _cMnuSelTop;
        private MenuItem _cMnuSelVert;
        private MenuItem _cMnuSelWidth;
        private ContextMenu _contextMenuRight;
        private HScrollBar _hScrollBar;
        private Panel _hScrollBarPanel;
        private LayoutDocToolStrip _layoutDocToolStrip;
        private LayoutInsertToolStrip _layoutInsertToolStrip;
        private LayoutListBox _layoutListBox;
        private LayoutMapToolStrip _layoutMapToolStrip;
        private LayoutMenuStrip _layoutMenuStrip;
        private LayoutPropertyGrid _layoutPropertyGrip;
        private LayoutZoomToolStrip _layoutZoomToolStrip;
        private MenuItem _menuItem19;
        private MenuItem _menuItem2;
        private MenuItem _menuItem4;
        private VScrollBar _vScrollBar;
    }
}
