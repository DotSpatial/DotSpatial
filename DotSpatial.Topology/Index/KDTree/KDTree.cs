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

using System.Collections.Generic;

namespace DotSpatial.Topology.KDTree
{
    /// <summary>
    /// This is an adaptation of the Java KdTree library implemented by Levy
    /// and Heckel. This simplified version is written by Marco A. Alvarez
    ///
    /// KdTree is a class supporting KD-tree insertion, deletion, equality
    /// search, range search, and nearest neighbor(s) using double-precision
    /// floating-point keys.  Splitting dimension is chosen naively, by
    /// depth modulo K.  Semantics are as follows:
    ///
    ///  Two different keys containing identical numbers should retrieve the
    ///      same value from a given KD-tree.  Therefore keys are cloned when a
    ///      node is inserted.
    ///
    ///  As with Hashtables, values inserted into a KD-tree are <I>not</I>
    ///      cloned.  Modifying a value between insertion and retrieval will
    ///      therefore modify the value stored in the tree.
    ///
    ///
    /// @author Simon Levy, Bjoern Heckel
    /// Translation by Marco A. Alvarez
    /// Adapted by Ted Dunsford to better suite the dot net framework by
    /// changing comments to work in intellisense and extending some of the
    /// basic objects to work more tightly with MapWindow.
    /// </summary>
    public class KdTree
    {
        #region Private Variables

        // K = number of dimensions
        private readonly int _k;

        // root of KD-tree

        // count of nodes
        private int _count;
        private KdNode _root;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new tree with the specified number of dimensions.
        /// </summary>
        /// <param name="k">An integer value specifying how many ordinates each key should have.</param>
        public KdTree(int k)
        {
            if (k < 0) throw new NegativeArgumentException("k");
            _k = k;
            _root = null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Insert a node into the KD-tree.
        /// </summary>
        /// <param name="key">The array of double valued keys marking the position to insert this object into the tree</param>
        /// <param name="value">The object value to insert into the tree</param>
        /// <exception cref="KeySizeException"> if key.length mismatches the dimension of the tree (K)</exception>
        /// <exception cref="KeyDuplicateException"> if the key already exists in the tree</exception>
        /// <remarks>
        /// Uses algorithm translated from 352.ins.c of
        ///   &#064;Book{GonnetBaezaYates1991,
        ///   author =    {G.H. Gonnet and R. Baeza-Yates},
        ///   title =     {Handbook of Algorithms and Data Structures},
        ///   publisher = {Addison-Wesley},
        ///   year =      {1991}
        /// </remarks>
        public void Insert(double[] key, object value)
        {
            if (key.Length != _k) throw new KeySizeException();

            _root = KdNode.Insert(new HPoint(key), value, _root, 0, _k);

            _count++;
        }

        /// <summary>
        /// Find the KD-tree node whose key is identical to the specified key.
        /// This uses the algorithm translated from 352.srch.c of Connet and Baeza-Yates.
        /// </summary>
        /// <param name="key">The key identifying the node to search for</param>
        /// <returns>An object that is the node with a matching key, or null if no key was found.</returns>
        /// <exception cref="KeySizeException"> if key.length mismatches the dimension of the tree (K)</exception>
        public object Search(double[] key)
        {
            if (key.Length != _k) throw new KeySizeException();

            KdNode kd = KdNode.Search(new HPoint(key), _root, _k);

            return (kd == null ? null : kd.V);
        }

        /// <summary>
        /// Deletes a node from the KD-tree.  Instead of actually deleting the node and
        /// rebuilding the tree, it marks the node as deleted.  Hence, it is up to the
        /// caller to rebuild the tree as needed for efficiency.
        /// </summary>
        /// <param name="key">The key to use to identify the node to delete</param>
        /// <exception cref="KeySizeException"> if key.length mismatches the dimension of the tree (K)</exception>
        /// <exception cref="KeyMissingException"> if the key was not found in the tree</exception>
        public void Delete(double[] key)
        {
            if (key.Length != _k) throw new KeySizeException();

            KdNode t = KdNode.Search(new HPoint(key), _root, _k);

            if (t == null) throw new KeyMissingException();

            t.IsDeleted = true;

            _count--;
        }

        /// <summary>
        /// Find KD-tree node whose key is nearest neighbor to key.
        /// Implements the Nearest Neighbor algorithm (Table 6.4) of
        /// </summary>
        /// <param name="key">key for KD-tree node</param>
        /// <returns>object at node nearest to key, or null on failure</returns>
        /// <exception cref="KeySizeException"> if key.length mismatches the dimensions of the tree (K)</exception>
        /// <remarks>
        ///  &#064;techreport{AndrewMooreNearestNeighbor,
        ///    author  = {Andrew Moore},
        ///    title   = {An introductory tutorial on kd-trees},
        ///    institution = {Robotics Institute, Carnegie Mellon University},
        ///    year    = {1991},
        ///    number  = {Technical Report No. 209, Computer Laboratory,
        ///               University of Cambridge},
        ///    address = {Pittsburgh, PA}
        /// </remarks>
        public object Nearest(double[] key)
        {
            object[] neighbors = Nearest(key, 1);
            return neighbors[0];
        }

        /// <summary>
        /// Find KD-tree nodes whose keys are <I>n</I> nearest neighbors to
        /// key. Uses algorithm above.  Neighbors are returned in ascending
        /// order of distance to key.
        /// </summary>
        /// <param name="key">key for KD-tree node</param>
        /// <param name="numNeighbors">The Integer showing how many neighbors to find</param>
        /// <returns>An array of objects at the node nearest to the key</returns>
        /// <exception cref="KeySizeException">Mismatch if key length doesn't match the dimension for the tree</exception>
        /// <exception cref="NeighborsOutOfRangeException">if <I>n</I> is negative or exceeds tree size </exception>
        public object[] Nearest(double[] key, int numNeighbors)
        {
            if (numNeighbors < 0 || numNeighbors > _count) throw new NeighborsOutOfRangeException();

            if (key.Length != _k) throw new KeySizeException();

            object[] neighbors = new object[numNeighbors];
            NearestNeighborList nnl = new NearestNeighborList(numNeighbors);

            // initial call is with infinite hyper-rectangle and max distance
            HRect hr = HRect.InfiniteHRect(key.Length);
            const double maxDistSqd = 1.79769e+30;
            HPoint keyp = new HPoint(key);

            KdNode.Nnbr(_root, keyp, hr, maxDistSqd, 0, _k, nnl);

            for (int i = 0; i < numNeighbors; ++i)
            {
                KdNode kd = (KdNode)nnl.RemoveHighest();
                neighbors[numNeighbors - i - 1] = kd.V;
            }

            return neighbors;
        }

        /// <summary>
        /// Reverses the conventional nearest neighbor in order to obtain the furthest neighbor instead.
        /// </summary>
        /// <param name="key">The key for the KD-tree node</param>
        /// <returns>The object that corresponds to the furthest object</returns>
        public object Farthest(double[] key)
        {
            object[] neighbors = Farthest(key, 1);
            return neighbors[0];
        }

        /// <summary>
        /// Find KD-tree nodes whose keys are <I>n</I> farthest neighbors from
        /// key.  Neighbors are returned in descending order of distance to key.
        /// </summary>
        /// <param name="key">key for KD-tree node</param>
        /// <param name="numNeighbors">The Integer showing how many neighbors to find</param>
        /// <returns>An array of objects at the node nearest to the key</returns>
        /// <exception cref="KeySizeException">Mismatch if key length doesn't match the dimension for the tree</exception>
        /// <exception cref="NeighborsOutOfRangeException">if <I>n</I> is negative or exceeds tree size </exception>
        public object[] Farthest(double[] key, int numNeighbors)
        {
            if (numNeighbors < 0 || numNeighbors > _count) throw new NeighborsOutOfRangeException();

            if (key.Length != _k) throw new KeySizeException();

            object[] neighbors = new object[numNeighbors];
            FarthestNeighborList fnl = new FarthestNeighborList(numNeighbors);

            // initial call is with infinite hyper-rectangle and max distance
            HRect hr = HRect.InfiniteHRect(key.Length);
            //double max_dist_sqd = 1.79769e+30; //Double.MaxValue;
            HPoint keyp = new HPoint(key);
            KdNode.FarthestNeighbor(_root, keyp, hr, 0, 0, _k, fnl);
            //KDNode.nnbr(_root, keyp, hr, max_dist_sqd, 0, _k, nnl);

            for (int i = 0; i < numNeighbors; ++i)
            {
                KdNode kd = (KdNode)fnl.RemoveFarthest();
                neighbors[numNeighbors - i - 1] = kd.V;
            }

            return neighbors;
        }

        /// <summary>
        /// Search a range in the KD-tree.
        /// </summary>
        /// <param name="lowKey">The lower bound in all ordinates for keys</param>
        /// <param name="highKey">Teh upper bound in all ordinates for keys</param>
        /// <returns>An array of objects whose keys fall in range [lowk, uppk]</returns>
        /// <remarks> Range search in a KD-tree.  Uses algorithm translated from 352.range.c of Gonnet and Baeza-Yates.</remarks>
        /// <exception cref="KeySizeException">Mismatch of the specified parameters compared with the tree or each other.</exception>
        public object[] SearchRange(double[] lowKey, double[] highKey)
        {
            if (lowKey.Length != highKey.Length) throw new KeySizeException();

            if (lowKey.Length != _k) throw new KeySizeException();

            List<KdNode> v = new List<KdNode>();

            KdNode.SearchRange(new HPoint(lowKey), new HPoint(highKey), _root, 0, _k, v);
            object[] o = new object[v.Count];
            for (int i = 0; i < v.Count; ++i)
            {
                KdNode n = v[i];
                o[i] = n.V;
            }
            return o;
        }

        /// <summary>
        /// Converts the value to a string
        /// </summary>
        /// <returns>A string</returns>
        public override string ToString()
        {
            return _root.ToString(0);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of members in this tree.
        /// </summary>
        public int Count
        {
            get { return _count; }
        }

        /// <summary>
        /// Gets the actual number of dimensions for this tree.
        /// </summary>
        public int K
        {
            get { return _k; }
        }

        #endregion
    }
}