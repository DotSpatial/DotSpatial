using System;
using System.Collections.Generic;
using DotSpatial.Data;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A LiDAR layer designed to support displaying Large LiDAR point
    /// cloud data
    /// </summary>
    /// <remarks>This layer is not yet fully implemented</remarks>
    public class MapLiDARLayer : Layer, IMapLiDARLayer
    {
        #region IMapLiDARLayer Members

        /// <summary>
        /// The LiDAR DataSet associated with the LiDAR layer
        /// </summary>
        public new ILiDARData DataSet
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Draws the LiDAR data points in the currently visible map display
        /// </summary>
        /// <param name="args">Information about the map display</param>
        /// <param name="regions">Information about the visible regions</param>
        public void DrawRegions(MapArgs args, List<Extent> regions)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}