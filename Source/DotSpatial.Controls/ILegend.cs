// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
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

        /// <summary>
        /// Gets or sets a value indicating whether the legend is used to determine whether a layer is selectable.
        /// If true, a layer is only selectable if it or a superior object (parental group, mapframe) is selected in legend.
        /// If false, the selectable state of the layers gets either determined by a plugin like SetSelectable or developers handle the selectable state by code.
        /// By default legend is used, but if the SetSelectable plugin gets loaded this is used instead of the legend.
        /// </summary>
        bool UseLegendForSelection { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the legend will show ContextMenu.
        /// </summary>
        bool ShowContextMenu { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the legendItemBoxes can be edited or mooved.
        /// </summary>
        bool EditLegendItemBoxes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the culture to use for resources.
        /// </summary>
        CultureInfo LegendCulture { get; set; }

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