// ********************************************************************************************************
// Product Name: DotSpatial.Layout.LayoutMenyStrip
// Description:  A menu strip designed to work along with the layout control
//
// ********************************************************************************************************
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
    /// A menu strip for the layout window.
    /// </summary>
    // This control will no longer be visible
    [ToolboxItem(false)]
    public partial class LayoutMenuStrip : MenuStrip
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutMenuStrip"/> class.
        /// </summary>
        public LayoutMenuStrip()
        {
            InitializeComponent();
        }

        #endregion

        #region Events

        /// <summary>
        /// Fires when the user clicks the close button on this menu strip
        /// </summary>
        public event EventHandler CloseClicked;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the layout control associated with this toolstrip.
        /// </summary>
        [Browsable(false)]
        public LayoutControl LayoutControl
        {
            get
            {
                return _layoutControl;
            }

            set
            {
                if (value == null) return;
                _layoutControl = value;

                // Sets the Margin tool to checked depending on the property of the layoutcontrol
                if (_layoutControl.LayoutZoomToolStrip != null)
                {
                    _showMargin.Checked = _layoutControl.ShowMargin;
                }

                // Sets the layoutzoom striptoolbar button to checked if its visible
                if (_layoutControl.LayoutZoomToolStrip != null)
                {
                    _zoomToolbar.Checked = _layoutControl.LayoutZoomToolStrip.Visible;
                }

                // Sets the layout doc toolbar button to checked if its visible
                if (_layoutControl.LayoutDocToolStrip != null)
                {
                    _docToolbar.Checked = _layoutControl.LayoutDocToolStrip.Visible;
                }

                _layoutControl.SelectionChanged += LayoutControlSelectionChanged;
            }
        }

        #endregion

        #region Methods

        private void CloseClick(object sender, EventArgs e)
        {
            CloseClicked?.Invoke(this, null);
        }

        private void DocToolbarCheckedChanged(object sender, EventArgs e)
        {
            _layoutControl.LayoutDocToolStrip.Visible = _docToolbar.Checked;
        }

        private void InsertToolbarCheckedChanged(object sender, EventArgs e)
        {
            _layoutControl.LayoutInsertToolStrip.Visible = _insertToolbar.Checked;
        }

        private void MapToolbarCheckedChanged(object sender, EventArgs e)
        {
            _layoutControl.LayoutMapToolStrip.Visible = _mapToolbar.Checked;
        }

        private void NewClick(object sender, EventArgs e)
        {
            _layoutControl.NewLayout(true);
        }

        private void OpenClick(object sender, EventArgs e)
        {
            _layoutControl.LoadLayout(true, true, true);
        }

        private void PageSetupClick(object sender, EventArgs e)
        {
            _layoutControl.ShowPageSetupDialog();
        }

        private void PrintClick(object sender, EventArgs e)
        {
            _layoutControl.Print();
        }

        private void RefreshClick(object sender, EventArgs e)
        {
            _layoutControl.RefreshElements();
        }

        private void SaveClick(object sender, EventArgs e)
        {
            _layoutControl.SaveLayout(false);
        }

        private void SaveAsClick(object sender, EventArgs e)
        {
            _layoutControl.SaveLayout(true);
        }

        private void SelectAllClick(object sender, EventArgs e)
        {
            _layoutControl.SelectAll();
        }

        private void SelectConvertClick(object sender, EventArgs e)
        {
            _layoutControl.ConvertSelectedToBitmap();
        }

        private void SelectDeleteClick(object sender, EventArgs e)
        {
            _layoutControl.DeleteSelected();
        }

        private void SelectInvertClick(object sender, EventArgs e)
        {
            _layoutControl.InvertSelection();
        }

        private void SelectMoveDownClick(object sender, EventArgs e)
        {
            _layoutControl.MoveSelectionDown();
        }

        private void SelectMoveUpClick(object sender, EventArgs e)
        {
            _layoutControl.MoveSelectionUp();
        }

        private void SelectNoneClick(object sender, EventArgs e)
        {
            _layoutControl.ClearSelection();
        }

        private void SelectPrinterClick(object sender, EventArgs e)
        {
            _layoutControl.ShowChoosePrinterDialog();
        }

        private void ShowMarginCheckedChanged(object sender, EventArgs e)
        {
            _layoutControl.ShowMargin = _showMargin.Checked;
        }

        private void ZoomFitClick(object sender, EventArgs e)
        {
            _layoutControl.ZoomFitToScreen();
        }

        private void ZoomInClick(object sender, EventArgs e)
        {
            _layoutControl.ZoomIn();
        }

        private void ZoomOutClick(object sender, EventArgs e)
        {
            _layoutControl.ZoomOut();
        }

        private void ZoomToolbarCheckedChanged(object sender, EventArgs e)
        {
            _layoutControl.LayoutZoomToolStrip.Visible = _zoomToolbar.Checked;
        }

        // Enables and disables the convert selected to bitmap option if the selection changes above or below 1
        private void LayoutControlSelectionChanged(object sender, EventArgs e)
        {
            _selectConvert.Enabled = _layoutControl.SelectedLayoutElements.Count == 1;
        }

        #endregion
    }
}