namespace DotSpatial.Topology.Index.IntervalRTree
{
    public class IntervalRTreeLeafNode<T> : IntervalRTreeNode<T>
    {
        #region Fields

        private readonly T _item;

        #endregion

        #region Constructors

        public IntervalRTreeLeafNode(double min, double max, T item)
            : base(min, max)
        {
            _item = item;
        }

        #endregion

        #region Methods

        public override void Query(double queryMin, double queryMax, IItemVisitor<T> visitor)
        {
            if (!Intersects(queryMin, queryMax)) return;
            visitor.VisitItem(_item);
        }

        #endregion
    }
}