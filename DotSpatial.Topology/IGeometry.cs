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
using System.Xml;

namespace DotSpatial.Topology
{
    /// <summary>
    /// This has the methods and properties associated with any topology
    /// </summary>
    public interface IGeometry : IComparable, IRelate, IOverlay, IBasicGeometry
    {
        #region Methods

        // IClonable should require Clone()

        // IComparable should require Compare, CompareTo

        /// <summary>
        /// Performs an operation with or on this <c>Geometry</c>'s
        /// coordinates. If you are using this method to modify the point, be sure
        /// to call GeometryChanged() afterwards. Notice that you cannot use this
        /// method to
        /// modify this Geometry if its underlying CoordinateSequence's Get method
        /// returns a copy of the Coordinate, rather than the actual Coordinate stored
        /// (if it even stores Coordinates at all).
        /// </summary>
        /// <param name="filter">The filter to apply to this <c>Geometry</c>'s coordinates</param>
        void Apply(ICoordinateFilter filter);

        /// <summary>
        /// Performs an operation with or on this <c>Geometry</c> and its
        /// subelement <c>Geometry</c>s (if any).
        /// Only GeometryCollections and subclasses
        /// have subelement Geometry's.
        /// </summary>
        /// <param name="filter">
        /// The filter to apply to this <c>Geometry</c> (and
        /// its children, if it is a <c>GeometryCollection</c>).
        /// </param>
        void Apply(IGeometryFilter filter);

        /// <summary>
        /// Performs an operation with or on this Geometry and its
        /// component Geometry's. Only GeometryCollections and
        /// Polygons have component Geometry's; for Polygons they are the LinearRings
        /// of the shell and holes.
        /// </summary>
        /// <param name="filter">The filter to apply to this <c>Geometry</c>.</param>
        void Apply(IGeometryComponentFilter filter);

        /// <summary>
        /// Returns a buffer region around this <c>Geometry</c> having the given width.
        /// The buffer of a Geometry is the Minkowski sum or difference of the Geometry with a disc of radius <c>distance</c>.
        /// </summary>
        /// <param name="distance">
        /// The width of the buffer, interpreted according to the
        /// <c>PrecisionModel</c> of the <c>Geometry</c>.
        /// </param>
        /// <returns>
        /// All points whose distance from this <c>Geometry</c>
        /// are less than or equal to <c>distance</c>.
        /// </returns>
        IGeometry Buffer(double distance);

        /// Buffer
        /// <summary>
        /// Returns a buffer region around this <c>Geometry</c> having the given width.
        /// The buffer of a Geometry is the Minkowski sum or difference of the Geometry with a disc of radius <c>distance</c>.
        /// </summary>
        /// <param name="distance">
        /// The width of the buffer, interpreted according to the
        /// <c>PrecisionModel</c> of the <c>Geometry</c>.
        /// </param>
        /// <param name="endCapStyle">Cap Style to use for compute buffer.</param>
        /// <returns>
        /// All points whose distance from this <c>Geometry</c>
        /// are less than or equal to <c>distance</c>.
        /// </returns>
        IGeometry Buffer(double distance, BufferStyle endCapStyle);

        /// Buffer
        /// <summary>
        /// Returns a buffer region around this <c>Geometry</c> having the given
        /// width and with a specified number of segments used to approximate curves.
        /// The buffer of a Geometry is the Minkowski sum of the Geometry with
        /// a disc of radius <c>distance</c>.  Curves in the buffer polygon are
        /// approximated with line segments.  This method allows specifying the
        /// accuracy of that approximation.
        /// </summary>
        /// <param name="distance">
        /// The width of the buffer, interpreted according to the
        /// <c>PrecisionModel</c> of the <c>Geometry</c>.
        /// </param>
        /// <param name="quadrantSegments">The number of segments to use to approximate a quadrant of a circle.</param>
        /// <returns>
        /// All points whose distance from this <c>Geometry</c>
        /// are less than or equal to <c>distance</c>.
        /// </returns>
        IGeometry Buffer(double distance, int quadrantSegments);

        /// Buffer
        /// <summary>
        /// Returns a buffer region around this <c>Geometry</c> having the given
        /// width and with a specified number of segments used to approximate curves.
        /// The buffer of a Geometry is the Minkowski sum of the Geometry with
        /// a disc of radius <c>distance</c>.  Curves in the buffer polygon are
        /// approximated with line segments.  This method allows specifying the
        /// accuracy of that approximation.
        /// </summary>
        /// <param name="distance">
        /// The width of the buffer, interpreted according to the
        /// <c>PrecisionModel</c> of the <c>Geometry</c>.
        /// </param>
        /// <param name="quadrantSegments">The number of segments to use to approximate a quadrant of a circle.</param>
        /// <param name="endCapStyle">Cap Style to use for compute buffer.</param>
        /// <returns>
        /// All points whose distance from this <c>Geometry</c>
        /// are less than or equal to <c>distance</c>.
        /// </returns>
        IGeometry Buffer(double distance, int quadrantSegments, BufferStyle endCapStyle);

        /// <summary>
        /// Clears any cached envelopes
        /// </summary>
        void ClearEnvelope();

        /// <summary>
        /// Given the specified test point, this returns the closest point in this geometry.
        /// </summary>
        /// <param name="testPoint"></param>
        /// <returns></returns>
        Coordinate ClosestPoint(Coordinate testPoint);

        /// ConvexHull
        /// <summary>
        /// Returns the smallest convex Polygon that contains all the points in the Geometry. This obviously applies only to Geometry s which contain 3 or more points.
        /// </summary>
        /// <returns>the minimum-area convex polygon containing this Geometry's points.</returns>
        IGeometry ConvexHull();

        /// <summary>
        /// Returns whether this <c>Geometry</c> is greater than, equal to,
        /// or less than another <c>Geometry</c> having the same class.
        /// </summary>
        /// <param name="o">A <c>Geometry</c> having the same class as this <c>Geometry</c>.</param>
        /// <returns>
        /// A positive number, 0, or a negative number, depending on whether
        /// this object is greater than, equal to, or less than <c>o</c>, as
        /// defined in "Normal Form For Geometry" in the NTS Technical
        /// Specifications.
        /// </returns>
        int CompareToSameClass(object o);

        /// Distance
        /// <summary>
        /// Returns the minimum distance between this <c>Geometry</c>
        /// and the <c>Geometry</c> g.
        /// </summary>
        /// <param name="geom">The <c>Geometry</c> from which to compute the distance.</param>
        double Distance(IGeometry geom);

        /// Equals
        /// <summary>
        /// Returns <c>true</c> if the DE-9IM intersection matrix for the two
        /// <c>Geometry</c>s is T*F**FFF*.
        /// </summary>
        /// <param name="geom">The <c>Geometry</c> with which to compare this <c>Geometry</c>.</param>
        /// <returns><c>true</c> if the two <c>Geometry</c>s are equal.</returns>
        bool Equals(IGeometry geom);

        /// EqualsExact
        /// <summary>
        /// Returns true if the two <c>Geometry</c>s are exactly equal,
        /// up to a specified tolerance.
        /// Two Geometries are exactly within a tolerance equal iff:
        /// they have the same class,
        /// they have the same values of Coordinates,
        /// within the given tolerance distance, in their internal
        /// Coordinate lists, in exactly the same order.
        /// If this and the other <c>Geometry</c>s are
        /// composites and any children are not <c>Geometry</c>s, returns
        /// false.
        /// </summary>
        /// <param name="geom">The <c>Geometry</c> with which to compare this <c>Geometry</c></param>
        /// <param name="tolerance">Distance at or below which two Coordinates will be considered equal.</param>
        /// <returns>
        /// <c>true</c> if this and the other <c>Geometry</c>
        /// are of the same class and have equal internal data.
        /// </returns>
        bool EqualsExact(IGeometry geom, double tolerance);

        /// EqualsExact
        /// <summary>
        /// Returns true if the two <c>Geometry</c>s are exactly equal.
        /// Two Geometries are exactly equal iff:
        /// they have the same class,
        /// they have the same values of Coordinates in their internal
        /// Coordinate lists, in exactly the same order.
        /// If this and the other <c>Geometry</c>s are
        /// composites and any children are not <c>Geometry</c>s, returns
        /// false.
        /// This provides a stricter test of equality than <c>equals</c>.
        /// </summary>
        /// <param name="geom">The <c>Geometry</c> with which to compare this <c>Geometry</c>.</param>
        /// <returns>
        /// <c>true</c> if this and the other <c>Geometry</c>
        /// are of the same class and have equal internal data.
        /// </returns>
        bool EqualsExact(IGeometry geom);

        /// <summary>
        /// Notifies this Geometry that its Coordinates have been changed by an external
        /// party. When GeometryChanged is called, this method will be called for
        /// this Geometry and its component Geometries.
        /// </summary>
        void GeometryChangedAction();

        /// <summary>
        /// Notifies this Geometry that its Coordinates have been changed by an external
        /// party (using a CoordinateFilter, for example). The Geometry will flush
        /// and/or update any information it has cached (such as its Envelope).
        /// </summary>
        void GeometryChanged();

        /// GetGeometryN
        /// <summary>
        /// Returns an element Geometry from a GeometryCollection,
        /// or <code>this</code>, if the geometry is not a collection.
        /// </summary>
        /// <param name="n">The index of the geometry element.</param>
        /// <returns>The n'th geometry contained in this geometry.</returns>
        IGeometry GetGeometryN(int n);

        /// IsWithinDistance
        /// <summary>
        /// Tests whether the distance from this <c>Geometry</c>
        /// to another is less than or equal to a specified value.
        /// </summary>
        /// <param name="geom">the Geometry to check the distance to.</param>
        /// <param name="distance">the distance value to compare.</param>
        /// <returns><c>true</c> if the geometries are less than <c>distance</c> apart.</returns>
        bool IsWithinDistance(IGeometry geom, double distance);

        /// Normalize
        /// <summary>
        /// Converts this <c>Geometry</c> to normal form (or
        /// canonical form ). Normal form is a unique representation for <c>Geometry</c>
        /// s. It can be used to test whether two <c>Geometry</c>s are equal
        /// in a way that is independent of the ordering of the coordinates within
        /// them. Normal form equality is a stronger condition than topological
        /// equality, but weaker than pointwise equality. The definitions for normal
        /// form use the standard lexicographical ordering for coordinates. "Sorted in
        /// order of coordinates" means the obvious extension of this ordering to
        /// sequences of coordinates.
        /// </summary>
        void Normalize();

        /// <summary>
        /// Returns the feature representation as GML 2.1.1 XML document.
        /// This XML document is based on <c>Geometry.xsd</c> schema.
        /// NO features or XLink are implemented here!
        /// </summary>
        XmlReader ToGmlFeature();

        #endregion

        #region Properties

        /// Area
        /// <summary>
        /// Returns the area of this Geometry.
        /// Areal Geometries have a non-zero area.
        /// They override this function to compute the area.
        /// Others return 0.0
        /// </summary>
        /// <returns>The area of the Geometry.</returns>
        double Area { get; }

        /// Boundary
        /// <summary>
        /// Returns the boundary, or the empty point if this <c>Geometry</c>
        /// is empty. For a discussion of this function, see the OpenGIS Simple
        /// Features Specification. As stated in SFS Section 2.1.13.1, "the boundary
        /// of a Geometry is a set of Geometries of the next lower dimension."
        /// </summary>
        /// <returns>The closure of the combinatorial boundary of this <c>Geometry</c>.</returns>
        IGeometry Boundary { get; set; }

        /// <summary>
        /// Returns the dimension of this <c>Geometry</c>s inherent boundary.
        /// </summary>
        /// <returns>
        /// The dimension of the boundary of the class implementing this
        /// interface, whether or not this object is the empty point. Returns
        /// <c>Dimension.False</c> if the boundary is the empty point.
        /// </returns>
        DimensionType BoundaryDimension { get; set; }

        /// Centroid
        /// <summary>
        /// Computes the centroid of this Geometry.
        /// The centroid is equal to the centroid of the set of component Geometries of highest
        /// dimension (since the lower-dimension geometries contribute zero "weight" to the centroid).
        /// </summary>
        /// <returns>A Point which is the centroid of this Geometry.</returns>
        IPoint Centroid { get; }

        /// Coordinate
        /// <summary>
        /// Returns a vertex of this Geometry
        /// </summary>
        Coordinate Coordinate { get; }

        /// Dimension
        /// <summary>
        /// Gets or sets the DotSpatial.Geometries.Dimensions of this Geometry.
        /// </summary>
        DimensionType Dimension { get; set; }

        /// EnvelopeInternal
        /// <summary>
        /// Returns the interior geometry
        /// </summary>
        IEnvelope EnvelopeInternal { get; }

        /// Factory
        /// <summary>
        /// Gets the factory which contains the context in whcih this geometry was created
        /// </summary>
        IGeometryFactory Factory { get; }

        /// InteriorPoint
        /// <summary>
        /// Computes an interior point of this Geometry.  An interior point is guaranteed
        /// to lie in the interior of the Geometry, if it possible to calculate such a point
        /// exactly. Otherwise, the point may lie on the boundary of the point.
        /// </summary>
        IGeometry InteriorPoint { get; }

        /// IsEmpty
        /// <summary>
        /// Returns whether or not the set of points in this geometry is empty
        /// </summary>
        bool IsEmpty { get; }

        /// IsRectangle
        /// <summary>
        /// Essentially is false for anything other than a polygon, which
        /// does a check to see if the polygon in question is a rectangle.
        /// </summary>
        bool IsRectangle { get; }

        /// IsSimple
        /// <summary>
        /// Returns false if the Geometry not simple.  Subclasses provide their own definition
        /// of "simple". If this Geometry is empty, returns true. In general, the SFS specifications
        /// of simplicity seem to follow the following rule: A Geometry is simple if the only
        /// self-intersections are at boundary points.  For all empty Geometrys, IsSimple==true.
        /// </summary>
        bool IsSimple { get; }

        /// IsValid
        /// <summary>
        /// Tests the validity of this Geometry.  Subclasses provide their own definition of "valid"
        /// </summary>
        bool IsValid { get; }

        /// Length
        /// <summary>
        /// Returns the length of this Geometry.  Linear geometries return their length.
        /// Areal geometries return their perimeter.  Others return 0.0
        /// </summary>
        double Length { get; }

        /// <summary>
        /// There used to be a class for precision model stuff, but for Interface purposes, this will be
        /// communicated as a an enumeration which can later be converted into a full fledged PrecisionModel.
        /// </summary>
        PrecisionModelType PrecisionModel { get; }

        /// SRID
        /// <summary>
        /// Gets/Sets the ID of the Spatial Reference System used by the Geometry. NTS supports Spatial Reference
        /// System information in the simple way defined in the SFS. A Spatial Reference System ID (SRID) is present
        /// in each Geometry object. Geometry provides basic accessor operations for this field, but no others. The SRID
        /// is represented as an integer.
        /// </summary>
        int Srid { get; set; }

        /// UserData
        /// <summary>
        /// Gets/Sets the user data object for this point, if any.  A simple scheme for applications to add their own custom
        /// data to a Geometry.  An example use might be to add an object representing a Coordinate Reference System.
        /// Notice that user data objects are not present in geometries created by construction methods.
        /// </summary>
        object UserData { get; set; }

        #endregion
    }
}