using System.Linq;
using System.Reflection;
using GeoAPI.Geometries;
using NetTopologySuite.Index.KdTree;

namespace DotSpatial.Tools
{
    /// <summary>
    /// An extended implementation of a 2-D KD-Tree.
    /// </summary>
    /// <typeparam name="T">Type to use for the KdTree objects.</typeparam>
    public class KdTreeEx<T> : KdTree<T>
        where T : class
    {
        #region Fields

        private readonly MethodInfo _findBestMatchNodeMethod;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="KdTreeEx{T}"/> class.
        /// </summary>
        public KdTreeEx()
        {
            _findBestMatchNodeMethod = typeof(KdTree<T>).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault(_ => _.Name == "FindBestMatchNode");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the _findBestMatchNodeMethod is set.
        /// </summary>
        public bool MethodFindBestMatchNodeFound => _findBestMatchNodeMethod != null;

        #endregion

        #region Methods

        /// <summary>
        /// Searches for the best matching node.
        /// </summary>
        /// <param name="coord">Coordinate used to find the best matching node.</param>
        /// <returns>Null if no node was found otherwise the nodes Data.</returns>
        public T Search(Coordinate coord)
        {
            var node = (KdNode<T>)_findBestMatchNodeMethod.Invoke(this, new object[] { coord });
            return node?.Data;
        }

        #endregion
    }
}