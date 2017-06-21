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

namespace DotSpatial.Topology.Index.Quadtree
{
    /// <summary>
    /// A Key is a unique identifier for a node in a quadtree.
    /// It contains a lower-left point and a level number. The level number
    /// is the power of two for the size of the node envelope.
    /// </summary>
    public class Key
    {
        // the fields which make up the key
        private readonly Coordinate _pt = new Coordinate();

        // auxiliary data which is derived from the key for use in computation
        private IEnvelope _env;
        private int _level;

        /// <summary>
        ///
        /// </summary>
        /// <param name="itemEnv"></param>
        public Key(IEnvelope itemEnv)
        {
            ComputeKey(itemEnv);
        }

        /// <summary>
        ///
        /// </summary>
        public virtual Coordinate Point
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
        public virtual IEnvelope Envelope
        {
            get
            {
                return _env;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual Coordinate Centre
        {
            get
            {
                return new Coordinate((_env.Minimum.X + _env.Maximum.X) / 2, (_env.Minimum.Y + _env.Maximum.Y) / 2);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="env"></param>
        /// <returns></returns>
        public static int ComputeQuadLevel(IEnvelope env)
        {
            double dx = env.Width;
            double dy = env.Height;
            double dMax = dx > dy ? dx : dy;
            int level = DoubleBits.GetExponent(dMax) + 1;
            return level;
        }

        /// <summary>
        /// Return a square envelope containing the argument envelope,
        /// whose extent is a power of two and which is based at a power of 2.
        /// </summary>
        /// <param name="itemEnv"></param>
        public void ComputeKey(IEnvelope itemEnv)
        {
            _level = ComputeQuadLevel(itemEnv);
            _env = new Envelope();
            ComputeKey(_level, itemEnv);
            // MD - would be nice to have a non-iterative form of this algorithm
            while (!_env.Contains(itemEnv))
            {
                _level += 1;
                ComputeKey(_level, itemEnv);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="level"></param>
        /// <param name="itemEnv"></param>
        private void ComputeKey(int level, IEnvelope itemEnv)
        {
            double quadSize = DoubleBits.PowerOf2(level);
            _pt.X = Math.Floor(itemEnv.Minimum.X / quadSize) * quadSize;
            _pt.Y = Math.Floor(itemEnv.Minimum.Y / quadSize) * quadSize;
            _env = new Envelope(_pt.X, _pt.X + quadSize, _pt.Y, _pt.Y + quadSize);
        }
    }
}