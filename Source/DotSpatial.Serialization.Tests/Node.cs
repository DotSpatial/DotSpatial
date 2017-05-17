// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;

namespace DotSpatial.Serialization.Tests
{
    /// <summary>
    /// Node
    /// </summary>
    public class Node
    {
        #region Fields

        [Serialize("Data")]
        private readonly object _data;

        [Serialize("Nodes")]
        private readonly List<Node> _nodes;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        public Node()
        {
            _data = null;
            _nodes = new List<Node>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public Node(object data)
        {
            _data = data;
            _nodes = new List<Node>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="nodes">The nodes.</param>
        public Node(object data, List<Node> nodes)
        {
            _data = data;
            _nodes = nodes;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the data.
        /// </summary>
        public object Data => _data;

        /// <summary>
        /// Gets the nodes.
        /// </summary>
        public List<Node> Nodes => _nodes;

        #endregion

        #region Methods

        /// <summary>
        /// Checks whether the given object equals this.
        /// </summary>
        /// <param name="obj">Object to check against.</param>
        /// <returns>True, if both are equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (_data == null) return ((Node)obj).Data == null;

            return _data.Equals(((Node)obj).Data);
        }

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            if (_data == null) return _nodes.Count;

            return _data.GetHashCode() ^ _nodes.Count;
        }

        #endregion
    }
}