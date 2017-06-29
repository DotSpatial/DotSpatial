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
using DotSpatial.Topology.Index.Quadtree;

namespace DotSpatial.Topology.Index.Bintree
{
    /// <summary>
    /// A Key is a unique identifier for a node in a tree.
    /// It contains a lower-left point and a level number. The level number
    /// is the power of two for the size of the node envelope.
    /// </summary>
    public class Key
    {
        // auxiliary data which is derived from the key for use in computation
        private Interval _interval;
        private int _level;
        private double _pt;

        /// <summary>
        ///
        /// </summary>
        /// <param name="interval"></param>
        public Key(Interval interval)
        {
            ComputeKey(interval);
        }

        /// <summary>
        ///
        /// </summary>
        public virtual double Point
        {
            get
            {
                return _pt;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual int Level
        {
            get
            {
                return _level;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual Interval Interval
        {
            get
            {
                return _interval;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static int ComputeLevel(Interval interval)
        {
            double dx = interval.Width;
            int level = DoubleBits.GetExponent(dx) + 1;
            return level;
        }

        /// <summary>
        /// Return a square envelope containing the argument envelope,
        /// whose extent is a power of two and which is based at a power of 2.
        /// </summary>
        /// <param name="itemInterval"></param>
        public void ComputeKey(Interval itemInterval)
        {
            _level = ComputeLevel(itemInterval);
            _interval = new Interval();
            ComputeInterval(_level, itemInterval);
            // MD - would be nice to have a non-iterative form of this algorithm
            while (!_interval.Contains(itemInterval))
            {
                _level += 1;
                ComputeInterval(_level, itemInterval);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="level"></param>
        /// <param name="itemInterval"></param>
        private void ComputeInterval(int level, Interval itemInterval)
        {
            double size = DoubleBits.PowerOf2(level);
            _pt = Math.Floor(itemInterval.Min / size) * size;
            _interval.Init(_pt, _pt + size);
        }
    }
}