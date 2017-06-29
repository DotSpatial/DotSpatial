// ********************************************************************************************************
// Product Name: DotSpatial.Compatibility.dll
// Description:  Supports DotSpatial interfaces organized for a MapWindow 4 plugin context.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 3:55:28 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using DotSpatial.Controls;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// PluginManager for dealing with additional plugins
    /// </summary>
    [ToolboxItem(false)]
    public partial class LegacyPluginManager : Component
    {
        #region Private Variables

        ILegend _legend;
        IBasicMap _map;
        MenuStrip _mapMenuStrip;
        ToolStrip _mapToolStrip;
        bool _pluginMenuIsVisible;
        IBasicMap _previewMap;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for Plugin Manager
        /// </summary>
        public LegacyPluginManager()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor for Plugin Manager
        /// </summary>
        /// <param name="container">A Container</param>
        public LegacyPluginManager(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Legend associated with this plugin manager
        /// </summary>
        public ILegend Legend
        {
            get { return _legend; }
            set { _legend = value; }
        }

        /// <summary>
        /// Gets or sets the Map associated with this plugin manager
        /// </summary>
        public IBasicMap Map
        {
            get
            {
                return _map;
            }
            set
            {
                _map = value;
            }
        }

        /// <summary>
        /// Gets or sets the MapMenuStrip associated with this plugin manager
        /// </summary>
        public MenuStrip MapMenuStrip
        {
            get { return _mapMenuStrip; }
            set
            {
                if (_pluginMenuIsVisible)
                {
                    AddPluginMenu();
                }
                if (_pluginMenuIsVisible == false)
                {
                    RemovePluginMenu();
                }
                _mapMenuStrip = value;
            }
        }

        /// <summary>
        /// Gets or sets the MapToolStrip associated with this plugin manager
        /// </summary>
        public ToolStrip MapToolstrip
        {
            get { return _mapToolStrip; }
            set { _mapToolStrip = value; }
        }

        /// <summary>
        /// Gets or sets the Preview Map associated with this plugin manager
        /// </summary>
        public IBasicMap PreviewMap
        {
            get { return _previewMap; }
            set { _previewMap = value; }
        }

        /// <summary>
        /// Controls whether or not a Plugin menu will be added to the MapMenuStrip
        /// specified by this Plugin Manager
        /// </summary>
        public bool PluginMenuIsVisible
        {
            get { return _pluginMenuIsVisible; }
            set
            {
                if (_pluginMenuIsVisible == false)
                {
                    if (value)
                    {
                        AddPluginMenu();
                    }
                }
                else
                {
                    if (value == false)
                    {
                        RemovePluginMenu();
                    }
                }
                _pluginMenuIsVisible = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks the dlls in the Plugins folder or any subfolder and
        /// adds a new checked menu item for each one that it finds.
        /// This can also be controlled using the PluginMenuIsVisible property.
        /// </summary>
        public void AddPluginMenu()
        {
            if (_mapMenuStrip == null) return;
            if (_pluginMenuIsVisible) return;
            _pluginMenuIsVisible = true;
        }

        /// <summary>
        /// Looks for a menu named Plug-ins and removes it.
        /// Control this through the PluginMenuIsVisible property.
        /// This can also be controlled using the PluginMenuIsVisible property.
        /// </summary>
        public void RemovePluginMenu()
        {
            if (_mapMenuStrip == null) return;
            if (_pluginMenuIsVisible == false) return;
            // The Find method is not supported by Mono 2.0
            // ToolStripItem[] tsList = _mapMenuStrip.Items.Find(MessageStrings.Plugins, false);
            List<ToolStripItem> tsList = new List<ToolStripItem>();
            foreach (ToolStripItem item in _mapMenuStrip.Items)
            {
                if (item.Text == "Apps")
                {
                    tsList.Add(item);
                }
            }
            foreach (ToolStripItem item in tsList)
            {
                _mapMenuStrip.Items.Remove(item);
            }
            _pluginMenuIsVisible = false;
        }

        #endregion
    }
}