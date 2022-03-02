// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.NTSExtension.Voronoi
{
    /// <summary>
    /// The VCircleEvent.
    /// </summary>
    internal class VCircleEvent : VEvent
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Center.
        /// </summary>
        public Vector2 Center { get; set; }

        /// <summary>
        /// Gets or sets the NodeL.
        /// </summary>
        public VDataNode NodeL { get; set; }

        /// <summary>
        /// Gets or sets the NodeN.
        /// </summary>
        public VDataNode NodeN { get; set; }

        /// <summary>
        /// Gets or sets the NodeR.
        /// </summary>
        public VDataNode NodeR { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the event is valid.
        /// </summary>
        public bool Valid { get; set; } = true;

        /// <summary>
        /// Gets the Y value.
        /// </summary>
        public override double Y => Center.Y + MathTools.Dist(NodeN.DataPoint.X, NodeN.DataPoint.Y, Center.X, Center.Y);

        /// <summary>
        /// Gets the X value.
        /// </summary>
        protected override double X => Center.X;

        #endregion
    }
}