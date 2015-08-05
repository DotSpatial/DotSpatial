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
        #region Fields

        // auxiliary data which is derived from the key for use in computation

        #endregion

        #region Constructors

        /// <summary>
        ///
        /// </summary>
        /// <param name="itemEnv"></param>
        public Key(Envelope itemEnv)
        {
            Point = new Coordinate();
            ComputeKey(itemEnv);
        }

        #endregion

        #region Properties

        /// <summary>
        ///
        /// </summary>
        public virtual Coordinate Centre
        {
            get
            {
                return new Coordinate((Envelope.Minimum.X + Envelope.Maximum.X) / 2, (Envelope.Minimum.Y + Envelope.Maximum.Y) / 2);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public Envelope Envelope { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public Coordinate Point { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Return a square envelope containing the argument envelope,
        /// whose extent is a power of two and which is based at a power of 2.
        /// </summary>
        /// <param name="itemEnv"></param>
        public void ComputeKey(Envelope itemEnv)
        {
            Level = ComputeQuadLevel(itemEnv);
            Envelope = new Envelope();
            ComputeKey(Level, itemEnv);
            // MD - would be nice to have a non-iterative form of this algorithm
            while (!Envelope.Contains(itemEnv))
            {
                Level += 1;
                ComputeKey(Level, itemEnv);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="env"></param>
        /// <returns></returns>
        public static int ComputeQuadLevel(Envelope env)
        {
            double dx = env.Width;
            double dy = env.Height;
            double dMax = dx > dy ? dx : dy;
            int level = DoubleBits.GetExponent(dMax) + 1;
            return level;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="level"></param>
        /// <param name="itemEnv"></param>
        private void ComputeKey(int level, Envelope itemEnv)
        {
            double quadSize = DoubleBits.PowerOf2(level);
            Point.X = Math.Floor(itemEnv.Minimum.X / quadSize) * quadSize;
            Point.Y = Math.Floor(itemEnv.Minimum.Y / quadSize) * quadSize;
            Envelope = new Envelope(Point.X, Point.X + quadSize, Point.Y, Point.Y + quadSize);
        }

        #endregion
    }
}