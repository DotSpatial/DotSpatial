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
using DotSpatial.Serialization;
using DotSpatial.Topology.Algorithm;
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Topology.Geometries
{
    /// <summary> 
    /// Represents a polygon with linear edges, which may include holes.
    /// The outer boundary (shell) 
    /// and inner boundaries (holes) of the polygon are represented by {@link LinearRing}s.
    /// The boundary rings of the polygon may have any orientation.
    /// Polygons are closed, simple geometries by definition.
    /// <para/>
    /// The polygon model conforms to the assertions specified in the 
    /// <a href="http://www.opengis.org/techno/specs.htm">OpenGIS Simple Features
    /// Specification for SQL</a>.
    /// <para/>
    /// A <c>Polygon</c> is topologically valid if and only if:
    /// <list type="Bullet">
    /// <item>the coordinates which define it are valid coordinates</item>
    /// <item>the linear rings for the shell and holes are valid
    /// (i.e. are closed and do not self-intersect)</item>
    /// <item>holes touch the shell or another hole at at most one point
    /// (which implies that the rings of the shell and holes must not cross)</item>
    /// <item>the interior of the polygon is connected,  
    /// or equivalently no sequence of touching holes 
    /// makes the interior of the polygon disconnected
    /// (i.e. effectively split the polygon into two pieces).</item>
    /// </list>
    /// </summary>
    [Serializable]
    public class Polygon : Geometry, IPolygon
    {
        #region Fields

        /// <summary>
        /// Represents an empty <c>Polygon</c>.
        /// </summary>
        public static readonly IPolygon Empty = new GeometryFactory().CreatePolygon(null, null);

        /// <summary>
        /// The interior boundaries, if any.
        /// </summary>
        private ILinearRing[] _holes;

        /// <summary>
        /// The exterior boundary, or <c>null</c> if this <c>Polygon</c>
        /// is the empty point.
        /// </summary>
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
                inShell = Factory.CreateLinearRing(new List<Coordinate>());
            if (inHoles == null)
                inHoles = new ILinearRing[] { };
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
        public Polygon(IPolygon polygonBase)
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

        #region Properties

        /// <summary>
        /// Returns the area of this <c>Polygon</c>
        /// </summary>
        /// <returns>Area in Meters (by default) when using projected coordinates.</returns>
        public override double Area
        {
            get
            {
                var area = 0.0;
                area += Math.Abs(CGAlgorithms.SignedArea(_shell.CoordinateSequence));
                for (int i = 0; i < _holes.Length; i++)
                    area -= Math.Abs(CGAlgorithms.SignedArea(_holes[i].CoordinateSequence));
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
                    return Factory.CreateMultiLineString(null);
                var rings = new ILinearRing[_holes.Length + 1];
                rings[0] = _shell;
                for (var i = 0; i < _holes.Length; i++)
                    rings[i + 1] = _holes[i];
                // create LineString or MultiLineString as appropriate
                if (rings.Length <= 1)
                    return Factory.CreateLinearRing(rings[0].CoordinateSequence);
                return Factory.CreateMultiLineString(CollectionUtil.Cast<ILinearRing, ILineString>(rings));
            }
        }

        /// <summary> 
        /// Returns the dimension of this <c>Geometry</c>s inherent boundary.
        /// </summary>
        /// <returns>    
        /// The dimension of the boundary of the class implementing this
        /// interface, whether or not this object is the empty point. Returns
        /// <c>Dimension.False</c> if the boundary is the empty point.
        /// </returns>
        public override Dimension BoundaryDimension
        {
            get
            {
                return Dimension.Curve;
            }
        }

        /// <summary>  
        /// Returns a vertex of this <c>Geometry</c>
        /// (usually, but not necessarily, the first one).
        /// </summary>
        /// <remarks>
        /// The returned coordinate should not be assumed to be an actual Coordinate object used in the internal representation. 
        /// </remarks>
        /// <returns>a Coordinate which is a vertex of this <c>Geometry</c>.</returns>
        /// <returns><c>null</c> if this Geometry is empty.
        /// </returns>
        public override Coordinate Coordinate
        {
            get
            {
                return _shell.Coordinate;
            }
        }

        /// <summary>
        /// Gets all the coordinates for the polygon.
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
        /// Returns the dimension of this geometry.
        /// </summary>
        /// <remarks>
        /// The dimension of a geometry is is the topological 
        /// dimension of its embedding in the 2-D Euclidean plane.
        /// In the NTS spatial model, dimension values are in the set {0,1,2}.
        /// <para>
        /// Note that this is a different concept to the dimension of 
        /// the vertex <see cref="Coordinate"/>s.
        /// The geometry dimension can never be greater than the coordinate dimension.
        /// For example, a 0-dimensional geometry (e.g. a Point) 
        /// may have a coordinate dimension of 3 (X,Y,Z). 
        /// </para>
        /// </remarks>
        /// <returns>  
        /// The topological dimensions of this geometry
        /// </returns>
        public override Dimension Dimension
        {
            get
            {
                return Dimension.Surface;
            }
        }

        /// <summary>
        /// This is just the Shell, but modified to work with IBasicPolygon
        /// </summary>
        public ILineString ExteriorRing
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
        public ILineString[] InteriorRings
        {
            get
            {
                return CollectionUtil.Cast<ILinearRing, ILineString>(_holes);
            }
        }

        // Collections can be arrays or lists
        public ILinearRing[] Holes
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
                var seq = Shell.CoordinateSequence;
                var env = EnvelopeInternal;
                for (int i = 0; i < 5; i++)
                {
                    double x = seq.GetX(i);
                    if (!(x == env.Minimum.X || x == env.Maximum.X))
                        return false;

                    double y = seq.GetY(i);
                    if (!(y == env.Minimum.Y || y == env.Maximum.Y))
                        return false;
                }

                // check vertices are in right order
                var prevX = seq.GetX(0);
                var prevY = seq.GetY(0);
                for (var i = 1; i <= 4; i++)
                {
                    var x = seq.GetX(i);
                    var y = seq.GetY(i);

                    var xChanged = x != prevX;
                    var yChanged = y != prevY;

                    if (xChanged == yChanged)
                        return false;

                    prevX = x;
                    prevY = y;
                }
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
        public int NumInteriorRings
        {
            get
            {
                return _holes.Length;
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

        public override OgcGeometryType OgcGeometryType
        {
            get { return OgcGeometryType.Polygon; }
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

            set { _shell = value; }
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

        public override void Apply(ICoordinateSequenceFilter filter)
        {
            ((LinearRing)_shell).Apply(filter);
            if (!filter.Done)
            {
                for (int i = 0; i < _holes.Length; i++)
                {
                    ((LinearRing)_holes[i]).Apply(filter);
                    if (filter.Done)
                        break;
                }
            }
            if (filter.GeometryChanged)
                GeometryChanged();
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
        /// <param name="o"></param>
        /// <returns></returns>
        public override int CompareToSameClass(object o)
        {
            ILinearRing thisShell = _shell;
            ILinearRing otherShell = ((IPolygon)o).Shell;
            return thisShell.CompareToSameClass(otherShell);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        protected internal override int CompareToSameClass(object other, IComparer<ICoordinateSequence> comparer)
        {
            var poly = (IPolygon)other;

            var thisShell = (LinearRing)_shell;
            var otherShell = (LinearRing)poly.Shell;
            int shellComp = thisShell.CompareToSameClass(otherShell, comparer);
            if (shellComp != 0) return shellComp;

            int nHole1 = NumHoles;
            int nHole2 = poly.NumHoles;
            int i = 0;
            while (i < nHole1 && i < nHole2)
            {
                var thisHole = (LinearRing)GetInteriorRingN(i);
                var otherHole = (LinearRing)poly.GetInteriorRingN(i);
                var holeComp = thisHole.CompareToSameClass(otherHole, comparer);
                if (holeComp != 0) return holeComp;
                i++;
            }
            if (i < nHole1) return 1;
            if (i < nHole2) return -1;
            return 0;
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
            var otherPolygon = (IPolygon)other;
            IGeometry thisShell = _shell;
            IGeometry otherPolygonShell = otherPolygon.Shell;
            if (!thisShell.EqualsExact(otherPolygonShell, tolerance))
                return false;
            if (_holes.Length != otherPolygon.Holes.Length)
                return false;
            for (int i = 0; i < _holes.Length; i++)
                if (!_holes[i].EqualsExact(otherPolygon.Holes[i], tolerance))
                    return false;
            return true;
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
        /// Gets an array of <see cref="System.Double"/> ordinate values
        /// </summary>
        /// <param name="ordinate">The ordinate index</param>
        /// <returns>An array of ordinate values</returns>
        public override double[] GetOrdinates(Ordinate ordinate)
        {
            if (IsEmpty)
                return new double[0];

            var ordinateFlag = OrdinatesUtility.ToOrdinatesFlag(ordinate);
            if ((_shell.CoordinateSequence.Ordinates & ordinateFlag) != ordinateFlag)
                return CreateArray(NumPoints, Coordinate.NullOrdinate);

            var result = new double[NumPoints];
            var ordinates = _shell.GetOrdinates(ordinate);
            Array.Copy(ordinates, 0, result, 0, ordinates.Length);
            var offset = ordinates.Length;
            foreach (var linearRing in _holes)
            {
                ordinates = linearRing.GetOrdinates(ordinate);
                Array.Copy(ordinates, 0, result, offset, ordinates.Length);
                offset += ordinates.Length;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Normalize()
        {
            Normalize(_shell, true);
            foreach (ILinearRing hole in _holes)
                Normalize(hole, false);
            Array.Sort(Holes);
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
                result.Add(uniqueCoordinates[i]);
            result.Add(uniqueCoordinates[0].Copy());
            ring.Coordinates = result;
            if (CGAlgorithms.IsCounterClockwise(ring.Coordinates) == clockwise)
                ring.Coordinates = ring.Coordinates.Reverse().ToList();
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

        public override IGeometry Reverse()
        {
            var poly = new Polygon(new List<Coordinate>());
            OnCopy(poly);
            poly.Shell = (LinearRing)((LinearRing)_shell.Clone()).Reverse();
            poly.Holes = CollectionUtil.Cast<LinearRing, ILinearRing>(new LinearRing[_holes.Length]);
            for (int i = 0; i < _holes.Length; i++)
            {
                poly.Holes[i] = (LinearRing)((LinearRing)_holes[i].Clone()).Reverse();
            }
            return poly;// return the clone
        }

        /// <summary>
        /// Rotates the polygon by the given radian angle around the Origin.
        /// </summary>
        /// <param name="origin">Coordinate the polygon gets rotated around.</param>
        /// <param name="radAngle">Rotation angle in radian.</param>
        public override void Rotate(Coordinate origin, Double radAngle)
        {
            _shell.Rotate(origin, radAngle);

            foreach (var h in Holes)
            {
                h.Rotate(origin, radAngle);
            }
        }

        /// <summary>
        /// Hole data might actually already be cast appropriately, but it might need to be
        /// converted into an array of linear rings.
        /// </summary>
        /// <param name="holeData"></param>
        private void SetHoles(ICollection<ILineString> holeData)
        {
            if (holeData == null || holeData.Count == 0)
            {
                _holes = new ILinearRing[] { };
                return;
            }
            _holes = holeData as ILinearRing[];
            if (_holes != null) return;

            List<ILinearRing> rings = new List<ILinearRing>();
            foreach (ILineString linestring in holeData)
            {
                rings.Add(new LinearRing(FromBasicGeometry(linestring) as ILineString));
            }
            _holes = rings.ToArray();
        }

        private void SetShell(ILineString basicShell)
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
    }
}