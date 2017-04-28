using System;
using DotSpatial.Data;

namespace DotSpatial.Plugins.ShapeEditor
{
    /// <summary>
    /// The arguments for the VertexMoved event.
    /// </summary>
    public class VertexMovedEventArgs : EventArgs
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexMovedEventArgs"/> class.
        /// </summary>
        /// <param name="affectedFeature">The feature affected by the vertex move.</param>
        public VertexMovedEventArgs(IFeature affectedFeature)
        {
            AffectedFeature = affectedFeature;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the affected feature.
        /// </summary>
        public IFeature AffectedFeature { get; set; }

        #endregion
    }
}