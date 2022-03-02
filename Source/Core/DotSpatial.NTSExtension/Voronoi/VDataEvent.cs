// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.NTSExtension.Voronoi
{
    /// <summary>
    /// The VDataEvent.
    /// </summary>
    internal class VDataEvent : VEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VDataEvent"/> class.
        /// </summary>
        /// <param name="dp">Vector used as DataPoint.</param>
        public VDataEvent(Vector2 dp)
        {
            DataPoint = dp;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the DataPoint of this event.
        /// </summary>
        public Vector2 DataPoint { get; }

        /// <summary>
        /// Gets the Y value.
        /// </summary>
        public override double Y => DataPoint.Y;

        /// <summary>
        /// Gets the X value.
        /// </summary>
        protected override double X => DataPoint.X;

        #endregion
    }
}