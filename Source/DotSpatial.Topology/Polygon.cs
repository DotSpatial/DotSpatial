// ********************************************************************************************************
// Product Name: DotSpatial.Topology.dll
// Description:  The basic topology module for the new dotSpatial libraries
// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is also protected by the GNU Lesser Public License and the sourcecode
// for the Net Topology Suite can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from the Net Topology Suite, which is a C# port of the Java Topology Suite.
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |         Name         |    Date    |                              Comment
// |----------------------|------------|------------------------------------------------------------
// |                      |            |
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using DotSpatial.Topology.Algorithm;

namespace DotSpatial.Topology
{
    /// <summary>
    /// Represents a linear polygon, which may include holes.
    /// The shell and holes of the polygon are represented by {LinearRing}s.
    /// In a valid polygon, holes may touch the shell or other holes at a single point.
    /// However, no sequence of touching holes may split the polygon into two pieces.
    /// The orientation of the rings in the polygon does not matter.
    /// The shell and holes must conform to the assertions specified in the
    /// <see href="http://www.opengis.org/techno/specs.htm"/> OpenGIS Simple Features Specification for SQL.
    /// </summary>
    [Serializable]
    public class Polygon : Geometry, IPolygon
    {
        #region Private Variables

        private ILinearRing[] _holes;
        private ILinearRing _shell;

        #endregion

        #region Constructors

        /* BEGIN ADDED BY MPAUL42: monoGIS team */

        /// <summary>
        /// Constructs a <c>Polygon</c> with the given exterior boundary.
        /// </summary>
        /// <param name="shell">
        /// The outer boundary of the new <c>Polygon</c>,
        /// or <c>null</c> or an empty <c>LinearRing</c> if the empty
        /// polygon is to be created.
        /// </param>
        /// <param name="factory"></param>
        public Polygon(ILinearRing shell, IGeometryFactory factory) : this(shell, null, factory) { }

        /// <summary>
        /// Constructs a <c>Polygon</c> with the given exterior boundary.
        /// </summary>
        /// <param name="shell">
        /// The outer boundary of the new <c>Polygon</c>,
        /// or <c>null</c> or an empty <c>LinearRing</c> if the empty
        /// polygon is to be created.
        /// </param>
        public Polygon(ILinearRing shell) : this(shell, null, DefaultFactory) { }

        /// <summary>
        /// Generates a new polygon using the default geometry factory from the specified set of coordinates,
        /// where the coordinates will become the polygon shell.
        /// </summary>
        /// <param name="shell">The shell of the polygon expressed as an enumerable collection of ICoordinate</param>
        public Polygon(IEnumerable<Coordinate> shell)
            : this(new LinearRing(shell))
        {
        }

        /// <summary>
        /// Initializes a new instance of the Polygon class.
        /// </summary>
        /// <param name="shell">
        /// The outer boundary of the new <c>Polygon</c>,
        /// or <c>null</c> or an empty <c>LinearRing</c> if the empty
        /// point is to be created.
        /// </param>
        /// <param name="holes">
        /// The inner boundaries of the new <c>Polygon</c>
        ///, or <c>null</c> or empty <c>LinearRing</c>s if the empty
        /// point is to be created.
        /// </param>
        /// <remarks>
        /// For create this <see cref="Geometry"/> is used a standard <see cref="GeometryFactory"/>
        /// with <see cref="PrecisionModel" /> <c> == </c> <see cref="PrecisionModelType.Floating"/>.
        /// </remarks>
        public Polygon(ILinearRing shell, ILinearRing[] holes) : this(shell, holes, DefaultFactory) { }

        /// <summary>
        /// Constructs a <c>Polygon</c> with the given exterior boundary and
        /// interior boundaries.
        /// </summary>
        /// <param name="inShell">
        /// The outer boundary of the new <c>Polygon</c>,
        /// or <c>null</c> or an empty <c>LinearRing</c> if the empty
        /// point is to be created.
        /// </param>
        /// <param name="inHoles">
        /// The inner boundaries of the new <c>Polygon</c>
        ///, or <c>null</c> or empty <c>LinearRing</c>s if the empty
        /// point is to be created.
        /// </param>
        /// <param name="factory"></param>
        /// <exception cref="PolygonException">Holes must not contain null elements</exception>
        public Polygon(ILinearRing inShell, ILinearRing[] inHoles, IGeometryFactory factory)
            : base(factory)
        {
            if (inShell == null)
                inShell = Factory.CreateLinearRing(null);
            if (inHoles == null)
                inHoles = new LinearRing[] { };
            if (HasNullElements(inHoles))
            {
                throw new PolygonException(TopologyText.PolygonException_HoleElementNull);
            }
            if (inShell.IsEmpty && HasNonEmptyElements(inHoles))
                throw new PolygonException(TopologyText.PolygonException_ShellEmptyButHolesNot);
            _shell = inShell;
            _holes = inHoles;
        }

        /// <summary>
        /// Constructor for a polygon
        /// </summary>
        /// <param name="polygonBase">A simpler BasicPolygon to empower with topology functions</param>
        public Polygon(IBasicPolygon polygonBase)
            : base(DefaultFactory)
        {
            SetHoles(polygonBase.Holes);

            LinearRing shell = new LinearRing(polygonBase.Shell);
            if (HasNullElements(_holes))
                throw new PolygonException(TopologyText.PolygonException_HoleElementNull);
            if (shell.IsEmpty && HasNonEmptyElements(_holes))
                throw new PolygonException(TopologyText.PolygonException_ShellEmptyButHolesNot);
            _shell = shell;
        }

        #endregion

        #region Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="filter"></param>
        public override void Apply(ICoordinateFilter filter)
        {
            _shell.Apply(filter);
            for (int i = 0; i < _holes.Length; i++)
                _holes[i].Apply(filter);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filter"></param>
        public override void Apply(IGeometryFilter filter)
        {
            filter.Filter(this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filter"></param>
        public override void Apply(IGeometryComponentFilter filter)
        {
            filter.Filter(this);
            _shell.Apply(filter);
            for (int i = 0; i < _holes.Length; i++)
                _holes[i].Apply(filter);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override int CompareToSameClass(object o)
        {
            ILinearRing thisShell = _shell;
            ILinearRing otherShell = ((IPolygon)o).Shell;
            return thisShell.CompareToSameClass(otherShell);
        }

        /// <summary>
        /// Clears any cached envelopes
        /// </summary>
        public override void ClearEnvelope()
        {
            _shell.ClearEnvelope();
            foreach (ILinearRing ring in Holes)
            {
                ring.ClearEnvelope();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override IGeometry ConvexHull()
        {
            return _shell.ConvexHull();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="other"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public override bool EqualsExact(IGeometry other, double tolerance)
        {
            if (!IsEquivalentClass(other))
                return false;
            Polygon otherPolygon = (Polygon)other;
            IGeometry thisShell = _shell;
            IGeometry otherPolygonShell = otherPolygon.Shell;
            if (!thisShell.EqualsExact(otherPolygonShell, tolerance))
                return false;
            if (_holes.Length != otherPolygon.Holes.Length)
                return false;
            if (_holes.Length != otherPolygon.Holes.Length)
                return false;
            for (int i = 0; i < _holes.Length; i++)
                if (!_holes[i].EqualsExact(otherPolygon.Holes[i], tolerance))
                    return false;
            return true;
        }

        /// <summary>
        /// Given the specified test point, this checks each segment, and will
        /// return the closest point on the specified segment.
        /// </summary>
        /// <param name="testPoint">The point to test.</param>
        /// <returns></returns>
        public override Coordinate ClosestPoint(Coordinate testPoint)
        {
            // For a point outside the polygon, it must be closer to the shell than
            // any holes.
            if (Intersects(new Point(testPoint)) == false)
            {
                return _shell.ClosestPoint(testPoint);
            }

            Coordinate closest = _shell.ClosestPoint(testPoint);
            double dist = testPoint.Distance(closest);
            foreach (ILinearRing ring in Holes)
            {
                Coordinate temp = ring.ClosestPoint(testPoint);
                double tempDist = testPoint.Distance(temp);
                if (tempDist >= dist) continue;
                dist = tempDist;
                closest = temp;
            }
            return closest;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public virtual ILineString GetInteriorRingN(int n)
        {
            return _holes[n];
        }

        /// <summary>
        ///
        /// </summary>
        public override void Normalize()
        {
            Normalize(_shell, true);
            foreach (LinearRing hole in _holes)
                Normalize(hole, false);
            Array.Sort(Holes);
        }

        /// <summary>
        /// Occurs during the copy process and ensures that the shell and holes are all duplicated and not direct references
        /// </summary>
        /// <param name="copy"></param>
        protected override void OnCopy(Geometry copy)
        {
            base.OnCopy(copy);
            Polygon poly = copy as Polygon;
            if (poly == null) return;
            poly.Shell = _shell.Copy();
            poly.Holes = new ILinearRing[_holes.Length];
            for (int i = 0; i < _holes.Length; i++)
                poly.Holes[i] = Holes[i].Copy();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override IEnvelope ComputeEnvelopeInternal()
        {
            return _shell.EnvelopeInternal;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ring"></param>
        /// <param name="clockwise"></param>
        private static void Normalize(ILinearRing ring, bool clockwise)
        {
            if (ring.IsEmpty)
                return;
            Coordinate[] uniqueCoordinates = new Coordinate[ring.Coordinates.Count - 1];
            for (int i = 0; i < uniqueCoordinates.Length; i++)
                uniqueCoordinates[i] = ring.Coordinates[i];
            Coordinate minCoordinate = CoordinateArrays.MinCoordinate(uniqueCoordinates);
            CoordinateArrays.Scroll(uniqueCoordinates, minCoordinate);
            List<Coordinate> result = new List<Coordinate>();
            for (int i = 0; i < uniqueCoordinates.Length; i++)
                result.Add(ring.Coordinates[i]);
            result.Add(uniqueCoordinates[0].Copy());
            ring.Coordinates = result;
            if (CgAlgorithms.IsCounterClockwise(ring.Coordinates) == clockwise)
                ring.Coordinates.Reverse();
        }

        #endregion

        #region Properties

        /// <summary>
        /// This is just the Shell, but modified to work with IBasicPolygon
        /// </summary>
        public virtual IBasicLineString ExteriorRing
        {
            get
            {
                return _shell;
            }
        }

        /// <summary>
        /// Returns the area of this <c>Polygon</c>
        /// </summary>
        /// <returns>Area in Meters (by default) when using projected coordinates.</returns>
        public override double Area
        {
            get
            {
                double area = 0.0;
                area += Math.Abs(CgAlgorithms.SignedArea(_shell.Coordinates));
                for (int i = 0; i < _holes.Length; i++)
                    area -= Math.Abs(CgAlgorithms.SignedArea(_holes[i].Coordinates));
                return area;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override IGeometry Boundary
        {
            get
            {
                if (IsEmpty)
                    return Factory.CreateGeometryCollection(null);
                ILinearRing[] rings = new ILinearRing[_holes.Length + 1];
                rings[0] = _shell;
                for (int i = 0; i < _holes.Length; i++)
                    rings[i + 1] = _holes[i];
                if (rings.Length <= 1)
                    return Factory.CreateLinearRing(rings[0].Coordinates);
                return Factory.CreateMultiLineString(rings);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override DimensionType BoundaryDimension
        {
            get
            {
                return DimensionType.Curve;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override Coordinate Coordinate
        {
            get
            {
                return _shell.Coordinate;
            }
        }

        /// <summary>
        /// Gets all the coordinates for the polygon.  Setting this assumes that all the coordintes
        /// belong in the shell.
        /// </summary>
        public override IList<Coordinate> Coordinates
        {
            get
            {
                if (IsEmpty)
                    return new Coordinate[] { };

                List<Coordinate> result = new List<Coordinate>();
                result.AddRange(Shell.Coordinates);
                foreach (var ring in Holes)
                {
                    result.AddRange(ring.Coordinates);
                }
                return result;
            }
            set
            {
                // Sets the coordinates of the shell and doesn't change holes
                Shell.Coordinates = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override DimensionType Dimension
        {
            get
            {
                return DimensionType.Surface;
            }
        }

        /// <summary>
        /// This will always contain points, even if it is technically empty
        /// </summary>
        public override FeatureType FeatureType
        {
            get
            {
                return FeatureType.Polygon;
            }
        }

        /// <summary>
        /// Specifically returns a Polygon type
        /// </summary>
        public override string GeometryType
        {
            get
            {
                return "Polygon";
            }
        }

        /// <summary>
        ///
        /// </summary>
        public ILinearRing[] Holes
        {
            get
            {
                return _holes;
            }
            set
            {
                _holes = value;
            }
        }

        // Collections can be arrays or lists
        ICollection<IBasicLineString> IBasicPolygon.Holes
        {
            get
            {
                return _holes;
            }
            set
            {
                SetHoles(value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override bool IsEmpty
        {
            get
            {
                return _shell.IsEmpty;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override bool IsRectangle
        {
            get
            {
                if (NumHoles != 0) return false;
                if (_shell == null) return false;
                if (_shell.NumPoints != 5) return false;

                // check vertices have correct values
                IList<Coordinate> seq = _shell.Coordinates;
                IEnvelope env = EnvelopeInternal;
                for (int i = 0; i < 5; i++)
                {
                    Coordinate coord = seq[i];
                    double x = coord.X;
                    if (!(x == env.Minimum.X || x == env.Maximum.X))
                        return false;

                    double y = coord.Y;
                    if (!(y == env.Minimum.Y || y == env.Maximum.Y))
                        return false;
                }

                // check vertices are in right order
                double prevX = seq[0].X;
                double prevY = seq[0].Y;
                for (int i = 1; i <= 4; i++)
                {
                    double x = seq[i].X;
                    double y = seq[i].Y;

                    bool xChanged = x != prevX;
                    bool yChanged = y != prevY;

                    if (xChanged == yChanged)
                        return false;

                    prevX = x;
                    prevY = y;
                }
                return true;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override bool IsSimple
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Returns the perimeter of this <c>Polygon</c>.
        /// </summary>
        /// <returns></returns>
        public override double Length
        {
            get
            {
                double len = 0.0;
                len += _shell.Length;
                foreach (ILinearRing ring in Holes)
                {
                    len += ring.Length;
                }
                return len;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual int NumHoles
        {
            get
            {
                return _holes.Length;
            }
        }

        /// <summary>
        /// For polygons, this returns the complete number of points, including all the points
        /// from the outer ring as well as from the interior holes.
        /// </summary>
        public override int NumPoints
        {
            get
            {
                int numPoints = _shell.NumPoints;
                for (int i = 0; i < _holes.Length; i++)
                    numPoints += _holes[i].NumPoints;
                return numPoints;
            }
        }

        /// <summary>
        /// This returns a full ILinearRing geometry
        /// </summary>
        public virtual ILinearRing Shell
        {
            get
            {
                return _shell;
            }
            set
            {
                _shell = value;
            }
        }

        IBasicLineString IBasicPolygon.Shell
        {
            get
            {
                return _shell;
            }
            set
            {
                SetShell(value);
            }
        }

        /// <summary>
        /// Hole data might actually already be cast appropriately, but it might need to be
        /// converted into an array of linear rings.
        /// </summary>
        /// <param name="holeData"></param>
        private void SetHoles(ICollection<IBasicLineString> holeData)
        {
            if (holeData == null || holeData.Count == 0)
            {
                _holes = new ILinearRing[] { };
                return;
            }
            _holes = holeData as ILinearRing[];
            if (_holes != null) return;

            List<ILinearRing> rings = new List<ILinearRing>();
            foreach (IBasicLineString linestring in holeData)
            {
                rings.Add(new LinearRing(FromBasicGeometry(linestring) as ILineString));
            }
            _holes = rings.ToArray();
        }

        private void SetShell(IBasicLineString basicShell)
        {
            if (basicShell == null)
            {
                _shell = null;
                return;
            }
            ILinearRing ring = basicShell as ILinearRing;
            if (ring == null)
            {
                _shell = new LinearRing(basicShell);
            }
        }

        #endregion

        #region Static

        /// <summary>
        /// Represents an empty <c>Polygon</c>.
        /// </summary>
        public static readonly IPolygon Empty = new GeometryFactory().CreatePolygon(null, null);

        #endregion
    }
}