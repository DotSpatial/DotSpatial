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

namespace DotSpatial.Topology.Index.Bintree
{
    /// <summary>
    /// Represents an (1-dimensional) closed interval on the Real number line.
    /// </summary>
    [Serializable]
    public class Interval
    {
        #region Fields

        private double _max;
        private double _min;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new interval instance, setting <see cref="Min"/>=<see cref="Max"/>=0d;
        /// </summary>
        public Interval()
        {
            _min = 0.0;
            _max = 0.0;
        }

        /// <summary>
        /// Creates a new interval instance, setting <see cref="Min"/>=<paramref name="min"/> and <see cref="Max"/>=<paramref name="max"/>;
        /// </summary>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        public Interval(double min, double max)
        {
            Init(min, max);
        }

        /// <summary>
        /// Creates a new interval instance, setting <see cref="Min"/>=<paramref name="interval.Min"/> and <see cref="Max"/>=<paramref name="interval.Max"/>.
        /// </summary>
        /// <param name="interval"></param>
        public Interval(Interval interval)
        {
            Init(interval.Min, interval.Max);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the centre of the interval (<see cref="Min"/> + <see cref="Width"/> * 0.5d)
        /// </summary>
        public double Centre
        {
            get { return Max - Min; }
        }

        /// <summary>
        /// Gets the width of the interval (<see cref="Max"/> - <see cref="Min"/>)
        /// </summary>
        public double Width
        {
            get { return Max - Min; }
        }

        /// <summary>
        /// Gets or sets a value indicating the maximum value of the closed interval.
        /// </summary>
        public double Max
        {
            get { return _max; }
            set { _max = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating the minimum value of the closed interval.
        /// </summary>
        public double Min
        {
            get { return _min; }
            set { _min = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Function to test if this <see cref="Interval"/> contains <paramref name="interval"/>.
        /// </summary>
        /// <remarks>This is more rigid than <see cref="Overlaps(Interval)"/></remarks>
        /// <param name="interval">The interval to test</param>
        /// <returns><c>true</c> if this interval contains <paramref name="interval"/></returns>
        public bool Contains(Interval interval)
        {
            return Contains(interval.Min, interval.Max);
        }

        /// <summary>
        /// Function to test if this <see cref="Interval"/> contains the interval R[<paramref name="min"/>, <paramref name="max"/>].
        /// </summary>
        /// <remarks>This is more rigid than <see cref="Overlaps(double, double)"/></remarks>
        /// <param name="min">The mimimum value of the interval</param>
        /// <param name="max">The maximum value of the interval</param>
        /// <returns><c>true</c> if this interval contains the interval R[<paramref name="min"/>, <paramref name="max"/>]</returns>
        public bool Contains(double min, double max)
        {
            return (min >= Min && max <= Max);
        }

        /// <summary>
        /// Function to test if this <see cref="Interval"/> contains the value <paramref name="p"/>.
        /// </summary>
        /// <param name="p">The value to test</param>
        /// <returns><c>true</c> if this interval contains the value <paramref name="p"/></returns>
        public bool Contains(double p)
        {
            return (p >= Min && p <= Max);
        }

        /// <summary>
        /// Method to expand this interval to contain <paramref name="interval"/>.
        /// </summary>
        /// <param name="interval">The interval to contain.</param>
        public void ExpandToInclude(Interval interval)
        {
            if (interval.Max > Max)
                Max = interval.Max;
            if (interval.Min < Min)
                Min = interval.Min;
        }

        /// <summary>
        /// Method to initialize the interval with the given <paramref name="min"/> and <paramref name="max"/> values. <br/>
        /// If <paramref name="max"/> &lt; <paramref name="min"/>, their values are exchanged.
        /// </summary>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        public void Init(double min, double max)
        {
            Min = min;
            Max = max;

            if (min > max)
            {
                Min = max;
                Max = min;
            }
        }

        /// <summary>
        /// Function to test if this <see cref="Interval"/> overlaps <paramref name="interval"/>.
        /// </summary>
        /// <param name="interval">The interval to test</param>
        /// <returns><c>true</c> if this interval overlaps <paramref name="interval"/></returns>
        public bool Overlaps(Interval interval)
        {
            return Overlaps(interval.Min, interval.Max);
        }

        /// <summary>
        /// Function to test if this <see cref="Interval"/> overlaps the interval R[<paramref name="min"/>, <paramref name="max"/>].
        /// </summary>
        /// <param name="min">The mimimum value of the interval</param>
        /// <param name="max">The maximum value of the interval</param>
        /// <returns><c>true</c> if this interval overlaps the interval R[<paramref name="min"/>, <paramref name="max"/>]</returns>
        public bool Overlaps(double min, double max)
        {
            return !(Min > max || Max < min);
        }

        #endregion
    }
}