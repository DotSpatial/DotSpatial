// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

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
        #region Fields

        private ToolStripButton _btnNewModel;
        private bool _enableFind;
        private ToolStripTextBox _txtBoxSearch;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolManagerToolStrip"/> class.
        /// </summary>
        public ToolManagerToolStrip()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the ToolManager currently associated with the toolstrip
        /// </summary>
        public ToolManager ToolManager { get; set; }

        #endregion

        #region Methods

        // Fires when the user clicks the new model tool
        private void BtnNewModelClick(object sender, EventArgs e)
        {
            ModelerForm aModelerFormForm = new ModelerForm();
            aModelerFormForm.Modeler.ToolManager = ToolManager;
            aModelerFormForm.Modeler.CreateNewModel(false);
            aModelerFormForm.Show(this);
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            _btnNewModel = new ToolStripButton
            {
                ToolTipText = MessageStrings.NewModel,
                Image = Images.NewModel.ToBitmap()
            };
            _btnNewModel.Click += BtnNewModelClick;
            Items.Add(_btnNewModel);

            _txtBoxSearch = new ToolStripTextBox
            {
                Text = MessageStrings.FindToolByName
            };
            _txtBoxSearch.TextChanged += TxtBoxSearchTextChanged;
            _txtBoxSearch.Leave += TxtBoxSearchLeave;
            _txtBoxSearch.Enter += TxtBoxSearchEnter;
            _txtBoxSearch.KeyPress += TxtBoxSearchKeyPress;
            Items.Add(_txtBoxSearch);

            ResumeLayout();
        }

        // Fires when the user click on the find tool text box
        private void TxtBoxSearchEnter(object sender, EventArgs e)
        {
            _txtBoxSearch.Text = string.Empty;
            _enableFind = true;
        }

        // Fires when the user hits a new, and handles it if its enter
        private void TxtBoxSearchKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                ToolManager.HighlightNextTool(_txtBoxSearch.Text);
        }

        // Fires when the user leaves the find tool text box
        private void TxtBoxSearchLeave(object sender, EventArgs e)
        {
            _enableFind = false;
            _txtBoxSearch.Text = MessageStrings.FindToolByName;
        }

        // Fires when the text is changed in the find tool text box and calls the toolmanager to highlight a relevant tool
        private void TxtBoxSearchTextChanged(object sender, EventArgs e)
        {
            if (_enableFind)
                ToolManager.HighlightTool(_txtBoxSearch.Text);
        }

        #endregion
    }
}