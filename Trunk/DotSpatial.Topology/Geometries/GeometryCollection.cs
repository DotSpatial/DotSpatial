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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Topology.Geometries
{
    /// <summary>
    /// Basic implementation of <c>GeometryCollection</c>.
    /// </summary>
    [Serializable]
    public class GeometryCollection : Geometry, IGeometryCollection
    {
        #region Fields

        /// <summary>
        /// Represents an empty <c>GeometryCollection</c>.
        /// </summary>
        public static readonly IGeometryCollection Empty = DefaultFactory.CreateGeometryCollection(null);

        /// <summary>
        /// Internal representation of this <c>GeometryCollection</c>.        
        /// </summary>
        private IGeometry[] _geometries;

        #endregion

        #region Constructors

        /// <summary>
        ///
        /// </summary>
        /// <param name="inGeometries">
        /// The <c>Geometry</c>s for this <c>GeometryCollection</c>,
        /// or <c>null</c> or an empty array to create the empty
        /// point. Elements may be empty <c>Geometry</c>s,
        /// but not <c>null</c>s.
        /// </param>
        /// <remarks>
        /// For create this <see cref="Geometry"/> is used a standard <see cref="GeometryFactory"/>
        /// with <see cref="PrecisionModel" /> <c> == </c> <see cref="PrecisionModelType.Floating"/>.
        /// </remarks>
        public GeometryCollection(IGeometry[] inGeometries) : this(inGeometries, DefaultFactory) { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inGeometries">
        /// The <c>Geometry</c>s for this <c>GeometryCollection</c>,
        /// or <c>null</c> or an empty array to create the empty
        /// point. Elements may be empty <c>Geometry</c>s,
        /// but not <c>null</c>s.
        /// </param>
        /// <param name="factory"></param>
        public GeometryCollection(IGeometry[] inGeometries, IGeometryFactory factory)
            : base(factory)
        {
            if (inGeometries == null) inGeometries = new IGeometry[] { };
            if (HasNullElements(inGeometries))
                throw new ArgumentException("geometries must not contain null elements");
            _geometries = inGeometries;
        }

        /// <summary>
        /// If the input geometry is a singular basic geometry, this will become a collection of 1 geometry.
        /// If the input geometry is a multi- basic geometry, this will simply ensure that each member
        /// is upgraded to a full geometry.
        /// </summary>
        /// <param name="inGeometry"></param>
        /// <param name="inFactory"></param>
        public GeometryCollection(IBasicGeometry inGeometry, IGeometryFactory inFactory)
            : base(inFactory)
        {
            if (inGeometry == null)
            {
                _geometries = new IGeometry[] { };
                return;
            }

            IBasicPolygon pg = inGeometry.GetBasicGeometryN(0) as IBasicPolygon;
            if (pg != null)
            {
                _geometries = new IGeometry[inGeometry.NumGeometries];
                for (int iGeom = 0; iGeom < inGeometry.NumGeometries; iGeom++)
                {
                    pg = inGeometry.GetBasicGeometryN(iGeom) as IBasicPolygon;
                    _geometries[iGeom] = new Polygon(pg);
                }
                return;
            }
            IBasicPoint pt = inGeometry.GetBasicGeometryN(0) as IBasicPoint;
            if (pt != null)
            {
                _geometries = new IGeometry[inGeometry.NumGeometries];
                for (int iGeom = 0; iGeom < inGeometry.NumGeometries; iGeom++)
                {
                    pt = inGeometry.GetBasicGeometryN(iGeom) as IBasicPoint;
                    _geometries[iGeom] = new Point(pt);
                }
                return;
            }
            IBasicLineString ls = inGeometry.GetBasicGeometryN(0) as IBasicLineString;
            if (ls != null)
            {
                _geometries = new IGeometry[inGeometry.NumGeometries];
                for (int iGeom = 0; iGeom < inGeometry.NumGeometries; iGeom++)
                {
                    ls = inGeometry.GetBasicGeometryN(iGeom) as IBasicLineString;
                    _geometries[iGeom] = new LineString(ls);
                }
                return;
            }
        }

        /// <summary>
        /// Creates a new Geometry Collection
        /// </summary>
        /// <param name="baseGeometries"></param>
        /// <param name="factory"></param>
        public GeometryCollection(IEnumerable<IBasicGeometry> baseGeometries, IGeometryFactory factory)
            : base(factory)
        {
            if (baseGeometries == null) return; // Don't set up the geometries array
            int count = baseGeometries.Count();

            if (_geometries == null)
                _geometries = new IGeometry[count];
            if (HasNullElements(baseGeometries))
                throw new ArgumentException("geometries must not contain null elements");

            int index = 0;
            foreach (IBasicGeometry baseGeometry in baseGeometries)
            {
                _geometries[index] = FromBasicGeometry(baseGeometry);
                index = index + 1;
            }
        }

        /// <summary>
        /// This should only be used by derived classes because it does not actually set the geometries
        /// </summary>
        protected GeometryCollection()
            : base(DefaultFactory)
        {
        }

        /// <summary>
        /// This should only be used by derived classes because it does not actually set the geometries
        /// </summary>
        /// <param name="factory">Specifies the factory that should be used when creating shapes in this multigeometry</param>
        protected GeometryCollection(IGeometryFactory factory)
            : base(factory)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns the area of this <c>GeometryCollection</c>.
        /// </summary>
        public override double Area
        {
            get
            {
                double area = 0.0;
                for (int i = 0; i < _geometries.Length; i++)
                    area += _geometries[i].Area;
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
                CheckNotGeometryCollection(this);
                throw new ShouldNeverReachHereException();
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override DimensionType BoundaryDimension
        {
            get
            {
                DimensionType dimension = DimensionType.False;
                for (int i = 0; i < _geometries.Length; i++)
                    dimension = (DimensionType)Math.Max((int)dimension, (int)(_geometries[i].BoundaryDimension));
                return dimension;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override Coordinate Coordinate
        {
            get
            {
                if (IsEmpty)
                    return null;
                return _geometries[0].Coordinate;
            }
        }

        /// <summary>
        /// Collects all coordinates of all subgeometries into an Array.
        /// Note that while changes to the coordinate objects themselves
        /// may modify the Geometries in place, the returned Array as such 
        /// is only a temporary container which is not synchronized back.
        /// </summary>
        /// <returns>The collected coordinates.</returns>
        public override IList<Coordinate> Coordinates
        {
            get
            {
                IList<Coordinate> coordinates = new Coordinate[NumPoints];
                int k = -1;
                for (int i = 0; i < _geometries.Length; i++)
                {
                    IList<Coordinate> childCoordinates = _geometries[i].Coordinates;
                    for (int j = 0; j < childCoordinates.Count; j++)
                    {
                        k++;
                        coordinates[k] = childCoordinates[j];
                    }
                }
                return coordinates;
            }
            set
            {
                // Make the assumption that we are setting only the first geometry coordinates?
                if (_geometries.Length < 1) return;
                _geometries[0].Coordinates = value;
            }
        }

        /* BEGIN ADDED BY MPAUL42: monoGIS team */

        /// <summary>
        /// Returns the number of geometries contained by this <see cref="GeometryCollection" />.
        /// </summary>
        public virtual int Count
        {
            get
            {
                return _geometries.Length;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override DimensionType Dimension
        {
            get
            {
                DimensionType dimension = DimensionType.False;
                for (int i = 0; i < _geometries.Length; i++)
                    dimension = (DimensionType)Math.Max((int)dimension, (int)_geometries[i].Dimension);
                return dimension;
            }
        }

        /// <summary>
        /// Gets the Envelope that envelops this GeometryCollection
        /// </summary>
        public new IEnvelope Envelope
        {
            get
            {
                IEnvelope env = new Envelope();
                // Enlarge the envelope to include all of the smaller envelopes
                for (int i = 0; i < NumGeometries; i++)
                {
                    env.ExpandToInclude(Geometries[i].Envelope);
                }
                return env;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual IGeometry[] Geometries
        {
            get { return _geometries; }
           protected set { _geometries = value; }
        }

        /// <summary>  
        /// Returns the name of this object's interface.
        /// </summary>
        /// <returns>"GeometryCollection"</returns>
        public override string GeometryType
        {
            get
            {
                return "GeometryCollection";
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override bool IsEmpty
        {
            get
            {
                for (int i = 0; i < _geometries.Length; i++)
                    if (!_geometries[i].IsEmpty)
                        return false;
                return true;
            }
        }

        /// <summary>
        /// Return <c>true</c> if all features in collection are of the same type.
        /// </summary>
        public bool IsHomogeneous
        {
            get
            {
                IGeometry baseGeom = _geometries[0];
                for (int i = 1; i < _geometries.Length; i++)
                    if (baseGeom.GetType() != _geometries[i].GetType())
                        return false;
                return true;
            }
        }

        /// <summary>  
        /// Returns the length of this <c>GeometryCollection</c>.
        /// </summary>
        public override double Length
        {
            get
            {
                double sum = 0.0;
                for (int i = 0; i < _geometries.Length; i++)
                    sum += (_geometries[i]).Length;
                return sum;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override int NumGeometries
        {
            get
            {
                return _geometries.Length;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override int NumPoints
        {
            get
            {
                int numPoints = 0;
                for (int i = 0; i < _geometries.Length; i++)
                    numPoints += _geometries[i].NumPoints;
                return numPoints;
            }
        }

        /// <summary>
        /// Gets the OGC geometry type
        /// </summary>
        public override OgcGeometryType OgcGeometryType
        {
            get { return OgcGeometryType.GeometryCollection; }
        }

        #endregion

        #region Indexers

        /// <summary>
        /// Returns the iTh element in the collection.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public virtual IGeometry this[int i]
        {
            get
            {
                return _geometries[i];
            }
            set { _geometries[i] = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="filter"></param>
        public override void Apply(ICoordinateFilter filter)
        {
            for (int i = 0; i < _geometries.Length; i++)
                _geometries[i].Apply(filter);
        }

        public override void Apply(ICoordinateSequenceFilter filter)
        {
            if (_geometries.Length == 0)
                return;
            for (int i = 0; i < _geometries.Length; i++)
            {
                ((Geometry)_geometries[i]).Apply(filter);
                if (filter.Done)
                {
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
            for (int i = 0; i < _geometries.Length; i++)
                _geometries[i].Apply(filter);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filter"></param>
        public override void Apply(IGeometryComponentFilter filter)
        {
            filter.Filter(this);
            for (int i = 0; i < _geometries.Length; i++)
                _geometries[i].Apply(filter);
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
            Coordinate closest = null;
            double dist = double.MaxValue;
            foreach (IGeometry geometry in Geometries)
            {
                Coordinate temp = geometry.ClosestPoint(testPoint);
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
            var theseElements = new List<IGeometry>(_geometries);
            var otherElements = new List<IGeometry>(((GeometryCollection) o)._geometries);
            return Compare(theseElements, otherElements);
        }

        protected internal override int CompareToSameClass(object o, IComparer<ICoordinateSequence> comp)
        {
            IGeometryCollection gc = (IGeometryCollection) o;

            int n1 = NumGeometries;
            int n2 = gc.NumGeometries;
            int i = 0;
            while (i < n1 && i < n2)
            {
                IGeometry thisGeom = GetGeometryN(i);
                Assert.IsTrue(thisGeom is Geometry);
                IGeometry otherGeom = gc.GetGeometryN(i);
                int holeComp = ((Geometry) thisGeom).CompareToSameClass(otherGeom, comp);
                if (holeComp != 0) return holeComp;
                i++;
            }
            if (i < n1) return 1;
            if (i < n2) return -1;
            return 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override IEnvelope ComputeEnvelopeInternal()
        {
            Envelope envelope = new Envelope();
            for (int i = 0; i < _geometries.Length; i++)
                envelope.ExpandToInclude(_geometries[i].EnvelopeInternal);
            return envelope;
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

            IGeometryCollection otherCollection = (IGeometryCollection) other;
            if (_geometries.Length != otherCollection.Geometries.Length)
                return false;

            for (int i = 0; i < _geometries.Length; i++) 
                if (!_geometries[i].EqualsExact(
                     otherCollection.Geometries[i], tolerance)) 
                        return false;
            return true;
        }

        /// <summary>
        /// This returns the index'th BasicGeometry where index is between 0 and NumGeometries - 1.
        /// If there is only one geometry, this will return this object.
        /// </summary>
        /// <param name="index">An integer index between 0 and NumGeometries - 1 specifying the basic geometry</param>
        /// <returns>A BasicGeometry representing only the specific sub-geometry specified</returns>
        public override IBasicGeometry GetBasicGeometryN(int index)
        {
            return _geometries[index];
        }

        /// <summary>
        /// Returns a <c>GeometryCollectionEnumerator</c>:
        /// this IEnumerator returns the parent geometry as first element.
        /// In most cases is more useful the code
        /// <c>geometryCollectionInstance.Geometries.GetEnumerator()</c>:
        /// this returns an IEnumerator over geometries composing GeometryCollection.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IGeometry> GetEnumerator()
        {
            return new GeometryCollectionEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public override IGeometry GetGeometryN(int n)
        {
            return _geometries[n];
        }

        public override double[] GetOrdinates(Ordinate ordinate)
        {
            if (IsEmpty)
                return new double[0];

            var result = new double[NumPoints];
            var offset = 0;
            for (var i = 0; i < NumGeometries; i++)
            {
                var geom = GetGeometryN(i);
                var ordinates = geom.GetOrdinates(ordinate);
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
            for (int i = 0; i < _geometries.Length; i++)
                _geometries[i].Normalize();
            Array.Sort(_geometries);
        }

        /// <summary>
        /// Handles the duplication process for geometry collections
        /// </summary>
        protected override void OnCopy(Geometry copy)
        {
            GeometryCollection gc = copy as GeometryCollection;
            if (gc == null) return;
            gc._geometries = new Geometry[_geometries.Length];
            for (int i = 0; i < _geometries.Length; i++)
                gc._geometries[i] = (Geometry)_geometries[i].Clone();
        }
      
	    ///<summary>
        /// Creates a <see cref="IGeometryCollection"/> with
        /// every component reversed.
        /// The order of the components in the collection are not reversed.
        ///</summary>
        /// <returns>A <see cref="IGeometryCollection"/></returns> in the reverse order
        public override IGeometry Reverse()
        {
            int n = _geometries.Length;
            IGeometry[] revGeoms = new IGeometry[n];
            for (int i = 0; i < _geometries.Length; i++)
            {
                revGeoms[i] = _geometries[i].Reverse();
            }
            return Factory.CreateGeometryCollection(revGeoms);
        }

        /// <summary>
        /// Rotates the geometries of the GeometryCollection by the given radian angle around the Origin.
        /// </summary>
        /// <param name="origin">Coordinate the geometries get rotated around.</param>
        /// <param name="radAngle">Rotation angle in radian.</param>
        public override void Rotate(Coordinate origin, Double radAngle)
        {
            foreach (IGeometry geo in Geometries)
            {
                IPolygon poly = geo as IPolygon;
                if (poly != null) { poly.Rotate(origin, radAngle); continue; }

                ILineString ls = geo as ILineString;
                if (ls != null) { ls.Rotate(origin, radAngle); continue; }

                IPoint pt = geo as IPoint;
                if (pt != null) { pt.Rotate(origin, radAngle); continue; }
            }
        }

        #endregion

        #region Classes

        /// <summary>
        /// Iterates over all <c>Geometry</c>'s in a <c>GeometryCollection</c>.
        /// Implements a pre-order depth-first traversal of the <c>GeometryCollection</c>
        /// (which may be nested). The original <c>GeometryCollection</c> is
        /// returned as well (as the first object), as are all sub-collections. It is
        /// simple to ignore the <c>GeometryCollection</c> objects if they are not
        /// needed.
        /// </summary>
        public class Enumerator : IEnumerator
        {
            #region Fields

            /// <summary>
            /// The number of <c>Geometry</c>s in the the <c>GeometryCollection</c>.
            /// </summary>
            private readonly int _max;

            /// <summary>
            /// The <c>GeometryCollection</c> being iterated over.
            /// </summary>
            private readonly IGeometryCollection _parent;

            /// <summary>
            /// Indicates whether or not the first element (the <c>GeometryCollection</c>
            /// ) has been returned.
            /// </summary>
            private bool _atStart;

            /// <summary>
            /// The index of the <c>Geometry</c> that will be returned when <c>next</c>
            /// is called.
            /// </summary>
            private int _index;

            /// <summary>
            /// The iterator over a nested <c>GeometryCollection</c>, or <c>null</c>
            /// if this <c>GeometryCollectionIterator</c> is not currently iterating
            /// over a nested <c>GeometryCollection</c>.
            /// </summary>
            Enumerator _subcollectionEnumerator;

            #endregion

            #region Constructors

            /// <summary>
            /// Constructs an iterator over the given <c>GeometryCollection</c>.
            /// </summary>
            /// <param name="parent">
            /// The collection over which to iterate; also, the first
            /// element returned by the iterator.
            /// </param>
            internal Enumerator(IGeometryCollection parent)
            {
                _parent = parent;
                _atStart = true;
                _index = 0;
                _max = parent.NumGeometries;
            }

            #endregion

            #region Properties

            /// <summary>
            ///
            /// </summary>
            public virtual object Current
            {
                get
                {
                    // the _parent GeometryCollection is the first object returned
                    if (_atStart)
                    {
                        _atStart = false;
                        return _parent;
                    }
                    if (_subcollectionEnumerator != null)
                    {
                        if (_subcollectionEnumerator.MoveNext())
                            return _subcollectionEnumerator.Current;
                        _subcollectionEnumerator = null;
                    }
                    if (_index >= _max)
                        throw new ArgumentOutOfRangeException();

                    IGeometry obj = _parent.GetGeometryN(_index++);
                    if (obj is GeometryCollection)
                    {
                        _subcollectionEnumerator = new Enumerator((GeometryCollection)obj);
                        // there will always be at least one element in the sub-collection
                        return _subcollectionEnumerator.Current;
                    }
                    return obj;
                }
            }

            #endregion

            #region Methods

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            public virtual bool MoveNext()
            {
                if (_atStart)
                    return true;
                if (_subcollectionEnumerator != null)
                {
                    if (_subcollectionEnumerator.MoveNext())
                        return true;
                    _subcollectionEnumerator = null;
                }
                if (_index >= _max)
                    return false;
                return true;
            }

            /// <summary>
            ///
            /// </summary>
            public virtual void Reset()
            {
                _atStart = true;
                _index = 0;
            }

            #endregion
        }

        #endregion
    }
}