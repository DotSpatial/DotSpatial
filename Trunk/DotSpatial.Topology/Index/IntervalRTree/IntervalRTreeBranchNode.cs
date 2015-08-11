using System;

namespace DotSpatial.Topology.Index.IntervalRTree
{
    public class IntervalRTreeBranchNode<T> : IntervalRTreeNode<T>
    {
        #region Fields

        private readonly IntervalRTreeNode<T> _node1;
        private readonly IntervalRTreeNode<T> _node2;

        #endregion

        #region Constructors

        public IntervalRTreeBranchNode(IntervalRTreeNode<T> n1, IntervalRTreeNode<T> n2)
        {
            _node1 = n1;
            _node2 = n2;
            BuildExtent(_node1, _node2);
        }

        #endregion

        #region Methods

        private void BuildExtent(IntervalRTreeNode<T> n1, IntervalRTreeNode<T> n2)
        {
            Min = Math.Min(n1.Min, n2.Min);
            Max = Math.Max(n1.Max, n2.Max);
        }

        public override void Query(double queryMin, double queryMax, IItemVisitor<T> visitor)
        {
            if (!Intersects(queryMin, queryMax)) return;
            if (_node1 != null) _node1.Query(queryMin, queryMax, visitor);
            if (_node2 != null) _node2.Query(queryMin, queryMax, visitor);
        }

        #endregion
    }
}