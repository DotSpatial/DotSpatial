// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/26/2009 6:17:22 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------
// Jiri Kadlec        | 03/08/2010         |  Added the PanelManager to work with tabs and panels
// Yang Cao           | 05/16/2011         |  Added the IHeaderControl to work with standard toolbar and ribbon control
// Eric Hullinger     | 01/02/2014         |  Made changes so that the progress bar on the splash screen will update properly
// Jacob Gillespie    | 03/31/2014         |  Plugins can now be stored in and loaded from one extensions/plugins directory.
// ********************************************************************************************************

using System;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Provides data for the <see langword='MapChanged'/> event.
    /// </summary>
    public class MapChangedEventArgs : EventArgs
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MapChangedEventArgs"/> class.
        /// </summary>
        /// <param name="oldValue">The old map.</param>
        /// <param name="newValue">The new map.</param>
        public MapChangedEventArgs(IMap oldValue, IMap newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the new map.
        /// </summary>
        public IMap NewValue { get; private set; }

        /// <summary>
        /// Gets the old map.
        /// </summary>
        public IMap OldValue { get; private set; }

        #endregion
    }
}