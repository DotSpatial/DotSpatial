// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.NTSExtension.Voronoi
{
    /// <summary>
    /// VDataNode
    /// </summary>
    internal class VDataNode : VNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VDataNode"/> class.
        /// </summary>
        /// <param name="dp">Vector used as data point.</param>
        public VDataNode(Vector2 dp)
        {
            DataPoint = dp;
        }

        #region Properties

        /// <summary>
        /// Gets the data point of this node.
        /// </summary>
        public Vector2 DataPoint { get; }

        #endregion
    }
}