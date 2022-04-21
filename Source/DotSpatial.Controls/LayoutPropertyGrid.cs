// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This is a control that allows users to easilly modify the various aspects of many different layout components.
    /// </summary>
    // This control will no longer be visible
    [ToolboxItem(false)]
    public partial class LayoutPropertyGrid : UserControl
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutPropertyGrid"/> class.
        /// </summary>
        public LayoutPropertyGrid()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the layout control associated with this property grid.
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
                _layoutControl = value;
                if (_layoutControl == null) return;
                _layoutControl.SelectionChanged += LayoutControlSelectionChanged;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// If the selection changes this event is called.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void LayoutControlSelectionChanged(object sender, EventArgs e)
        {
            // This code is so that the property grid gets updates if one of the properties changes
            foreach (LayoutElement selecteElement in _layoutControl.LayoutElements)
                selecteElement.Invalidated -= SelecteElementInvalidated;
            foreach (LayoutElement selecteElement in _layoutControl.SelectedLayoutElements)
                selecteElement.Invalidated += SelecteElementInvalidated;

            // If there is no selection get the layoutControls properties otherwise show the selected elements properties
            _propertyGrid.SelectedObjects = _layoutControl.SelectedLayoutElements.Count > 0 ? _layoutControl.SelectedLayoutElements.ToArray() : null;
        }

        private void LayoutPropertyGridKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    _layoutControl.DeleteSelected();
                    break;
                case Keys.F5:
                    _layoutControl.RefreshElements();
                    break;
            }
        }

        private void SelecteElementInvalidated(object sender, EventArgs e)
        {
            // If there is no selection get the layoutControls properties otherwise show the selected elements properties
            _propertyGrid.Refresh();
        }

        #endregion
    }
}