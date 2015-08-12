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
// *********************************************************************************************************

using System;
using System.Text;

namespace DotSpatial.Topology.Geometries
{
    /// <summary>  
    /// Models a <b>Dimensionally Extended Nine-Intersection Model (DE-9IM)</b> matrix. 
    /// </summary>
    /// <remarks>
    /// <para>
    /// DE-9IM matrices (such as "212FF1FF2")
    /// specify the topological relationship between two <see cref="IGeometry"/>s. 
    /// This class can also represent matrix patterns (such as "T*T******")
    /// which are used for matching instances of DE-9IM matrices.
    /// </para>
    /// <para>
    /// Methods are provided to:
    /// <list type="Bullet">
    /// <item>Set and query the elements of the matrix in a convenient fashion.</item>
    /// <item>Convert to and from the standard string representation (specified in SFS Section 2.1.13.2).</item>
    /// <item>Test to see if a matrix matches a given pattern string.</item>
    /// </list>
    /// </para>
    /// For a description of the DE-9IM and the spatial predicates derived from it, see the <i>
    /// <see href="http://www.opengis.org/techno/specs.htm">OGC 99-049 OpenGIS Simple Features Specification for SQL.</see></i> as well as 
    /// <i>OGC 06-103r4 OpenGIS 
    /// Implementation Standard for Geographic information - 
    /// Simple feature access - Part 1: Common architecture</i>
    /// (which provides some further details on certain predicate specifications).
    /// <para>
    /// The entries of the matrix are defined by the constants in the <see cref="Dimension"/> enum.
    /// The indices of the matrix represent the topological locations 
    /// that occur in a geometry (Interior, Boundary, Exterior).  
    /// These are provided as constants in the <see cref="Location"/> enum.
    /// </para>
    /// </remarks>
    public class IntersectionMatrix : IIntersectionMatrix
    {
        #region Fields

        /// <summary>  
        /// Internal representation of this <see cref="IntersectionMatrix" />.
        /// </summary>
        private readonly DimensionType[,] _matrix;

        #endregion

        #region Constructors

        /// <summary>  
        /// Creates an <see cref="IntersectionMatrix" /> with <c>Null</c> location values.
        /// </summary>
        public IntersectionMatrix()
        {
            _matrix = new DimensionType[3, 3];
            SetAll(DimensionType.False);
        }

        /// <summary>
        /// Creates an <see cref="IntersectionMatrix" /> with the given dimension
        /// symbols.
        /// </summary>
        /// <param name="elements">A string of nine dimension symbols in row major order.</param>
        public IntersectionMatrix(string elements)
            : this()
        {
            Set(elements);
        }

        /// <summary> 
        /// Creates an <see cref="IntersectionMatrix" /> with the same elements as
        /// <c>other</c>.
        /// </summary>
        /// <param name="other">An <see cref="IntersectionMatrix" /> to copy.</param>         
        public IntersectionMatrix(IntersectionMatrix other)
            : this()
        {
            _matrix[(int)LocationType.Interior, (int)LocationType.Interior] = other._matrix[(int)LocationType.Interior, (int)LocationType.Interior];
            _matrix[(int)LocationType.Interior, (int)LocationType.Boundary] = other._matrix[(int)LocationType.Interior, (int)LocationType.Boundary];
            _matrix[(int)LocationType.Interior, (int)LocationType.Exterior] = other._matrix[(int)LocationType.Interior, (int)LocationType.Exterior];
            _matrix[(int)LocationType.Boundary, (int)LocationType.Interior] = other._matrix[(int)LocationType.Boundary, (int)LocationType.Interior];
            _matrix[(int)LocationType.Boundary, (int)LocationType.Boundary] = other._matrix[(int)LocationType.Boundary, (int)LocationType.Boundary];
            _matrix[(int)LocationType.Boundary, (int)LocationType.Exterior] = other._matrix[(int)LocationType.Boundary, (int)LocationType.Exterior];
            _matrix[(int)LocationType.Exterior, (int)LocationType.Interior] = other._matrix[(int)LocationType.Exterior, (int)LocationType.Interior];
            _matrix[(int)LocationType.Exterior, (int)LocationType.Boundary] = other._matrix[(int)LocationType.Exterior, (int)LocationType.Boundary];
            _matrix[(int)LocationType.Exterior, (int)LocationType.Exterior] = other._matrix[(int)LocationType.Exterior, (int)LocationType.Exterior];
        }

        #endregion

        #region Indexers

        /// <summary>
        /// See methods Get(int, int) and Set(int, int, int value)
        /// </summary>
        public DimensionType this[LocationType row, LocationType column]
        {
            get
            {
                return Get(row, column);
            }
            set
            {
                Set(row, column, value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds one matrix to another.
        /// Addition is defined by taking the maximum dimension value of each position
        /// in the summand matrices.
        /// </summary>
        /// <param name="im">The matrix to add.</param>
        public virtual void Add(IIntersectionMatrix im)
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    SetAtLeast((LocationType)i, (LocationType)j, im.Get((LocationType)i, (LocationType)j));
        }

        /// <summary>
        /// Returns the value of one of this <see cref="IntersectionMatrix" />s
        /// elements.
        /// </summary>
        /// <param name="row">
        /// The row of this <see cref="IntersectionMatrix" />, indicating
        /// the interior, boundary or exterior of the first <see cref="IGeometry"/>.
        /// </param>
        /// <param name="column">
        /// The column of this <see cref="IntersectionMatrix" />,
        /// indicating the interior, boundary or exterior of the second <see cref="IGeometry"/>.
        /// </param>
        /// <returns>The dimension value at the given matrix position.</returns>
        public virtual DimensionType Get(LocationType row, LocationType column)
        {
            return _matrix[(int)row, (int)column];
        }

        /// <summary> 
        /// Returns <c>true</c> if this <see cref="IntersectionMatrix" /> is
        /// T*****FF*.
        /// </summary>
        /// <returns><c>true</c> if the first <see cref="IGeometry"/> contains the second.</returns>
        public virtual bool IsContains()
        {
            return Matches(_matrix[(int)LocationType.Interior, (int)LocationType.Interior], 'T') &&
                    _matrix[(int)LocationType.Exterior, (int)LocationType.Interior] == DimensionType.False &&
                    _matrix[(int)LocationType.Exterior, (int)LocationType.Boundary] == DimensionType.False;
        }

        /// <summary>
        /// Returns <c>true</c> if this <see cref="IntersectionMatrix" /> is <c>T*F**F***</c>
        /// or <c>*TF**F***</c> or <c>**FT*F***</c> or <c>**F*TF***</c>
        /// </summary>
        /// <returns><c>true</c> if the first <see cref="IGeometry"/> is covered by the second</returns>
        public bool IsCoveredBy()
        {
            bool hasPointInCommon = Matches(_matrix[(int)LocationType.Interior, (int)LocationType.Interior], 'T')
                                    || Matches(_matrix[(int)LocationType.Interior, (int)LocationType.Boundary], 'T')
                                    || Matches(_matrix[(int)LocationType.Boundary, (int)LocationType.Interior], 'T')
                                    || Matches(_matrix[(int)LocationType.Boundary, (int)LocationType.Boundary], 'T');

            return hasPointInCommon &&
                _matrix[(int)LocationType.Interior, (int)LocationType.Exterior] == DimensionType.False &&
                _matrix[(int)LocationType.Boundary, (int)LocationType.Exterior] == DimensionType.False;
        }

        /// <summary>
        /// Returns <c>true</c> if this <see cref="IntersectionMatrix" /> is <c>T*****FF*</c>
        /// or <c>*T****FF*</c> or <c>***T**FF*</c> or <c>****T*FF*</c>.
        /// </summary>
        /// <returns><c>true</c> if the first <see cref="IGeometry"/> covers the second</returns>
        public virtual bool IsCovers()
        {
            bool hasPointInCommon = IsTrue(_matrix[(int)LocationType.Interior, (int)LocationType.Interior])
                                    || IsTrue(_matrix[(int)LocationType.Interior, (int)LocationType.Boundary])
                                    || IsTrue(_matrix[(int)LocationType.Boundary, (int)LocationType.Interior])
                                    || IsTrue(_matrix[(int)LocationType.Boundary, (int)LocationType.Boundary]);

            return hasPointInCommon &&
                    _matrix[(int)LocationType.Exterior, (int)LocationType.Interior] == DimensionType.False &&
                    _matrix[(int)LocationType.Exterior, (int)LocationType.Boundary] == DimensionType.False;
        }

        /// <summary>
        /// Returns <c>true</c> if this <see cref="IntersectionMatrix" /> is
        ///  T*T****** (for a point and a curve, a point and an area or a line
        /// and an area) 0******** (for two curves).
        /// </summary>
        /// <param name="dimensionOfGeometryA">The dimension of the first <see cref="IGeometry"/>.</param>
        /// <param name="dimensionOfGeometryB">The dimension of the second <see cref="IGeometry"/>.</param>
        /// <returns>
        /// <c>true</c> if the two <see cref="IGeometry"/>
        /// s related by this <see cref="IntersectionMatrix" /> cross. For this
        /// function to return <c>true</c>, the <see cref="IGeometry"/>s must
        /// be a point and a curve; a point and a surface; two curves; or a curve
        /// and a surface.
        /// </returns>
        public virtual bool IsCrosses(DimensionType dimensionOfGeometryA, DimensionType dimensionOfGeometryB)
        {
            if ((dimensionOfGeometryA == DimensionType.Point && dimensionOfGeometryB == DimensionType.Curve) ||
                (dimensionOfGeometryA == DimensionType.Point && dimensionOfGeometryB == DimensionType.Surface) ||
                (dimensionOfGeometryA == DimensionType.Curve && dimensionOfGeometryB == DimensionType.Surface))
                return IsTrue(_matrix[(int)LocationType.Interior, (int)LocationType.Interior]) &&
                       IsTrue(_matrix[(int)LocationType.Interior, (int)LocationType.Exterior]);

            if ((dimensionOfGeometryA == DimensionType.Curve && dimensionOfGeometryB == DimensionType.Point) ||
                (dimensionOfGeometryA == DimensionType.Surface && dimensionOfGeometryB == DimensionType.Point) ||
                (dimensionOfGeometryA == DimensionType.Surface && dimensionOfGeometryB == DimensionType.Curve))
                return IsTrue(_matrix[(int)LocationType.Interior, (int)LocationType.Interior]) &&
                       IsTrue(_matrix[(int)LocationType.Exterior, (int)LocationType.Interior]);

            if (dimensionOfGeometryA == DimensionType.Curve && dimensionOfGeometryB == DimensionType.Curve)
                return _matrix[(int)LocationType.Interior, (int)LocationType.Interior] == 0;

            return false;
        }

        /// <summary>
        /// Returns <c>true</c> if this <see cref="IntersectionMatrix" /> is FF*FF****.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the two <see cref="IGeometry"/>'s related by
        /// this <see cref="IntersectionMatrix" /> are disjoint.
        /// </returns>
        public virtual bool IsDisjoint()
        {
            return
                _matrix[(int)LocationType.Interior, (int)LocationType.Interior] == DimensionType.False &&
                _matrix[(int)LocationType.Interior, (int)LocationType.Boundary] == DimensionType.False &&
                _matrix[(int)LocationType.Boundary, (int)LocationType.Interior] == DimensionType.False &&
                _matrix[(int)LocationType.Boundary, (int)LocationType.Boundary] == DimensionType.False;
        }

        /// <summary> 
        /// Tests whether the argument dimensions are equal and 
        /// this <c>IntersectionMatrix</c> matches
        /// the pattern <tt>T*F**FFF*</tt>.
        /// <para/>
        /// <b>Note:</b> This pattern differs from the one stated in 
        /// <i>Simple feature access - Part 1: Common architecture</i>.
        /// That document states the pattern as <tt>TFFFTFFFT</tt>.  This would
        /// specify that
        /// two identical <tt>POINT</tt>s are not equal, which is not desirable behaviour.
        /// The pattern used here has been corrected to compute equality in this situation.
        /// </summary>
        /// <param name="dimensionOfGeometryA">The dimension of the first <see cref="IGeometry"/>.</param>
        /// <param name="dimensionOfGeometryB">The dimension of the second <see cref="IGeometry"/>.</param>
        /// <returns>
        /// <c>true</c> if the two <see cref="IGeometry"/>s
        /// related by this <see cref="IntersectionMatrix" /> are equal; the
        /// <see cref="IGeometry"/>s must have the same dimension to be equal.
        /// </returns>
        public virtual bool IsEquals(DimensionType dimensionOfGeometryA, DimensionType dimensionOfGeometryB)
        {
            if (dimensionOfGeometryA != dimensionOfGeometryB)
                return false;

            return IsTrue(_matrix[(int)LocationType.Interior, (int)LocationType.Interior]) &&
                   _matrix[(int)LocationType.Interior, (int)LocationType.Exterior] == DimensionType.False &&
                   _matrix[(int)LocationType.Boundary, (int)LocationType.Exterior] == DimensionType.False &&
                   _matrix[(int)LocationType.Exterior, (int)LocationType.Interior] == DimensionType.False &&
                   _matrix[(int)LocationType.Exterior, (int)LocationType.Boundary] == DimensionType.False;
        }

        /// <summary>
        /// Returns <c>true</c> if <c>isDisjoint</c> returns false.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the two <see cref="IGeometry"/>'s related by
        /// this <see cref="IntersectionMatrix" /> intersect.
        /// </returns>
        public virtual bool IsIntersects()
        {
            return !IsDisjoint();
        }

        /// <summary>
        /// Returns <c>true</c> if this <see cref="IntersectionMatrix" /> is
        ///  T*T***T** (for two points or two surfaces)
        ///  1*T***T** (for two curves).
        /// </summary>
        /// <param name="dimensionOfGeometryA">The dimension of the first <see cref="IGeometry"/>.</param>
        /// <param name="dimensionOfGeometryB">The dimension of the second <see cref="IGeometry"/>.</param>
        /// <returns>
        /// <c>true</c> if the two <see cref="IGeometry"/>
        /// s related by this <see cref="IntersectionMatrix" /> overlap. For this
        /// function to return <c>true</c>, the <see cref="IGeometry"/>s must
        /// be two points, two curves or two surfaces.
        /// </returns>
        public virtual bool IsOverlaps(DimensionType dimensionOfGeometryA, DimensionType dimensionOfGeometryB)
        {
            if ((dimensionOfGeometryA == DimensionType.Point && dimensionOfGeometryB == DimensionType.Point) ||
                (dimensionOfGeometryA == DimensionType.Surface && dimensionOfGeometryB == DimensionType.Surface))
                return IsTrue(_matrix[(int)LocationType.Interior, (int)LocationType.Interior]) &&
                       IsTrue(_matrix[(int)LocationType.Interior, (int)LocationType.Exterior]) &&
                       IsTrue(_matrix[(int)LocationType.Exterior, (int)LocationType.Interior]);

            if (dimensionOfGeometryA == DimensionType.Curve && dimensionOfGeometryB == DimensionType.Curve)
                return _matrix[(int)LocationType.Interior, (int)LocationType.Interior] == DimensionType.Curve &&
                       IsTrue(_matrix[(int)LocationType.Interior, (int)LocationType.Exterior]) &&
                       IsTrue(_matrix[(int)LocationType.Exterior, (int)LocationType.Interior]);

            return false;
        }

        /// <summary>
        /// Returns <c>true</c> if this <see cref="IntersectionMatrix" /> is
        /// FT*******, F**T***** or F***T****.
        /// </summary>
        /// <param name="dimensionOfGeometryA">The dimension of the first <see cref="IGeometry"/>.</param>
        /// <param name="dimensionOfGeometryB">The dimension of the second <see cref="IGeometry"/>.</param>
        /// <returns>
        /// <c>true</c> if the two <see cref="IGeometry"/>
        /// s related by this <see cref="IntersectionMatrix" /> touch; Returns false
        /// if both <see cref="IGeometry"/>s are points.
        /// </returns>
        public virtual bool IsTouches(DimensionType dimensionOfGeometryA, DimensionType dimensionOfGeometryB)
        {
            if (dimensionOfGeometryA > dimensionOfGeometryB)
                //no need to get transpose because pattern matrix is symmetrical
                return IsTouches(dimensionOfGeometryB, dimensionOfGeometryA);

            if ((dimensionOfGeometryA == DimensionType.Surface && dimensionOfGeometryB == DimensionType.Surface) ||
                (dimensionOfGeometryA == DimensionType.Curve && dimensionOfGeometryB == DimensionType.Curve) ||
                (dimensionOfGeometryA == DimensionType.Curve && dimensionOfGeometryB == DimensionType.Surface) ||
                (dimensionOfGeometryA == DimensionType.Point && dimensionOfGeometryB == DimensionType.Surface) ||
                (dimensionOfGeometryA == DimensionType.Point && dimensionOfGeometryB == DimensionType.Curve))
                return _matrix[(int)LocationType.Interior, (int)LocationType.Interior] == DimensionType.False &&
                        (IsTrue(_matrix[(int)LocationType.Interior, (int)LocationType.Boundary]) ||
                         IsTrue(_matrix[(int)LocationType.Boundary, (int)LocationType.Interior]) ||
                         IsTrue(_matrix[(int)LocationType.Boundary, (int)LocationType.Boundary]));

            return false;
        }

        /// <summary>
        /// Tests if the dimension value matches <tt>TRUE</tt>
        /// (i.e.  has value 0, 1, 2 or TRUE).
        /// </summary>
        /// <param name="actualDimensionValue">A number that can be stored in the <c>IntersectionMatrix</c>.
        /// Possible values are <c>{<see cref="DimensionType.True"/>, <see cref="DimensionType.False"/>, <see cref="DimensionType.Dontcare"/>, 
        /// <see cref="DimensionType.Point"/>, <see cref="DimensionType.Curve"/>, <see cref="DimensionType.Surface"/>}</c></param>
        /// <returns><c>true</c> if the dimension value matches <see cref="DimensionType.True"/></returns>
        public static bool IsTrue(DimensionType actualDimensionValue)
        {
            if (actualDimensionValue >= 0 || actualDimensionValue == DimensionType.True)
            {
                return true;
            }
            return false;
        }

        /// <summary>  
        /// Returns <c>true</c> if this <see cref="IntersectionMatrix" /> is
        /// T*F**F***.
        /// </summary>
        /// <returns><c>true</c> if the first <see cref="IGeometry"/> is within the second.</returns>
        public virtual bool IsWithin()
        {
            return IsTrue(_matrix[(int)LocationType.Interior, (int)LocationType.Interior]) &&
                   _matrix[(int)LocationType.Interior, (int)LocationType.Exterior] == DimensionType.False &&
                   _matrix[(int)LocationType.Boundary, (int)LocationType.Exterior] == DimensionType.False;
        }

        /// <summary>  
        /// Tests if the dimension value satisfies the dimension symbol.
        /// </summary>
        /// <param name="actualDimensionValue">
        /// a number that can be stored in the <c>IntersectionMatrix</c>.
        /// Possible values are <c>{True, False, Dontcare, 0, 1, 2}</c>.
        /// </param>
        /// <param name="requiredDimensionSymbol">
        /// A character used in the string
        /// representation of an <see cref="IntersectionMatrix" />. 
        /// Possible values are <c>T, F, * , 0, 1, 2</c>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the dimension symbol encompasses the dimension value.        
        /// </returns>        
        public static bool Matches(DimensionType actualDimensionValue, char requiredDimensionSymbol)
        {
            if (requiredDimensionSymbol == Dimension.SymDontcare)
                return true;
            if (requiredDimensionSymbol == Dimension.SymTrue && (actualDimensionValue >= DimensionType.Point || actualDimensionValue == DimensionType.True))
                return true;
            if (requiredDimensionSymbol == Dimension.SymFalse && actualDimensionValue == DimensionType.False)
                return true;
            if (requiredDimensionSymbol == Dimension.SymP && actualDimensionValue == DimensionType.Point)
                return true;
            if (requiredDimensionSymbol == Dimension.SymL && actualDimensionValue == DimensionType.Curve)
                return true;
            if (requiredDimensionSymbol == Dimension.SymA && actualDimensionValue == DimensionType.Surface)
                return true;
            return false;
        }

        /// <summary>
        /// Tests if each of the actual dimension symbols in a matrix string satisfies the
        /// corresponding required dimension symbol in a pattern string.
        /// </summary>
        /// <param name="actualDimensionSymbols">
        /// Nine dimension symbols to validate.
        /// Possible values are <c>T, F, * , 0, 1, 2</c>.
        /// </param>
        /// <param name="requiredDimensionSymbols">
        /// Nine dimension symbols to validate
        /// against. Possible values are <c>T, F, * , 0, 1, 2</c>.
        /// </param>
        /// <returns>
        /// <c>true</c> if each of the required dimension
        /// symbols encompass the corresponding actual dimension symbol.
        /// </returns>
        public static bool Matches(string actualDimensionSymbols, string requiredDimensionSymbols)
        {
            IntersectionMatrix m = new IntersectionMatrix(actualDimensionSymbols);
            return m.Matches(requiredDimensionSymbols);
        }

        /// <summary> 
        /// Returns whether the elements of this <see cref="IntersectionMatrix" />
        /// satisfies the required dimension symbols.
        /// </summary>
        /// <param name="requiredDimensionSymbols"> 
        /// Nine dimension symbols with which to
        /// compare the elements of this <see cref="IntersectionMatrix" />. Possible
        /// values are <c>{T, F, * , 0, 1, 2}</c>.
        /// </param>
        /// <returns>
        /// <c>true</c> if this <see cref="IntersectionMatrix" />
        /// matches the required dimension symbols.
        /// </returns>
        public virtual bool Matches(string requiredDimensionSymbols)
        {
            if (requiredDimensionSymbols.Length != 9)
                throw new ArgumentException("Should be length 9: " + requiredDimensionSymbols);

            for (int ai = 0; ai < 3; ai++)
                for (int bi = 0; bi < 3; bi++)
                    if (!Matches(_matrix[ai, bi], requiredDimensionSymbols[3 * ai + bi]))
                        return false;
            return true;
        }

        /// <summary>
        /// Changes the value of one of this <see cref="IntersectionMatrix" /> elements.
        /// </summary>
        /// <param name="row">
        /// The row of this <see cref="IntersectionMatrix" />,
        /// indicating the interior, boundary or exterior of the first <see cref="IGeometry"/>
        /// </param>
        /// <param name="column">
        /// The column of this <see cref="IntersectionMatrix" />,
        /// indicating the interior, boundary or exterior of the second <see cref="IGeometry"/>
        /// </param>
        /// <param name="dimensionValue">
        /// The new value of the element
        /// </param>
        public virtual void Set(LocationType row, LocationType column, DimensionType dimensionValue)
        {
            _matrix[(int)row, (int)column] = dimensionValue;
        }

        /// <summary>
        /// Changes the elements of this <see cref="IntersectionMatrix" /> to the
        /// dimension symbols in <c>dimensionSymbols</c>.
        /// </summary>
        /// <param name="dimensionSymbols">
        /// Nine dimension symbols to which to set this <see cref="IntersectionMatrix" />
        /// s elements. Possible values are <c>{T, F, * , 0, 1, 2}</c>
        /// </param>
        public void Set(string dimensionSymbols)
        {
            for (int i = 0; i < dimensionSymbols.Length; i++)
            {
                int row = i / 3;
                int col = i % 3;
                _matrix[row, col] = Dimension.ToDimensionValue(dimensionSymbols[i]);
            }
        }

        /// <summary>  
        /// Changes the elements of this <see cref="IntersectionMatrix" /> to <c>dimensionValue</c>.
        /// </summary>
        /// <param name="dimensionValue">
        /// The dimension value to which to set this <see cref="IntersectionMatrix" />
        /// s elements. Possible values <c>True, False, Dontcare, 0, 1, 2}</c>.
        /// </param>         
        public void SetAll(DimensionType dimensionValue)
        {
            for (int ai = 0; ai < 3; ai++)
                for (int bi = 0; bi < 3; bi++)
                    _matrix[ai, bi] = dimensionValue;
        }

        /// <summary>
        /// Changes the specified element to <c>minimumDimensionValue</c> if the element is less.
        /// </summary>
        /// <param name="row">
        /// The row of this <see cref="IntersectionMatrix" />, 
        /// indicating the interior, boundary or exterior of the first <see cref="IGeometry"/>.
        /// </param>
        /// <param name="column">
        /// The column of this <see cref="IntersectionMatrix" />, 
        /// indicating the interior, boundary or exterior of the second <see cref="IGeometry"/>.
        /// </param>
        /// <param name="minimumDimensionValue">
        /// The dimension value with which to compare the
        /// element. The order of dimension values from least to greatest is
        /// <c>True, False, Dontcare, 0, 1, 2</c>.
        /// </param>
        public virtual void SetAtLeast(LocationType row, LocationType column, DimensionType minimumDimensionValue)
        {
            if (_matrix[(int)row, (int)column] < minimumDimensionValue)
                _matrix[(int)row, (int)column] = minimumDimensionValue;
        }

        /// <summary>
        /// For each element in this <see cref="IntersectionMatrix" />, changes the
        /// element to the corresponding minimum dimension symbol if the element is
        /// less.
        /// </summary>
        /// <param name="minimumDimensionSymbols"> 
        /// Nine dimension symbols with which to
        /// compare the elements of this <see cref="IntersectionMatrix" />. The
        /// order of dimension values from least to greatest is <c>Dontcare, True, False, 0, 1, 2</c>.
        /// </param>
        public virtual void SetAtLeast(string minimumDimensionSymbols)
        {
            for (int i = 0; i < minimumDimensionSymbols.Length; i++)
            {
                int row = i / 3;
                int col = i % 3;
                SetAtLeast((LocationType)row, (LocationType)col, Dimension.ToDimensionValue(minimumDimensionSymbols[i]));
            }
        }

        /// <summary>
        /// If row >= 0 and column >= 0, changes the specified element to <c>minimumDimensionValue</c>
        /// if the element is less. Does nothing if row is smaller to 0 or column is smaller to 0.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="minimumDimensionValue"></param>
        public virtual void SetAtLeastIfValid(LocationType row, LocationType column, DimensionType minimumDimensionValue)
        {
            if (row >= LocationType.Interior && column >= LocationType.Interior)
                SetAtLeast(row, column, minimumDimensionValue);
        }

        /// <summary>
        /// Returns a nine-character <c>String</c> representation of this <see cref="IntersectionMatrix" />.
        /// </summary>
        /// <returns>
        /// The nine dimension symbols of this <see cref="IntersectionMatrix" />
        /// in row-major order.
        /// </returns>
        public override string ToString()
        {
            StringBuilder buf = new StringBuilder("123456789");
            for (int ai = 0; ai < 3; ai++)
                for (int bi = 0; bi < 3; bi++)
                    buf[3 * ai + bi] = Dimension.ToDimensionSymbol(_matrix[ai, bi]);
            return buf.ToString();
        }

        /// <summary>  
        /// Transposes this IntersectionMatrix.
        /// </summary>
        /// <returns>This <see cref="IntersectionMatrix" /> as a convenience,</returns>
        public virtual IIntersectionMatrix Transpose()
        {
            DimensionType temp = _matrix[1, 0];
            _matrix[1, 0] = _matrix[0, 1];
            _matrix[0, 1] = temp;

            temp = _matrix[2, 0];
            _matrix[2, 0] = _matrix[0, 2];
            _matrix[0, 2] = temp;

            temp = _matrix[2, 1];
            _matrix[2, 1] = _matrix[1, 2];
            _matrix[1, 2] = temp;

            return this;
        }

        #endregion
    }
}