﻿// ********************************************************************************************************
// Product Name: DotSpatial.Layout.LayoutPropertyGrid
// Description:  A property grid designed to work along with the layout engine
//
// ********************************************************************************************************
//
// The Original Code is DotSpatial.dll Version 6.0
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Jul, 2009.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// ------------------|------------|---------------------------------------------------------------
// Ted Dunsford      | 8/28/2009  | Cleaned up some code formatting using resharper
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This is a control that allows users to easilly modify the various aspects of many different layout components
    /// </summary>
    //This control will no longer be visible
    [ToolboxItem(false)]
    public class LayoutPropertyGrid : UserControl
    {
        private LayoutControl _layoutControl;
        private PropertyGrid _propertyGrid;

        /// <summary>
        /// Creates a new instance of the Layout Property Grid
        /// </summary>
        public LayoutPropertyGrid()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the layout control associated with this property grid
        /// </summary>
        [Browsable(false)]
        public LayoutControl LayoutControl
        {
            get { return _layoutControl; }
            set
            {
                _layoutControl = value;
                if (_layoutControl == null) return;
                _layoutControl.SelectionChanged += LayoutControlSelectionChanged;
            }
        }

        #region ------------------- Event Handlers

        /// <summary>
        /// If the selection changes this event is called
        /// </summary>
        private void LayoutControlSelectionChanged(object sender, EventArgs e)
        {
            //This code is so that the property grid gets updates if one of the properties changes
            foreach (LayoutElement selecteElement in _layoutControl.LayoutElements)
                selecteElement.Invalidated -= SelecteElementInvalidated;
            foreach (LayoutElement selecteElement in _layoutControl.SelectedLayoutElements)
                selecteElement.Invalidated += SelecteElementInvalidated;

            //If there is no selection get the layoutControls properties otherwise show the selected elements properties
            if (_layoutControl.SelectedLayoutElements.Count > 0)
                _propertyGrid.SelectedObjects = _layoutControl.SelectedLayoutElements.ToArray();
            else
                _propertyGrid.SelectedObjects = null;
        }

        private void SelecteElementInvalidated(object sender, EventArgs e)
        {
            //If there is no selection get the layoutControls properties otherwise show the selected elements properties
            _propertyGrid.Refresh();
        }

        private void LayoutPropertyGrid_KeyUp(object sender, KeyEventArgs e)
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

        #endregion

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(LayoutPropertyGrid));
            this._propertyGrid = new PropertyGrid();
            this.SuspendLayout();
            //
            // _propertyGrid
            //
            resources.ApplyResources(this._propertyGrid, "_propertyGrid");
            this._propertyGrid.Name = "_propertyGrid";
            //
            // LayoutPropertyGrid
            //
            this.Controls.Add(this._propertyGrid);
            this.Name = "LayoutPropertyGrid";
            this.KeyUp += new KeyEventHandler(this.LayoutPropertyGrid_KeyUp);
            this.ResumeLayout(false);
        }

        #endregion
    }
}