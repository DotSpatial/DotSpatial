// ********************************************************************************************************
// Product Name: DotSpatial.Tools.ToolManagerToolStrip
// Description:  A tool strip designed to work along with the tool manager
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
// The Original Code is Toolbox.dll for the DotSpatial 4.6/6 ToolManager project
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Apr, 2009.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A Tool strip to use with the ToolManager when used as a graphical control
    /// </summary>
    [ToolboxItem(false)]
    public class ToolManagerToolStrip : ToolStrip
    {
        #region "Private Variables"

        private ToolStripButton _btnNewModel;
        private bool _enableFind;
        private ToolManager _toolManager;
        private ToolStripTextBox _txtBoxSearch;

        #endregion

        #region "Constructor"

        /// <summary>
        /// Creates an instance of the toolstrip
        /// </summary>
        public ToolManagerToolStrip()
        {
            InitializeComponent();
        }

        #endregion

        #region "properties"

        /// <summary>
        /// Gets or sets the ToolManager currently associated with the toolstrip
        /// </summary>
        public ToolManager ToolManager
        {
            get { return _toolManager; }
            set { _toolManager = value; }
        }

        #endregion

        private void InitializeComponent()
        {
            SuspendLayout();

            _btnNewModel = new ToolStripButton();
            _btnNewModel.ToolTipText = MessageStrings.NewModel;
            _btnNewModel.Image = Images.NewModel.ToBitmap();
            _btnNewModel.Click += BtnNewModelClick;
            Items.Add(_btnNewModel);

            _txtBoxSearch = new ToolStripTextBox();
            _txtBoxSearch.Text = MessageStrings.FindToolByName;
            _txtBoxSearch.TextChanged += TxtBoxSearchTextChanged;
            _txtBoxSearch.Leave += TxtBoxSearchLeave;
            _txtBoxSearch.Enter += TxtBoxSearchEnter;
            _txtBoxSearch.KeyPress += TxtBoxSearchKeyPress;
            Items.Add(_txtBoxSearch);

            ResumeLayout();
        }

        #region "Envent Handlers"

        // Fires when the user click on the find tool text box
        private void TxtBoxSearchEnter(object sender, EventArgs e)
        {
            _txtBoxSearch.Text = string.Empty;
            _enableFind = true;
        }

        // Fires when the user leaves the find tool text box
        private void TxtBoxSearchLeave(object sender, EventArgs e)
        {
            _enableFind = false;
            _txtBoxSearch.Text = MessageStrings.FindToolByName;
        }

        //Fires when the text is changed in the find tool text box and calls the toolmanager to highlight a relevant tool
        private void TxtBoxSearchTextChanged(object sender, EventArgs e)
        {
            if (_enableFind)
                _toolManager.HighlightTool(_txtBoxSearch.Text);
        }

        //Fires when the user hits a new, and handles it if its enter
        private void TxtBoxSearchKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                _toolManager.HighlightNextTool(_txtBoxSearch.Text);
        }

        //Fires when the user clicks the new model tool
        private void BtnNewModelClick(object sender, EventArgs e)
        {
            ModelerForm aModelerFormForm = new ModelerForm();
            aModelerFormForm.Modeler.ToolManager = _toolManager;
            aModelerFormForm.Modeler.CreateNewModel(false);
            aModelerFormForm.Show(this);
        }

        #endregion
    }
}