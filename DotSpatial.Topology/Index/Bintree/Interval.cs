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

namespace DotSpatial.Topology.Index.Bintree
{
    /// <summary>
    /// Represents an (1-dimensional) closed interval on the Real number line.
    /// </summary>
    public class Interval
    {
        private double _max;
        private double _min;

        /// <summary>
        ///
        /// </summary>
        public Interval()
        {
            _min = 0.0;
            _max = 0.0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public Interval(double min, double max)
        {
            Init(min, max);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="interval"></param>
        public Interval(Interval interval)
        {
            Init(interval.Min, interval.Max);
        }

        /// <summary>
        ///
        /// </summary>
        public virtual double Min
        {
            get
            {
                return _min;
            }
            set
            {
                _min = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual double Max
        {
            get
            {
                return _max;
            }
            set
            {
                _max = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual double Width
        {
            get
            {
                return Max - Min;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void Init(double min, double max)
        {
            Min = min;
            Max = max;

            if (min <= max) return;
            Min = max;
            Max = min;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="interval"></param>
        public virtual void ExpandToInclude(Interval interval)
        {
            if (interval.Max > Max)
                Max = interval.Max;
            if (interval.Min < Min)
                Min = interval.Min;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public virtual bool Overlaps(Interval interval)
        {
            return Overlaps(interval.Min, interval.Max);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public virtual bool Overlaps(double min, double max)
        {
            return Min <= max && Max >= min;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public virtual bool Contains(Interval interval)
        {
            return Contains(interval.Min, interval.Max);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public virtual bool Contains(double min, double max)
        {
            return (min >= Min && max <= Max);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual bool Contains(double p)
        {
            return (p >= Min && p <= Max);
        }
    }
}