using System.Collections.Generic;
using System.Drawing;
using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Symbology;
using GeoAPI.Geometries;
using Point = System.Drawing.Point;

namespace DotSpatial.Plugins.ShapeEditor
{
    /// <summary>
    /// This is an abtract class that provides functionality for snapping objects.
    /// </summary>
    public abstract class SnappableMapFunction : MapFunctionZoom
    {
        #region Fields

        /// <summary>
        /// This is true if the current mouse position has been snapped.
        /// </summary>
        protected bool IsSnapped = false;

        /// <summary>
        /// List of layers that will be snapped to.
        /// </summary>
        protected List<IFeatureLayer> SnapLayers;

        /// <summary>
        /// This is the pen that will be used to draw the snapping circle.
        /// </summary>
        protected Pen SnapPen = new Pen(Color.HotPink, 2F);

        /// <summary>
        /// +/- N pixels around the mouse point.
        /// </summary>
        protected int SnapTol = 9;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="map"></param>
        public SnappableMapFunction(IMap map)
            : base(map)
        {
            DoSnapping = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// This determines if snapping is performed or not.
        /// </summary>
        public bool DoSnapping { get; set; }

        /// <summary>
        /// Feature the computed snappedCoord belongs to.
        /// </summary>
        public IFeature SnappedFeature { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Add the given layer to the snap list.  This list determines which layers
        /// the current layer will be snapped to.
        /// </summary>
        /// <param name="layer"></param>
        public void AddLayerToSnap(IFeatureLayer layer)
        {
            if (SnapLayers == null)
                InitializeSnapLayers();
            SnapLayers.Add(layer);
        }

        /// <summary>
        /// Computes a snapped coordinate.  If the mouse is near a snappable object, the output
        /// location of the mouse will be the coordinates of the object rather than the actual
        /// mouse coords.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="snappedCoord">set if a coordinate is found</param>
        /// <returns>true if snap found</returns>
        protected bool ComputeSnappedLocation(GeoMouseArgs e, ref Coordinate snappedCoord)
        {
            if (SnapLayers == null || e == null || Map == null)
                return false;

            Rectangle mouseRect = new Rectangle(e.X - SnapTol, e.Y - SnapTol, SnapTol * 2, SnapTol * 2);

            Extent pix = Map.PixelToProj(mouseRect);
            if (pix == null)
                return false;

            Envelope env = pix.ToEnvelope();

            foreach (IFeatureLayer layer in SnapLayers)
            {
                foreach (IFeature feat in layer.DataSet.Features)
                {
                    foreach (Coordinate c in feat.Geometry.Coordinates)
                    {
                        // If the mouse envelope contains the current coordinate, we found a snap location.
                        if (env.Contains(c))
                        {
                            snappedCoord = c;
                            SnappedFeature = feat;
                            return true;
                        }
                    }
                }
            }
            SnappedFeature = null;
            return false;
        }

        /// <summary>
        /// Perform any actions in the OnMouseMove event that are necessary for snap drawing.
        /// </summary>
        /// <param name="prevWasSnapped"></param>
        /// <param name="pos"></param>
        protected void DoMouseMoveForSnapDrawing(bool prevWasSnapped, Point pos)
        {
            // Invalidate the region around the mouse so that the previous snap colors are erased.
            if ((prevWasSnapped || IsSnapped) && DoSnapping)
            {
                Rectangle invalid = new Rectangle(pos.X - 30, pos.Y - 30, 60, 60);
                Map.Invalidate(invalid);
            }
        }

        /// <summary>
        /// Perform any drawing necessary for snapping (e.g. draw a circle around snapped location).
        /// </summary>
        /// <param name="graphics">graphics to draw on</param>
        /// <param name="pos">point where the circles center will be</param>
        protected void DoSnapDrawing(Graphics graphics, Point pos)
        {
            if (IsSnapped)
            {
                graphics.DrawEllipse(SnapPen, pos.X - 5, pos.Y - 5, 10, 10);
            }
        }

        /// <summary>
        /// Initialize/Reinitialize the list of snap layers (i.e. when a layer has
        /// been selected or reselected).
        /// </summary>
        protected void InitializeSnapLayers()
        {
            SnapLayers = new List<IFeatureLayer>();
        }

        #endregion
    }
}
