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
// *******************************************************************************************************

namespace DotSpatial.Topology
{
    /// <summary>
    /// A Dimensionally Extended Nine-Intersection Model (DE-9IM) matrix. This class
    /// can used to represent both computed DE-9IM's (like 212FF1FF2) as well as
    /// patterns for matching them (like T*T******).
    /// Methods are provided to:
    /// Set and query the elements of the matrix in a convenient fashion
    /// convert to and from the standard string representation (specified in
    /// SFS Section 2.1.13.2).
    /// Test to see if a matrix matches a given pattern string.
    /// For a description of the DE-9IM, see the <see href="http://www.opengis.org/techno/specs.htm"/>OpenGIS Simple Features
    /// Specification for SQL.
    /// </summary>
    public interface IIntersectionMatrix
    {
        /// <summary>
        /// Adds one matrix to another.
        /// Addition is defined by taking the maximum dimension value of each position
        /// in the summand matrices.
        /// </summary>
        /// <param name="im">The matrix to add.</param>
        void Add(IIntersectionMatrix im);

        /// <summary>
        /// Changes the value of one of this <c>IntersectionMatrix</c>s
        /// elements.
        /// </summary>
        /// <param name="row">
        /// The row of this <c>IntersectionMatrix</c>,
        /// indicating the interior, boundary or exterior of the first <c>Geometry</c>
        /// </param>
        /// <param name="column">
        /// The column of this <c>IntersectionMatrix</c>,
        /// indicating the interior, boundary or exterior of the second <c>Geometry</c>
        /// </param>
        /// <param name="dimensionValue">
        /// The new value of the element
        /// </param>
        void Set(LocationType row, LocationType column, DimensionType dimensionValue);

        /// <summary>
        /// Changes the elements of this <c>IntersectionMatrix</c> to the
        /// dimension symbols in <c>dimensionSymbols</c>.
        /// </summary>
        /// <param name="dimensionSymbols">
        /// Nine dimension symbols to which to set this <c>IntersectionMatrix</c>
        /// s elements. Possible values are <c>{T, F, *, 0, 1, 2}</c>
        /// </param>
        void Set(string dimensionSymbols);

        /// <summary>
        /// Changes the specified element to <c>minimumDimensionValue</c> if the
        /// element is less.
        /// </summary>
        /// <param name="row">
        /// The row of this <c>IntersectionMatrix</c>
        ///, indicating the interior, boundary or exterior of the first <c>Geometry</c>.
        /// </param>
        /// <param name="column">
        /// The column of this <c>IntersectionMatrix</c>
        ///, indicating the interior, boundary or exterior of the second <c>Geometry</c>.
        /// </param>
        /// <param name="minimumDimensionValue">
        /// The dimension value with which to compare the
        /// element. The order of dimension values from least to greatest is
        /// <c>True, False, Dontcare, 0, 1, 2</c>.
        /// </param>
        void SetAtLeast(LocationType row, LocationType column, DimensionType minimumDimensionValue);

        /// <summary>
        /// If row >= 0 and column >= 0, changes the specified element to <c>minimumDimensionValue</c>
        /// if the element is less. Does nothing if row is smaller to 0 or column is smaller to 0.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="minimumDimensionValue"></param>
        void SetAtLeastIfValid(LocationType row, LocationType column, DimensionType minimumDimensionValue);

        /// <summary>
        /// For each element in this <c>IntersectionMatrix</c>, changes the
        /// element to the corresponding minimum dimension symbol if the element is
        /// less.
        /// </summary>
        /// <param name="minimumDimensionSymbols">
        /// Nine dimension symbols with which to
        /// compare the elements of this <c>IntersectionMatrix</c>. The
        /// order of dimension values from least to greatest is <c>Dontcare, True, False, 0, 1, 2</c>.
        /// </param>
        void SetAtLeast(string minimumDimensionSymbols);

        /// <summary>
        /// Changes the elements of this <c>IntersectionMatrix</c> to <c>dimensionValue</c>.
        /// </summary>
        /// <param name="dimensionValue">
        /// The dimension value to which to set this <c>IntersectionMatrix</c>
        /// s elements. Possible values <c>True, False, Dontcare, 0, 1, 2}</c>.
        /// </param>
        void SetAll(DimensionType dimensionValue);

        /// <summary>
        /// Returns the value of one of this <c>IntersectionMatrix</c>s
        /// elements.
        /// </summary>
        /// <param name="row">
        /// The row of this <c>IntersectionMatrix</c>, indicating
        /// the interior, boundary or exterior of the first <c>Geometry</c>.
        /// </param>
        /// <param name="column">
        /// The column of this <c>IntersectionMatrix</c>,
        /// indicating the interior, boundary or exterior of the second <c>Geometry</c>.
        /// </param>
        /// <returns>The dimension value at the given matrix position.</returns>
        DimensionType Get(LocationType row, LocationType column);

        /// <summary>
        /// Returns <c>true</c> if this <c>IntersectionMatrix</c> is
        /// FF*FF****.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the two <c>Geometry</c>s related by
        /// this <c>IntersectionMatrix</c> are disjoint.
        /// </returns>
        bool IsDisjoint();

        /// <summary>
        /// Returns <c>true</c> if <c>isDisjoint</c> returns false.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the two <c>Geometry</c>s related by
        /// this <c>IntersectionMatrix</c> intersect.
        /// </returns>
        bool IsIntersects();

        /// <summary>
        /// Returns <c>true</c> if this <c>IntersectionMatrix</c> is
        /// FT*******, F**T***** or F***T****.
        /// </summary>
        /// <param name="dimensionOfGeometryA">The dimension of the first <c>Geometry</c>.</param>
        /// <param name="dimensionOfGeometryB">The dimension of the second <c>Geometry</c>.</param>
        /// <returns>
        /// <c>true</c> if the two <c>Geometry</c>
        /// s related by this <c>IntersectionMatrix</c> touch; Returns false
        /// if both <c>Geometry</c>s are points.
        /// </returns>
        bool IsTouches(DimensionType dimensionOfGeometryA, DimensionType dimensionOfGeometryB);

        /// <summary>
        /// Returns <c>true</c> if this <c>IntersectionMatrix</c> is
        ///  T*T****** (for a point and a curve, a point and an area or a line
        /// and an area) 0******** (for two curves).
        /// </summary>
        /// <param name="dimensionOfGeometryA">The dimension of the first <c>Geometry</c>.</param>
        /// <param name="dimensionOfGeometryB">The dimension of the second <c>Geometry</c>.</param>
        /// <returns>
        /// <c>true</c> if the two <c>Geometry</c>
        /// s related by this <c>IntersectionMatrix</c> cross. For this
        /// function to return <c>true</c>, the <c>Geometry</c>s must
        /// be a point and a curve; a point and a surface; two curves; or a curve
        /// and a surface.
        /// </returns>
        bool IsCrosses(DimensionType dimensionOfGeometryA, DimensionType dimensionOfGeometryB);

        /// <summary>
        /// Returns <c>true</c> if this <c>IntersectionMatrix</c> is
        /// T*F**F***.
        /// </summary>
        /// <returns><c>true</c> if the first <c>Geometry</c> is within the second.</returns>
        bool IsWithin();

        /// <summary>
        /// Returns <c>true</c> if this <c>IntersectionMatrix</c> is
        /// T*****FF*.
        /// </summary>
        /// <returns><c>true</c> if the first <c>Geometry</c> contains the second.</returns>
        bool IsContains();

        /// <summary>
        /// Returns <c>true</c> if this <c>IntersectionMatrix</c> is <c>T*****FF*</c>
        /// or <c>*T****FF*</c> or <c>***T**FF*</c> or <c>****T*FF*</c>.
        /// </summary>
        /// <returns><c>true</c> if the first <c>Geometry</c> covers the second</returns>
        bool IsCovers();

        /// <summary>
        /// Returns <c>true</c> if this <c>IntersectionMatrix</c> is T*F**FFF*.
        /// </summary>
        /// <param name="dimensionOfGeometryA">The dimension of the first <c>Geometry</c>.</param>
        /// <param name="dimensionOfGeometryB">The dimension of the second <c>Geometry</c>.</param>
        /// <returns>
        /// <c>true</c> if the two <c>Geometry</c>
        /// s related by this <c>IntersectionMatrix</c> are equal; the
        /// <c>Geometry</c>s must have the same dimension for this function
        /// to return <c>true</c>.
        /// </returns>
        bool IsEquals(DimensionType dimensionOfGeometryA, DimensionType dimensionOfGeometryB);

        /// <summary>
        /// Returns <c>true</c> if this <c>IntersectionMatrix</c> is
        ///  T*T***T** (for two points or two surfaces)
        ///  1*T***T** (for two curves).
        /// </summary>
        /// <param name="dimensionOfGeometryA">The dimension of the first <c>Geometry</c>.</param>
        /// <param name="dimensionOfGeometryB">The dimension of the second <c>Geometry</c>.</param>
        /// <returns>
        /// <c>true</c> if the two <c>Geometry</c>
        /// s related by this <c>IntersectionMatrix</c> overlap. For this
        /// function to return <c>true</c>, the <c>Geometry</c>s must
        /// be two points, two curves or two surfaces.
        /// </returns>
        bool IsOverlaps(DimensionType dimensionOfGeometryA, DimensionType dimensionOfGeometryB);

        /// <summary>
        /// Returns whether the elements of this <c>IntersectionMatrix</c>
        /// satisfies the required dimension symbols.
        /// </summary>
        /// <param name="requiredDimensionSymbols">
        /// Nine dimension symbols with which to
        /// compare the elements of this <c>IntersectionMatrix</c>. Possible
        /// values are <c>{T, F, *, 0, 1, 2}</c>.
        /// </param>
        /// <returns>
        /// <c>true</c> if this <c>IntersectionMatrix</c>
        /// matches the required dimension symbols.
        /// </returns>
        bool Matches(string requiredDimensionSymbols);

        /// <summary>
        /// Transposes this IntersectionMatrix.
        /// </summary>
        /// <returns>This <c>IntersectionMatrix</c> as a convenience,</returns>
        IIntersectionMatrix Transpose();

        /// <summary>
        /// Returns a nine-character <c>String</c> representation of this <c>IntersectionMatrix</c>.
        /// </summary>
        /// <returns>
        /// The nine dimension symbols of this <c>IntersectionMatrix</c>
        /// in row-major order.
        /// </returns>
        string ToString();
    }
}