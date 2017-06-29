// ********************************************************************************************************
// Product Name: DotSpatial.Layout.LayoutMenyStrip
// Description:  A menu strip designed to work along with the layout control
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll Version 6.0
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Jul, 2009.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// Name              |   Date     | Comments
// ------------------|------------|---------------------------------------------------------------
// Ted Dunsford      | 8/28/2009  | Cleaned up some code formatting using resharper
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    ///
    /// </summary>
    [ToolboxItem(true)]
    public class LayoutMenuStrip : MenuStrip
    {
        #region "Private Variables"

        private ToolStripMenuItem _close;
        private ToolStripMenuItem _docToolbar;
        private ToolStripMenuItem _fileMenu;
        private ToolStripMenuItem _insertToolbar;
        private LayoutControl _layoutControl;
        private ToolStripMenuItem _mapToolbar;
        private ToolStripMenuItem _new;
        private ToolStripMenuItem _open;
        private ToolStripMenuItem _pageSetup;
        private ToolStripMenuItem _print;
        private ToolStripMenuItem _refresh;
        private ToolStripMenuItem _save;
        private ToolStripMenuItem _saveAs;
        private ToolStripMenuItem _selectAll;
        private ToolStripMenuItem _selectConvert;
        private ToolStripMenuItem _selectDelete;
        private ToolStripMenuItem _selectInvert;
        private ToolStripMenuItem _selectMenu;
        private ToolStripMenuItem _selectMoveDown;
        private ToolStripMenuItem _selectMoveUp;
        private ToolStripMenuItem _selectNone;
        private ToolStripMenuItem _selectPrinter;
        private ToolStripMenuItem _showMargin;
        private ToolStripMenuItem _toolbars;

        private ToolStripMenuItem _viewMenu;
        private ToolStripMenuItem _zoomFit;
        private ToolStripMenuItem _zoomIn;
        private ToolStripMenuItem _zoomOut;
        private ToolStripMenuItem _zoomToolbar;

        /// <summary>
        /// Fires when the user clicks the close button on this menu strip
        /// </summary>
        public event EventHandler CloseClicked;

        #endregion

        #region "Constructor"

        /// <summary>
        /// Creates an instance of the toolstrip
        /// </summary>
        public LayoutMenuStrip()
        {
            InitializeComponent();
        }

        #endregion

        #region "properties"

        /// <summary>
        /// The layout control associated with this toolstrip
        /// </summary>
        [Browsable(false)]
        public LayoutControl LayoutControl
        {
            get { return _layoutControl; }
            set
            {
                if (value == null) return;
                _layoutControl = value;

                //Sets the Margin tool to checked depending on the property of the layoutcontrol
                if (_layoutControl.LayoutZoomToolStrip != null)
                {
                    if (_layoutControl.ShowMargin)
                        _showMargin.Checked = true;
                    else
                        _showMargin.Checked = false;
                }

                //Sets the layoutzoom striptoolbar button to checked if its visible
                if (_layoutControl.LayoutZoomToolStrip != null)
                {
                    if (_layoutControl.LayoutZoomToolStrip.Visible)
                        _zoomToolbar.Checked = true;
                    else
                        _zoomToolbar.Checked = false;
                }

                //Sets the layout doc toolbar button to checked if its visible
                if (_layoutControl.LayoutDocToolStrip != null)
                {
                    if (_layoutControl.LayoutDocToolStrip.Visible)
                        _docToolbar.Checked = true;
                    else
                        _docToolbar.Checked = false;
                }

                _layoutControl.SelectionChanged += LayoutControlSelectionChanged;
            }
        }

        #endregion

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.SuspendLayout();

            //File Menu
            _fileMenu = new ToolStripMenuItem(MessageStrings.LayoutMenuStripFile);
            _new = new ToolStripMenuItem(MessageStrings.LayoutMenuStripNew);
            _new.Image = Images.file_new;
            _new.Click += new EventHandler(_New_Click);
            _fileMenu.DropDownItems.Add(_new);
            _fileMenu.DropDownItems.Add(new ToolStripSeparator());
            _open = new ToolStripMenuItem(MessageStrings.LayoutMenuStripOpen);
            _open.Image = Images.FolderOpen;
            _open.Click += new EventHandler(_Open_Click);
            _fileMenu.DropDownItems.Add(_open);
            _fileMenu.DropDownItems.Add(new ToolStripSeparator());
            _save = new ToolStripMenuItem(MessageStrings.LayoutMenuStripSave);
            _save.Image = Images.save.ToBitmap();
            _save.Click += new EventHandler(_Save_Click);
            _fileMenu.DropDownItems.Add(_save);
            _saveAs = new ToolStripMenuItem(MessageStrings.LayoutMenuStripSaveAs);
            _saveAs.Image = Images.file_saveas;
            _saveAs.Click += new EventHandler(_SaveAs_Click);
            _fileMenu.DropDownItems.Add(_saveAs);
            _fileMenu.DropDownItems.Add(new ToolStripSeparator());
            _selectPrinter = new ToolStripMenuItem(MessageStrings.LayoutMenuStripSelectPrinter);
            _selectPrinter.Click += new EventHandler(_SelectPrinter_Click);
            _fileMenu.DropDownItems.Add(_selectPrinter);
            _pageSetup = new ToolStripMenuItem(MessageStrings.LayoutMenuPageSetup);
            _pageSetup.Image = Images.PageSetup.ToBitmap();
            _pageSetup.Click += new EventHandler(_PageSetup_Click);
            _fileMenu.DropDownItems.Add(_pageSetup);
            _print = new ToolStripMenuItem(MessageStrings.LayoutMenuStripPrint);
            _print.Image = Images.printer;
            _print.Click += new EventHandler(_Print_Click);
            _fileMenu.DropDownItems.Add(_print);
            _fileMenu.DropDownItems.Add(new ToolStripSeparator());
            _close = new ToolStripMenuItem(MessageStrings.LayoutMenuStripClose);
            _close.Click += new EventHandler(_Close_Click);
            _close.Image = Images.file_close;
            _fileMenu.DropDownItems.Add(_close);
            this.Items.Add(_fileMenu);

            //The Select Menu
            _selectMenu = new ToolStripMenuItem(MessageStrings.LayoutMenuStripSelect);
            _selectAll = new ToolStripMenuItem(MessageStrings.LayoutMenuStripSelectAll);
            _selectAll.Image = Images.select_all;
            _selectAll.Click += new EventHandler(_SelectAll_Click);
            _selectMenu.DropDownItems.Add(_selectAll);
            _selectNone = new ToolStripMenuItem(MessageStrings.LayoutMenuStripSelectNone);
            _selectNone.Image = Images.select_none;
            _selectNone.Click += new EventHandler(_SelectNone_Click);
            _selectMenu.DropDownItems.Add(_selectNone);
            _selectInvert = new ToolStripMenuItem(MessageStrings.LayoutMenuStripSelectInvert);
            _selectInvert.Click += new EventHandler(_SelectInvert_Click);
            _selectMenu.DropDownItems.Add(_selectInvert);
            _selectMenu.DropDownItems.Add(new ToolStripSeparator());
            _selectMoveUp = new ToolStripMenuItem(MessageStrings.LayoutMenuStripSelectMoveUp);
            _selectMoveUp.Image = Images.up;
            _selectMoveUp.Click += new EventHandler(_SelectMoveUp_Click);
            _selectMenu.DropDownItems.Add(_selectMoveUp);
            _selectMoveDown = new ToolStripMenuItem(MessageStrings.LayoutMenuStripSelectMoveDown);
            _selectMoveDown.Image = Images.down;
            _selectMoveDown.Click += new EventHandler(_SelectMoveDown_Click);
            _selectMenu.DropDownItems.Add(_selectMoveDown);
            _selectMenu.DropDownItems.Add(new ToolStripSeparator());
            _selectDelete = new ToolStripMenuItem(MessageStrings.LayoutMenuStripSelectDelete);
            _selectDelete.Image = Images.mnuLayerClear;
            _selectDelete.Click += new EventHandler(_SelectDelete_Click);
            _selectMenu.DropDownItems.Add(_selectDelete);
            _selectMenu.DropDownItems.Add(new ToolStripSeparator());
            _selectConvert = new ToolStripMenuItem(MessageStrings.LayoutMenuStripSelectConvert);
            _selectConvert.Click += new EventHandler(_SelectConvert_Click);
            _selectConvert.Enabled = false;
            _selectMenu.DropDownItems.Add(_selectConvert);
            this.Items.Add(_selectMenu);

            //View Menu
            _viewMenu = new ToolStripMenuItem(MessageStrings.LayoutMenuStripView);

            _zoomIn = new ToolStripMenuItem(MessageStrings.LayoutMenuStripZoomIn);
            _zoomIn.Image = Images.layout_zoom_in.ToBitmap();
            _zoomIn.Click += new EventHandler(_ZoomIn_Click);
            _viewMenu.DropDownItems.Add(_zoomIn);

            _zoomOut = new ToolStripMenuItem(MessageStrings.LayoutMenuStripZoomOut);
            _zoomOut.Image = Images.layout_zoom_out.ToBitmap();
            _zoomOut.Click += new EventHandler(_ZoomOut_Click);
            _viewMenu.DropDownItems.Add(_zoomOut);

            _zoomFit = new ToolStripMenuItem(MessageStrings.LayoutMenuStripZoomFull);
            _zoomFit.Image = Images.layout_zoom_full_extent.ToBitmap();
            _zoomFit.Click += new EventHandler(_ZoomFit_Click);
            _viewMenu.DropDownItems.Add(_zoomFit);

            _viewMenu.DropDownItems.Add(new ToolStripSeparator());

            _showMargin = new ToolStripMenuItem(MessageStrings.LayoutMenuStripShowMargin);
            _showMargin.CheckOnClick = true;
            _showMargin.CheckedChanged += new EventHandler(_showMargin_CheckedChanged);
            _viewMenu.DropDownItems.Add(_showMargin);

            _viewMenu.DropDownItems.Add(new ToolStripSeparator());

            _toolbars = new ToolStripMenuItem(MessageStrings.LayoutMenuStripToolbars);
            _viewMenu.DropDownItems.Add(_toolbars);

            _docToolbar = new ToolStripMenuItem(MessageStrings.LayoutMenuStripDocToolbar);
            _docToolbar.Checked = true;
            _docToolbar.CheckOnClick = true;
            _docToolbar.CheckedChanged += new EventHandler(_DocToolbar_CheckedChanged);
            _toolbars.DropDownItems.Add(_docToolbar);

            _zoomToolbar = new ToolStripMenuItem(MessageStrings.LayoutMenuStripZoomToolbar);
            _zoomToolbar.Checked = true;
            _zoomToolbar.CheckOnClick = true;
            _zoomToolbar.CheckedChanged += new EventHandler(_ZoomToolbar_CheckedChanged);
            _toolbars.DropDownItems.Add(_zoomToolbar);

            _mapToolbar = new ToolStripMenuItem(MessageStrings.LayoutMenuStripMapToolbar);
            _mapToolbar.Checked = true;
            _mapToolbar.CheckOnClick = true;
            _mapToolbar.CheckedChanged += new EventHandler(_MapToolbar_CheckedChanged);
            _toolbars.DropDownItems.Add(_mapToolbar);

            _insertToolbar = new ToolStripMenuItem(MessageStrings.LayoutMenuStripInsertToolbar);
            _insertToolbar.Checked = true;
            _insertToolbar.CheckOnClick = true;
            _insertToolbar.CheckedChanged += new EventHandler(_InsertToolbar_CheckedChanged);
            _toolbars.DropDownItems.Add(_insertToolbar);

            _viewMenu.DropDownItems.Add(new ToolStripSeparator());

            _refresh = new ToolStripMenuItem(MessageStrings.LayoutMenuStripRefresh);
            _refresh.Click += new EventHandler(_Refresh_Click);
            _viewMenu.DropDownItems.Add(_refresh);

            this.Items.Add(_viewMenu);

            this.ResumeLayout();
        }

        #endregion

        #region ---------------------- File Menu Events

        private void _New_Click(object sender, EventArgs e)
        {
            _layoutControl.NewLayout(true);
        }

        private void _Open_Click(object sender, EventArgs e)
        {
            _layoutControl.LoadLayout(true, true, true);
        }

        private void _Save_Click(object sender, EventArgs e)
        {
            _layoutControl.SaveLayout(false);
        }

        private void _SaveAs_Click(object sender, EventArgs e)
        {
            _layoutControl.SaveLayout(true);
        }

        private void _SelectPrinter_Click(object sender, EventArgs e)
        {
            _layoutControl.ShowChoosePrinterDialog();
        }

        private void _PageSetup_Click(object sender, EventArgs e)
        {
            _layoutControl.ShowPageSetupDialog();
        }

        private void _Print_Click(object sender, EventArgs e)
        {
            _layoutControl.Print();
        }

        private void _Close_Click(object sender, EventArgs e)
        {
            if (CloseClicked != null)
                CloseClicked(this, null);
        }

        #endregion

        #region ---------------------- Selection Menu Events

        private void _SelectConvert_Click(object sender, EventArgs e)
        {
            _layoutControl.ConvertSelectedToBitmap();
        }

        private void _SelectDelete_Click(object sender, EventArgs e)
        {
            _layoutControl.DeleteSelected();
        }

        private void _SelectMoveDown_Click(object sender, EventArgs e)
        {
            _layoutControl.MoveSelectionDown();
        }

        private void _SelectMoveUp_Click(object sender, EventArgs e)
        {
            _layoutControl.MoveSelectionUp();
        }

        private void _SelectInvert_Click(object sender, EventArgs e)
        {
            _layoutControl.InvertSelection();
        }

        private void _SelectNone_Click(object sender, EventArgs e)
        {
            _layoutControl.ClearSelection();
        }

        private void _SelectAll_Click(object sender, EventArgs e)
        {
            _layoutControl.SelectAll();
        }

        #endregion

        #region ---------------------- View Menu Events

        private void _Refresh_Click(object sender, EventArgs e)
        {
            _layoutControl.RefreshElements();
        }

        private void _InsertToolbar_CheckedChanged(object sender, EventArgs e)
        {
            if (_insertToolbar.Checked)
                _layoutControl.LayoutInsertToolStrip.Visible = true;
            else
                _layoutControl.LayoutInsertToolStrip.Visible = false;
        }

        private void _MapToolbar_CheckedChanged(object sender, EventArgs e)
        {
            if (_mapToolbar.Checked)
                _layoutControl.LayoutMapToolStrip.Visible = true;
            else
                _layoutControl.LayoutMapToolStrip.Visible = false;
        }

        private void _showMargin_CheckedChanged(object sender, EventArgs e)
        {
            if (_showMargin.Checked)
                _layoutControl.ShowMargin = true;
            else
                _layoutControl.ShowMargin = false;
        }

        private void _ZoomToolbar_CheckedChanged(object sender, EventArgs e)
        {
            if (_zoomToolbar.Checked)
                _layoutControl.LayoutZoomToolStrip.Visible = true;
            else
                _layoutControl.LayoutZoomToolStrip.Visible = false;
        }

        private void _DocToolbar_CheckedChanged(object sender, EventArgs e)
        {
            if (_docToolbar.Checked)
                _layoutControl.LayoutDocToolStrip.Visible = true;
            else
                _layoutControl.LayoutDocToolStrip.Visible = false;
        }

        private void _ZoomIn_Click(object sender, EventArgs e)
        {
            _layoutControl.ZoomIn();
        }

        private void _ZoomOut_Click(object sender, EventArgs e)
        {
            _layoutControl.ZoomOut();
        }

        private void _ZoomFit_Click(object sender, EventArgs e)
        {
            _layoutControl.ZoomFitToScreen();
        }

        #endregion

        //Enables and disables the convert selected to bitmap option if the selection changes above or below 1
        private void LayoutControlSelectionChanged(object sender, EventArgs e)
        {
            if (_layoutControl.SelectedLayoutElements.Count == 1)
                _selectConvert.Enabled = true;
            else
                _selectConvert.Enabled = false;
        }
    }
}