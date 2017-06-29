// ********************************************************************************************************
// Product Name: DotSpatial.Layout.LayoutDocToolStrip
// Description:  A tool strip designed to work along with the layout engine
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
// The Initial Developer of this Original Code is Brian Marchionni. Created in Aug, 2009.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A Brian Marchioni original toolstrip... preloaded with content.
    /// </summary>
    [ToolboxItem(true)]
    public class LayoutDocToolStrip : ToolStrip
    {
        #region "Private Variables"

        private ToolStripButton _btnNew;
        private ToolStripButton _btnOpen;
        private ToolStripButton _btnPrint;
        private ToolStripButton _btnSave;
        private ToolStripButton _btnSaveAs;
        private LayoutControl _layoutControl;

        #endregion

        #region "Constructor"

        /// <summary>
        /// Creates an instance of the toolstrip
        /// </summary>
        public LayoutDocToolStrip()
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
            set { _layoutControl = value; if (_layoutControl == null) return; }
        }

        #endregion

        private void InitializeComponent()
        {
            this._btnNew = new ToolStripButton();
            this._btnOpen = new ToolStripButton();
            this._btnSave = new ToolStripButton();
            this._btnSaveAs = new ToolStripButton();
            this._btnPrint = new ToolStripButton();
            this.SuspendLayout();
            //
            // _btnZoomIn
            //
            this._btnNew.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._btnNew.Image = Images.file_new;
            this._btnNew.Size = new Size(23, 22);
            this._btnNew.Text = MessageStrings.LayoutMenuStripNew;
            this._btnNew.Click += this._btnNew_Click;
            //
            // _btnZoomOut
            //
            this._btnOpen.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._btnOpen.Image = Images.FolderOpen;
            this._btnOpen.Size = new Size(23, 22);
            this._btnOpen.Text = MessageStrings.LayoutMenuStripOpen;
            this._btnOpen.Click += this._btnOpen_Click;
            //
            // _btnZoomFullExtent
            //
            this._btnSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._btnSave.Image = Images.save.ToBitmap();
            this._btnSave.Size = new Size(23, 22);
            this._btnSave.Text = MessageStrings.LayoutMenuStripSave;
            this._btnSave.Click += this._btnSave_Click;
            //
            // _comboZoom
            //
            this._btnSaveAs.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._btnSaveAs.Image = Images.file_saveas;
            this._btnSaveAs.Size = new Size(23, 22);
            this._btnSaveAs.Text = MessageStrings.LayoutMenuStripSaveAs;
            this._btnSaveAs.Click += _btnSaveAs_Click;

            this._btnPrint.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._btnPrint.Image = Images.printer;
            this._btnPrint.Size = new Size(23, 22);
            this._btnPrint.Text = MessageStrings.LayoutMenuStripPrint;
            this._btnPrint.Click += _btnPrint_Click;

            //
            // LayoutToolStrip
            //
            this.Items.AddRange(new ToolStripItem[] {
            this._btnNew,
            this._btnOpen,
            this._btnSave,
            this._btnSaveAs,
            new ToolStripSeparator(),
            this._btnPrint});
            this.ResumeLayout(false);
        }

        #region "Envent Handlers"

        //Fires the print method on the layoutcontrol
        private void _btnPrint_Click(object sender, EventArgs e)
        {
            _layoutControl.Print();
        }

        //Fires the saveas method on the layoutcontrol
        private void _btnSaveAs_Click(object sender, EventArgs e)
        {
            _layoutControl.SaveLayout(true);
        }

        //Fires the save method on the layoutcontrol
        private void _btnSave_Click(object sender, EventArgs e)
        {
            _layoutControl.SaveLayout(false);
        }

        //Fires the new method on the layoutcontrol
        private void _btnNew_Click(object sender, EventArgs e)
        {
            _layoutControl.NewLayout(true);
        }

        //Fires the open method on the layoutcontrol
        private void _btnOpen_Click(object sender, EventArgs e)
        {
            _layoutControl.LoadLayout(true, true, true);
        }

        #endregion
    }
}