using System.Collections.Generic;
using System.Drawing;
using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Symbology;

namespace DotSpatial.Plugins.ShapeEditor
{
    /// <summary>
    /// This is an abtract class that provides functionality for snapping objects.
    /// </summary>
    public abstract class SnappableMapFunction : MapFunction
    {
        /// <summary>
        /// +/- N pixels around the mouse point.
        /// </summary>
        protected int snapTol = 9;

        /// <summary>
        /// List of layers that will be snapped to.
        /// </summary>
        protected List<IFeatureLayer> snapLayers = null;

        /// <summary>
        /// This is true if the current mouse position has been snapped.
        /// </summary>
        protected bool isSnapped = false;

        /// <summary>
        /// This is the pen that will be used to draw the snapping circle.
        /// </summary>
        protected Pen snapPen = new Pen(Color.HotPink, 2F);

        /// <summary>
        /// This determines if snapping is performed or not.
        /// </summary>
        public bool DoSnapping { get; set; }

        private IFeature _snappedFeature;

        /// <summary>
        /// Feature the computed snappedCoord belongs to.
        /// </summary>
        public IFeature SnappedFeature
        {
            get { return _snappedFeature; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public SnappableMapFunction()
        {
            this.DoSnapping = false;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="map"></param>
        public SnappableMapFunction(IMap map)
            : base(map)
        {
            this.DoSnapping = false;
        }

        /// <summary>
        /// Add the given layer to the snap list.  This list determines which layers
        /// the current layer will be snapped to.
        /// </summary>
        /// <param name="layer"></param>
        public void AddLayerToSnap(IFeatureLayer layer)
        {
            if (this.snapLayers == null)
                InitializeSnapLayers();
            this.snapLayers.Add(layer);
        }

        /// <summary>
        /// Initialize/Reinitialize the list of snap layers (i.e. when a layer has
        /// been selected or reselected).
        /// </summary>
        protected void InitializeSnapLayers()
        {
            this.snapLayers = new List<IFeatureLayer>();
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
            if (this.snapLayers == null || e == null || Map == null)
                return false;

            Rectangle mouseRect = new Rectangle(e.X - this.snapTol, e.Y - this.snapTol, this.snapTol * 2, this.snapTol * 2);

            Extent pix = Map.PixelToProj(mouseRect);
            if (pix == null)
                return false;

            IEnvelope env = pix.ToEnvelope();

            foreach (IFeatureLayer layer in this.snapLayers)
            {
                foreach (IFeature feat in layer.DataSet.Features)
                {
                    foreach (Coordinate c in feat.Coordinates)
                    {
                        // If the mouse envelope contains the current coordinate, we found a snap location.
                        if (env.Contains(c))
                        {
                            snappedCoord = c;
                            _snappedFeature = feat;
                            return true;
                        }
                    }
                }
            }
            _snappedFeature = null;
            return false;
        }

        /// <summary>
        /// Perform any drawing necessary for snapping (e.g. draw a circle around snapped location).
        /// </summary>
        /// <param name="graphics">graphics to draw on</param>
        protected void DoSnapDrawing(Graphics graphics, Point pos)
        {
            if (this.isSnapped)
            {
                graphics.DrawEllipse(this.snapPen, pos.X - 5, pos.Y - 5, 10, 10);
            }
        }

        /// <summary>
        /// Perform any actions in the OnMouseMove event that are necessary for snap drawing.
        /// </summary>
        /// <param name="prevWasSnapped"></param>
        /// <param name="pos"></param>
        protected void DoMouseMoveForSnapDrawing(bool prevWasSnapped, Point pos)
        {
            // Invalidate the region around the mouse so that the previous snap colors are erased.
            if ((prevWasSnapped || this.isSnapped) && this.DoSnapping)
            {
                Rectangle invalid = new Rectangle(pos.X - 30, pos.Y - 30, 60, 60);
                Map.Invalidate(invalid);
            }
        }
    }
}
