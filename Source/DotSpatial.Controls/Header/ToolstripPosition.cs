// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Controls.Header
{
    /// <summary>
    /// Helper class to remember the toolstrip positions to save to file on exit.
    /// </summary>
    [Serializable]
    internal class ToolstripPosition
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolstripPosition"/> class.
        /// </summary>
        /// <param name="name">Name of the toolstrip.</param>
        /// <param name="row">The row of the toolstrip position.</param>
        /// <param name="column">The column of the toolstrip position.</param>
        public ToolstripPosition(string name, int row, int column)
        {
            Name = name;
            Row = row;
            Column = column;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the column of the toolstrip position.
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// Gets the name of the toolstrip.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the row of the toolstrip position.
        /// </summary>
        public int Row { get; set; }

        #endregion
    }
}