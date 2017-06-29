// ********************************************************************************************************
// Product Name: DotSpatial.Topology.dll
// Description:  The basic topology module for the new dotSpatial libraries
// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/27/2009 4:55:32 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// Name              |   Date             |   Comments
// ------------------|--------------------|---------------------------------------------------------
// Benjamin Dittes   | August 10, 2005    |  Authored original code for working with laser data
// Ted Dunsford      | August 26, 2009    |  Ported and cleaned up the raw source from code project
// ********************************************************************************************************

using System;

namespace DotSpatial.Topology.Voronoi
{
    internal abstract class VEvent : IComparable
    {
        public abstract double Y { get; }

        protected abstract double X { get; }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (!(obj is VEvent))
                throw new ArgumentException("obj not VEvent!");
            int i = Y.CompareTo(((VEvent)obj).Y);
            return i != 0 ? i : X.CompareTo(((VEvent)obj).X);
        }

        #endregion
    }

    internal class VDataEvent : VEvent
    {
        public readonly Vector2 DataPoint;

        public VDataEvent(Vector2 dp)
        {
            DataPoint = dp;
        }

        public override double Y
        {
            get
            {
                return DataPoint.Y;
            }
        }

        protected override double X
        {
            get
            {
                return DataPoint.X;
            }
        }
    }

    internal class VCircleEvent : VEvent
    {
        public Vector2 Center;
        public VDataNode NodeL;
        public VDataNode NodeN;
        public VDataNode NodeR;
        public bool Valid = true;

        public override double Y
        {
            get
            {
                return Center.Y + MathTools.Dist(NodeN.DataPoint.X, NodeN.DataPoint.Y, Center.X, Center.Y);
            }
        }

        protected override double X
        {
            get
            {
                return Center.X;
            }
        }
    }
}