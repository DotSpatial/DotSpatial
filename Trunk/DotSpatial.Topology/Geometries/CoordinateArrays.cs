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
using DotSpatial.Topology.Mathematics;

namespace DotSpatial.Topology.Geometries
{
    /// <summary>
    ///	Useful utility functions for handling Coordinate arrays.
    /// </summary>
    public static class CoordinateArrays
    {
        #region Methods

        /// <summary>
        /// Returns either the given coordinate array if its length is greater than
        /// the given amount, or an empty coordinate array.
        /// </summary>
        /// <param name="n">Length amount.</param>
        /// <param name="c">Array of Coordinates.</param>
        /// <returns>New Coordinate array.</returns>
        public static Coordinate[] AtLeastNCoordinatesOrNothing(int n, Coordinate[] c)
        {
            return c.Length >= n ? c : new Coordinate[] { };
        }

        /// <summary>
        /// Compares two <see cref="Coordinate" /> arrays
        /// in the forward direction of their coordinates,
        /// using lexicographic ordering.
        /// </summary>
        /// <param name="pts1"></param>
        /// <param name="pts2"></param>
        /// <returns></returns>
        public static int Compare(Coordinate[] pts1, Coordinate[] pts2)
        {
            int i = 0;
            while (i < pts1.Length && i < pts2.Length)
            {
                int compare = pts1[i].CompareTo(pts2[i]);
                if (compare != 0)
                    return compare;
                i++;
            }

            // handle situation when arrays are of different length
            if (i < pts2.Length)
                return -1;
            if (i < pts1.Length)
                return 1;

            return 0;
        }

        /// <summary>
        /// Creates a deep copy of the argument <c>Coordinate</c> array.
        /// </summary>
        /// <param name="coordinates">Array of Coordinates.</param>
        /// <returns>Deep copy of the input.</returns>
        public static Coordinate[] CopyDeep(Coordinate[] coordinates)
        {
            Coordinate[] copy = new Coordinate[coordinates.Length];
            for (int i = 0; i < coordinates.Length; i++)
                copy[i] = new Coordinate(coordinates[i]);
            return copy;
        }

        /// <summary>
        /// Creates a deep copy of a given section of a source <see cref="Coordinate"/> array into a destination Coordinate array.
        /// The destination array must be an appropriate size to receive the copied coordinates.
        /// </summary>
        /// <param name="src">An array of Coordinates</param>
        /// <param name="srcStart">The index to start copying from</param>
        /// <param name="dest">The array to receive the deep-copied coordinates</param>
        /// <param name="destStart">The destination index to start copying to</param>
        /// <param name="length">The number of items to copy</param>
        public static void CopyDeep(Coordinate[] src, int srcStart, Coordinate[] dest, int destStart, int length)
        {
            for (int i = 0; i < length; i++)
            {
                Coordinate c = new Coordinate(src[srcStart + i]);
                dest[destStart + i] = c;
            }
        }

        /// <summary>
        /// Computes the <see cref="Envelope"/> of the coordinates.
        /// </summary>
        /// <param name="coordinates">the <see cref="Coordinate"/> array to scan.</param>
        /// <returns>the <see cref="Envelope"/> of the <paramref name="coordinates"/>.</returns>
        public static Envelope Envelope(Coordinate[] coordinates)
        {
            Envelope env = new Envelope();
            for (int i = 0; i < coordinates.Length; i++)
            {
                Coordinate c = coordinates[i];
                env.ExpandToInclude(c);
            }
            return env;
        }

        /// <summary>
        /// Returns <c>true</c> if the two arrays are identical, both <c>null</c>, or pointwise
        /// equal (as compared using Coordinate.Equals).
        /// </summary>
        /// <param name="coord1">First array of Coordinates.</param>
        /// <param name="coord2">Second array of Coordinates.</param>
        /// <returns><c>true</c> if two Coordinates array are equals; false otherwise</returns>
        public static bool Equals(Coordinate[] coord1, Coordinate[] coord2)
        {
            if (coord1 == coord2)
                return true;
            if (coord1 == null || coord2 == null)
                return false;
            if (coord1.Length != coord2.Length)
                return false;
            for (int i = 0; i < coord1.Length; i++)
                if (!coord1[i].Equals(coord2[i]))
                    return false;
            return true;
        }

        /// <summary>
        /// Returns <c>true</c> if the two arrays are identical, both <c>null</c>, or pointwise
        /// equal, using a user-defined <see cref="System.Collections.IComparer" /> 
        /// for <see cref="Coordinate" />s.
        /// </summary>
        /// <param name="coord1">An array of <see cref="Coordinate" />s.</param>
        /// <param name="coord2">Another array of <see cref="Coordinate" />s.</param>
        /// <param name="coordinateComparer">
        /// A <see cref="System.Collections.IComparer" /> for <see cref="Coordinate" />s.
        /// </param>
        /// <returns></returns>
        public static bool Equals(Coordinate[] coord1, Coordinate[] coord2,
                IComparer<Coordinate[]> coordinateComparer)
        {
            if (coord1 == coord2)
                return true;
            if (coord1 == null || coord2 == null)
                return false;
            if (coord1.Length != coord2.Length)
                return false;
            if (coordinateComparer.Compare(coord1, coord2) != 0)
                return false;
            return true;
        }

        /// <summary>
        /// Extracts a subsequence of the input <see cref="Coordinate" /> array
        /// from indices <paramref name="start" /> to <paramref name="end"/> (inclusive).
        /// The input indices are clamped to the array size;
        /// If the end index is less than the start index,
        /// the extracted array will be empty.
        /// </summary>
        /// <param name="pts">The input array.</param>
        /// <param name="start">The index of the start of the subsequence to extract.</param>
        /// <param name="end">The index of the end of the subsequence to extract.</param>
        /// <returns>A subsequence of the input array.</returns>
        public static Coordinate[] Extract(Coordinate[] pts, int start, int end)
        {
            start = MathUtil.Clamp(start, 0, pts.Length);
            end = MathUtil.Clamp(end, -1, pts.Length);

            int npts = end - start + 1;
            if (end < 0)
                npts = 0;
            if (start >= pts.Length)
                npts = 0;
            if (end < start)
                npts = 0;

            Coordinate[] extractPts = new Coordinate[npts];
            if (npts == 0)
                return extractPts;

            Array.Copy(pts, start, extractPts, 0, npts);
            return extractPts;
        }

        /// <summary>
        /// Returns whether Equals returns true for any two consecutive
        /// coordinates in the given array.
        /// </summary>
        /// <param name="coords">Array of Coordinates.</param>
        /// <returns>true if coord has repeated points; false otherwise.</returns>
        public static bool HasRepeatedPoints(IEnumerable<Coordinate> coords)
        {
            Coordinate previous = null;
            foreach (var coord in coords)
            {
                if (coord.Equals(previous)) return true;
                previous = coord;
            }
            return false;
        }

        /// <summary>
        /// Determines which orientation of the <see cref="Coordinate" /> array is (overall) increasing.
        /// In other words, determines which end of the array is "smaller"
        /// (using the standard ordering on <see cref="Coordinate" />).
        /// Returns an integer indicating the increasing direction.
        /// If the sequence is a palindrome, it is defined to be
        /// oriented in a positive direction.
        /// </summary>
        /// <param name="pts">The array of Coordinates to test.</param>
        /// <returns>
        /// <c>1</c> if the array is smaller at the start or is a palindrome,
        /// <c>-1</c> if smaller at the end.
        /// </returns>
        public static int IncreasingDirection(IList<Coordinate> pts)
        {
            for (int i = 0; i < pts.Count / 2; i++)
            {
                int j = pts.Count - 1 - i;
                // skip equal points on both ends
                int comp = pts[i].CompareTo(pts[j]);
                if (comp != 0)
                    return comp;
            }
            // array must be a palindrome - defined to be in positive direction
            return 1;
        }

        /// <summary>
        /// Returns the index of <paramref name="coordinate" /> in <paramref name="coordinates" />.
        /// The first position is 0; the second is 1; etc.
        /// </summary>
        /// <param name="coordinate">A <see cref="Coordinate" /> to search for.</param>
        /// <param name="coordinates">A <see cref="Coordinate" /> array to search.</param>
        /// <returns>The position of <c>coordinate</c>, or -1 if it is not found.</returns>
        public static int IndexOf(Coordinate coordinate, Coordinate[] coordinates)
        {
            for (int i = 0; i < coordinates.Length; i++)
                if (coordinate.Equals(coordinates[i]))
                    return i;
            return -1;
        }

        /**
         * Extracts the coordinates which intersect an {@link Envelope}.
         * 
         * @param coordinates the coordinates to scan
         * @param env the envelope to intersect with
         * @return an array of the coordinates which intersect the envelope
         */

        public static Coordinate[] Intersection(Coordinate[] coordinates, Envelope env)
        {
            CoordinateList coordList = new CoordinateList();
            for (int i = 0; i < coordinates.Length; i++)
            {
                Coordinate c = coordinates[i];
                if (env.Intersects(c))
                    coordList.Add(c, true);
            }
            return coordList.ToCoordinateArray();
        }

        /// <summary>
        /// Determines whether two <see cref="Coordinate" /> arrays of equal length
        /// are equal in opposite directions.
        /// </summary>
        /// <param name="pts1"></param>
        /// <param name="pts2"></param>
        /// <returns></returns>
        private static bool IsEqualReversed(Coordinate[] pts1, Coordinate[] pts2)
        {
            for (int i = 0; i < pts1.Length; i++)
            {
                Coordinate p1 = pts1[i];
                Coordinate p2 = pts2[pts1.Length - i - 1];
                if (p1.CompareTo(p2) != 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Tests whether an array of <see cref="Coordinate"/>s forms a ring, by checking length and closure.
        /// Self-intersection is not checked.
        /// </summary>
        /// <param name="pts">An array of Coordinates</param>
        /// <returns>true if the coordinate form a ring.</returns>
        public static bool IsRing(Coordinate[] pts)
        {
            if (pts.Length < 4)
                return false;
            if (!pts[0].Equals2D(pts[pts.Length - 1]))
                return false;
            return true;
        }

        /// <summary>
        /// Returns the minimum coordinate, using the usual lexicographic comparison.
        /// </summary>
        /// <param name="coordinates">Array to search.</param>
        /// <returns>The minimum coordinate in the array, found using <c>CompareTo</c>.</returns>
        public static Coordinate MinCoordinate(Coordinate[] coordinates)
        {
            Coordinate minCoord = null;
            for (int i = 0; i < coordinates.Length; i++)
                if (minCoord == null || minCoord.CompareTo(coordinates[i]) > 0)
                    minCoord = coordinates[i];
            return minCoord;
        }

        /// <summary>
        /// Finds a <see cref="Coordinate "/> in a list of <see cref="Coordinate "/>s 
        /// which is not contained in another list of <see cref="Coordinate "/>s.
        /// </summary>
        /// <param name="testPts">The <see cref="Coordinate" />s to test.</param>
        /// <param name="pts">An array of <see cref="Coordinate" />s to test the input points against.</param>
        /// <returns>
        /// A <see cref="Coordinate" /> from <paramref name="testPts" /> 
        /// which is not in <paramref name="pts" />, or <c>null</c>.
        /// </returns>
        public static Coordinate PointNotInList(Coordinate[] testPts, Coordinate[] pts)
        {
            for (int i = 0; i < testPts.Length; i++)
            {
                Coordinate testPt = testPts[i];
                if (IndexOf(testPt, pts) < 0)
                    return testPt;
            }
            return null;
        }

        /// <summary>
        /// Collapses a coordinate array to remove all null elements.
        /// </summary>
        /// <param name="coord">The coordinate array to collapse</param>
        /// <returns>An Array containing only non-null elements</returns>
        public static Coordinate[] RemoveNull(Coordinate[] coord)
        {
            List<Coordinate> coordinateList = new List<Coordinate>(coord.Length);
            foreach (Coordinate coordinate in coord)
            {
                if (coordinate != null)
                    coordinateList.Add(coordinate);
            }
            return coordinateList.ToArray();
        }

        /// <summary>
        /// If the coordinate array argument has repeated points,
        /// constructs a new array containing no repeated points.
        /// Otherwise, returns the argument.
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        public static IList<Coordinate> RemoveRepeatedPoints(IList<Coordinate> coords)
        {
            if (!HasRepeatedPoints(coords))
                return coords;
            CoordinateList coordList = new CoordinateList(coords, false);
            return coordList;
        }

        /// <summary>
        /// Reverses the coordinates in an array in-place.
        /// </summary>
        /// <param name="coord">Array of Coordinates.</param>
        public static void Reverse(Coordinate[] coord)
        {
            Array.Reverse(coord);
        }

        /// <summary>
        /// Shifts the positions of the coordinates until <c>firstCoordinate</c> is first.
        /// </summary>
        /// <param name="coordinates">Array to rearrange.</param>
        /// <param name="firstCoordinate">Coordinate to make first.</param>
        public static void Scroll(Coordinate[] coordinates, Coordinate firstCoordinate)
        {
            int i = IndexOf(firstCoordinate, coordinates);
            if (i < 0)
                return;
            Coordinate[] newCoordinates = new Coordinate[coordinates.Length];
            Array.Copy(coordinates, i, newCoordinates, 0, coordinates.Length - i);
            Array.Copy(coordinates, 0, newCoordinates, coordinates.Length - i, i);
            Array.Copy(newCoordinates, 0, coordinates, 0, coordinates.Length);
        }

        /// <summary>
        /// Converts the given <see cref="System.Collections.ICollection" /> of 
        /// <see cref="Coordinate" />s into a <see cref="Coordinate" /> array.
        /// </summary>
        /// <param name="coordList"><see cref="System.Collections.ICollection" /> of coordinates.</param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException">
        /// If <paramref name="coordList"/> contains not only <see cref="Coordinate" />s.
        /// </exception>
        [Obsolete("Use generic method instead")]
        public static Coordinate[] ToCoordinateArray(ICollection coordList)
        {
            List<Coordinate> tempList = new List<Coordinate>(coordList.Count);
            foreach (Coordinate coord in coordList)
                tempList.Add(coord);
            return tempList.ToArray();
        }

        /// <summary>
        /// Converts the given <see cref="ICollection{T}" /> of 
        /// <see cref="Coordinate" />s into a <see cref="Coordinate" /> array.
        /// </summary>
        /// <param name="coordList"><see cref="ICollection{T}"/> of coordinates.</param>
        /// <returns></returns>
        public static Coordinate[] ToCoordinateArray(ICollection<Coordinate> coordList)
        {
            List<Coordinate> tempList = new List<Coordinate>(coordList.Count);
            foreach (Coordinate coord in coordList)
                tempList.Add(coord);
            return tempList.ToArray();
        }

        #endregion

        #region Classes

        /// <summary>
        /// A comparator for <see cref="Coordinate" /> arrays modulo their directionality.
        /// E.g. if two coordinate arrays are identical but reversed
        /// they will compare as equal under this ordering.
        /// If the arrays are not equal, the ordering returned
        /// is the ordering in the forward direction.
        /// </summary>        
        public class BidirectionalComparator : IComparer<Coordinate[]>
        {
            #region Methods

            /// <summary>
            ///
            /// </summary>
            /// <param name="pts1"></param>
            /// <param name="pts2"></param>
            /// <returns></returns>
            public virtual int Compare(Coordinate[] pts1, Coordinate[] pts2)
            {
                if (pts1.Length < pts2.Length)
                    return -1;
                if (pts1.Length > pts2.Length)
                    return 1;

                if (pts1.Length == 0)
                    return 0;

                int forwardComp = CoordinateArrays.Compare(pts1, pts2);
                bool isEqualRev = IsEqualReversed(pts1, pts2);
                return isEqualRev ? 0 : forwardComp;
            }

            #endregion
        }

        /// <summary>
        /// Compares two <see cref="Coordinate" /> arrays
        /// in the forward direction of their coordinates,
        /// using lexicographic ordering.
        /// </summary>
        public class ForwardComparator : IComparer<Coordinate[]>
        {
            #region Methods

            /// <summary>
            /// Compares the specified <see cref="Coordinate" />s arrays.
            /// </summary>
            /// <param name="pts1"></param>
            /// <param name="pts2"></param>
            /// <returns></returns>
            public virtual int Compare(Coordinate[] pts1, Coordinate[] pts2)
            {
                return CoordinateArrays.Compare(pts1, pts2);
            }

            #endregion
        }

        #endregion
    }
}