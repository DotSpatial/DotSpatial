using System;
using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.Shape
{
    public abstract class GeometricShapeBuilder
    {
        #region Fields

        protected IGeometryFactory GeomFactory;
        private IEnvelope _extent = new Envelope(0, 1, 0, 1);

        #endregion

        #region Constructors

        protected GeometricShapeBuilder(IGeometryFactory geomFactory)
        {
            GeomFactory = geomFactory;
        }

        #endregion

        #region Properties

        public Coordinate Centre
        {
            get { return _extent.Center(); }
        }

        public double Diameter
        {
            get { return Math.Min(_extent.Height, _extent.Width); }
        }

        public double Radius
        {
            get { return Diameter * 0.5; }
        }

        public IEnvelope Extent
        {
            get { return _extent; }
            set { _extent = value; }
        }

        /// <summary>
        /// Gets or sets the total number of points in the created <see cref="IGeometry"/>.
        /// The created geometry will have no more than this number of points,
        /// unless more are needed to create a valid geometry.
        /// </summary>
        public int NumPoints { get; set; }

        #endregion

        #region Methods

        protected Coordinate CreateCoord(double x, double y)
        {
            var pt = new Coordinate(x, y);
            GeomFactory.PrecisionModel.MakePrecise(pt);
            return pt;
        }

        public abstract IGeometry GetGeometry();

        public LineSegment GetSquareBaseLine()
        {
            var radius = Radius;

            var centre = Centre;
            var p0 = new Coordinate(centre.X - radius, centre.Y - radius);
            var p1 = new Coordinate(centre.X + radius, centre.Y - radius);

            return new LineSegment(p0, p1);
        }

        public Envelope GetSquareExtent()
        {
            var radius = Radius;

            var centre = Centre;
            return new Envelope(centre.X - radius, centre.X + radius,
                    centre.Y - radius, centre.Y + radius);
        }

        #endregion
    }
}