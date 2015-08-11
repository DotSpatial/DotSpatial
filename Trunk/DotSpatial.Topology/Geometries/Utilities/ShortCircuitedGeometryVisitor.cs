namespace DotSpatial.Topology.Geometries.Utilities
{
    /// <summary>
    /// A visitor to <see cref="IGeometry"/> elements which  componets, which 
    /// allows short-circuiting when a defined condition holds.
    /// </summary>
    public abstract class ShortCircuitedGeometryVisitor
    {
        #region Fields

        private bool _isDone;

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geom"></param>
        public void ApplyTo(IGeometry geom) 
        {
            for (var i = 0; i < geom.NumGeometries && ! _isDone; i++) 
            {
                var element = geom.GetGeometryN(i);
                if (!(element is IGeometryCollection)) 
                {
                    Visit(element);
                    if (IsDone()) 
                    {
                        _isDone = true;
                        return;
                    }
                }
                else ApplyTo(element);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract bool IsDone();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        protected abstract void Visit(IGeometry element);

        #endregion
    }
}
