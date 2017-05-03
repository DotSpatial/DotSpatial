// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is DotSpatial.dll for the DotSpatial project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// The easiest way to implement this is to inherit a UserControl, and then
    /// implement the members specific to the legend.
    /// </summary>
    public interface ILegend
    {
        #region Events

        /// <summary>
        /// Occurs when the drag method is used to alter the order of layers or
        /// groups in the legend.
        /// </summary>
        event EventHandler OrderChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the list of map frames being displayed by this legend.
        /// </summary>
        List<ILegendItem> RootNodes { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a map frame as a root node, and links an event handler to update
        /// when the mapframe triggers an ItemChanged event.
        /// </summary>
        /// <param name="mapFrame">MapFrame that gets added.</param>
        void AddMapFrame(IFrame mapFrame);

        /// <summary>
        /// Given the current list of Maps or 3DMaps, it
        /// rebuilds the treeview nodes
        /// </summary>
        void RefreshNodes();

        /// <summary>
        /// Removes the specified map frame if it is a root node.
        /// </summary>
        /// <param name="mapFrame">MapFrame that gets removed.</param>
        /// <param name="preventRefresh">Boolean, if true, removing the map frame will not automatically force a refresh of the legend.</param>
        void RemoveMapFrame(IFrame mapFrame, bool preventRefresh);

        #endregion
    }
}