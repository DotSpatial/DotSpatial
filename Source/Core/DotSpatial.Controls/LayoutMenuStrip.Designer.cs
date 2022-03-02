using System;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    public partial class LayoutMenuStrip
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            this.SuspendLayout();

            // File Menu
            _fileMenu = new ToolStripMenuItem(MessageStrings.LayoutMenuStripFile);
            _new = new ToolStripMenuItem(MessageStrings.LayoutMenuStripNew);
            _new.Image = Images.file_new;
            _new.Click += new EventHandler(NewClick);
            _fileMenu.DropDownItems.Add(_new);
            _fileMenu.DropDownItems.Add(new ToolStripSeparator());
            _open = new ToolStripMenuItem(MessageStrings.LayoutMenuStripOpen);
            _open.Image = Images.FolderOpen;
            _open.Click += new EventHandler(OpenClick);
            _fileMenu.DropDownItems.Add(_open);
            _fileMenu.DropDownItems.Add(new ToolStripSeparator());
            _save = new ToolStripMenuItem(MessageStrings.LayoutMenuStripSave);
            _save.Image = Images.save.ToBitmap();
            _save.Click += new EventHandler(SaveClick);
            _fileMenu.DropDownItems.Add(_save);
            _saveAs = new ToolStripMenuItem(MessageStrings.LayoutMenuStripSaveAs);
            _saveAs.Image = Images.file_saveas;
            _saveAs.Click += new EventHandler(SaveAsClick);
            _fileMenu.DropDownItems.Add(_saveAs);
            _fileMenu.DropDownItems.Add(new ToolStripSeparator());
            _selectPrinter = new ToolStripMenuItem(MessageStrings.LayoutMenuStripSelectPrinter);
            _selectPrinter.Click += new EventHandler(SelectPrinterClick);
            _fileMenu.DropDownItems.Add(_selectPrinter);
            _pageSetup = new ToolStripMenuItem(MessageStrings.LayoutMenuPageSetup);
            _pageSetup.Image = Images.PageSetup.ToBitmap();
            _pageSetup.Click += new EventHandler(PageSetupClick);
            _fileMenu.DropDownItems.Add(_pageSetup);
            _print = new ToolStripMenuItem(MessageStrings.LayoutMenuStripPrint);
            _print.Image = Images.printer;
            _print.Click += new EventHandler(PrintClick);
            _fileMenu.DropDownItems.Add(_print);
            _fileMenu.DropDownItems.Add(new ToolStripSeparator());
            _close = new ToolStripMenuItem(MessageStrings.LayoutMenuStripClose);
            _close.Click += new EventHandler(CloseClick);
            _close.Image = Images.file_close;
            _fileMenu.DropDownItems.Add(_close);
            this.Items.Add(_fileMenu);

            // The Select Menu
            _selectMenu = new ToolStripMenuItem(MessageStrings.LayoutMenuStripSelect);
            _selectAll = new ToolStripMenuItem(MessageStrings.LayoutMenuStripSelectAll);
            _selectAll.Image = Images.select_all;
            _selectAll.Click += new EventHandler(SelectAllClick);
            _selectMenu.DropDownItems.Add(_selectAll);
            _selectNone = new ToolStripMenuItem(MessageStrings.LayoutMenuStripSelectNone);
            _selectNone.Image = Images.select_none;
            _selectNone.Click += new EventHandler(SelectNoneClick);
            _selectMenu.DropDownItems.Add(_selectNone);
            _selectInvert = new ToolStripMenuItem(MessageStrings.LayoutMenuStripSelectInvert);
            _selectInvert.Click += new EventHandler(SelectInvertClick);
            _selectMenu.DropDownItems.Add(_selectInvert);
            _selectMenu.DropDownItems.Add(new ToolStripSeparator());
            _selectMoveUp = new ToolStripMenuItem(MessageStrings.LayoutMenuStripSelectMoveUp);
            _selectMoveUp.Image = Images.up;
            _selectMoveUp.Click += new EventHandler(SelectMoveUpClick);
            _selectMenu.DropDownItems.Add(_selectMoveUp);
            _selectMoveDown = new ToolStripMenuItem(MessageStrings.LayoutMenuStripSelectMoveDown);
            _selectMoveDown.Image = Images.down;
            _selectMoveDown.Click += new EventHandler(SelectMoveDownClick);
            _selectMenu.DropDownItems.Add(_selectMoveDown);
            _selectMenu.DropDownItems.Add(new ToolStripSeparator());
            _selectDelete = new ToolStripMenuItem(MessageStrings.LayoutMenuStripSelectDelete);
            _selectDelete.Image = Images.mnuLayerClear;
            _selectDelete.Click += new EventHandler(SelectDeleteClick);
            _selectMenu.DropDownItems.Add(_selectDelete);
            _selectMenu.DropDownItems.Add(new ToolStripSeparator());
            _selectConvert = new ToolStripMenuItem(MessageStrings.LayoutMenuStripSelectConvert);
            _selectConvert.Click += new EventHandler(SelectConvertClick);
            _selectConvert.Enabled = false;
            _selectMenu.DropDownItems.Add(_selectConvert);
            this.Items.Add(_selectMenu);

            // View Menu
            _viewMenu = new ToolStripMenuItem(MessageStrings.LayoutMenuStripView);

            _zoomIn = new ToolStripMenuItem(MessageStrings.LayoutMenuStripZoomIn);
            _zoomIn.Image = Images.layout_zoom_in.ToBitmap();
            _zoomIn.Click += new EventHandler(ZoomInClick);
            _viewMenu.DropDownItems.Add(_zoomIn);

            _zoomOut = new ToolStripMenuItem(MessageStrings.LayoutMenuStripZoomOut);
            _zoomOut.Image = Images.layout_zoom_out.ToBitmap();
            _zoomOut.Click += new EventHandler(ZoomOutClick);
            _viewMenu.DropDownItems.Add(_zoomOut);

            _zoomFit = new ToolStripMenuItem(MessageStrings.LayoutMenuStripZoomFull);
            _zoomFit.Image = Images.layout_zoom_full_extent.ToBitmap();
            _zoomFit.Click += new EventHandler(ZoomFitClick);
            _viewMenu.DropDownItems.Add(_zoomFit);

            _viewMenu.DropDownItems.Add(new ToolStripSeparator());

            _showMargin = new ToolStripMenuItem(MessageStrings.LayoutMenuStripShowMargin);
            _showMargin.CheckOnClick = true;
            _showMargin.CheckedChanged += new EventHandler(ShowMarginCheckedChanged);
            _viewMenu.DropDownItems.Add(_showMargin);

            _viewMenu.DropDownItems.Add(new ToolStripSeparator());

            _toolbars = new ToolStripMenuItem(MessageStrings.LayoutMenuStripToolbars);
            _viewMenu.DropDownItems.Add(_toolbars);

            _docToolbar = new ToolStripMenuItem(MessageStrings.LayoutMenuStripDocToolbar);
            _docToolbar.Checked = true;
            _docToolbar.CheckOnClick = true;
            _docToolbar.CheckedChanged += new EventHandler(DocToolbarCheckedChanged);
            _toolbars.DropDownItems.Add(_docToolbar);

            _zoomToolbar = new ToolStripMenuItem(MessageStrings.LayoutMenuStripZoomToolbar);
            _zoomToolbar.Checked = true;
            _zoomToolbar.CheckOnClick = true;
            _zoomToolbar.CheckedChanged += new EventHandler(ZoomToolbarCheckedChanged);
            _toolbars.DropDownItems.Add(_zoomToolbar);

            _mapToolbar = new ToolStripMenuItem(MessageStrings.LayoutMenuStripMapToolbar);
            _mapToolbar.Checked = true;
            _mapToolbar.CheckOnClick = true;
            _mapToolbar.CheckedChanged += new EventHandler(MapToolbarCheckedChanged);
            _toolbars.DropDownItems.Add(_mapToolbar);

            _insertToolbar = new ToolStripMenuItem(MessageStrings.LayoutMenuStripInsertToolbar);
            _insertToolbar.Checked = true;
            _insertToolbar.CheckOnClick = true;
            _insertToolbar.CheckedChanged += new EventHandler(InsertToolbarCheckedChanged);
            _toolbars.DropDownItems.Add(_insertToolbar);

            _viewMenu.DropDownItems.Add(new ToolStripSeparator());

            _refresh = new ToolStripMenuItem(MessageStrings.LayoutMenuStripRefresh);
            _refresh.Click += new EventHandler(RefreshClick);
            _viewMenu.DropDownItems.Add(_refresh);

            this.Items.Add(_viewMenu);

            this.ResumeLayout();
        }

        #endregion

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
    }
}