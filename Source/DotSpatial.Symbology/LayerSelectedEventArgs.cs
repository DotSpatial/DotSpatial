// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// LayerSelectedEventArgs
    /// </summary>
    public class LayerSelectedEventArgs : LayerEventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayerSelectedEventArgs"/> class.
        /// </summary>
        /// <param name="layer">The layer of the event.</param>
        /// <param name="selected">Indicates whether the layer is selected.</param>
        public LayerSelectedEventArgs(ILayer layer, bool selected)
            : base(layer)
        {
            IsSelected = selected;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether or not the layer is selected.
        /// </summary>
        public bool IsSelected { get; protected set; }

        #endregion
    }
}