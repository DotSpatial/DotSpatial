// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;

namespace DotSpatial.Serialization.Tests
{
    /// <summary>
    /// Node.
    /// </summary>
    public class Node
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        public Node()
        {
            Data = null;
            Nodes = new List<Node>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public Node(object data)
        {
            Data = data;
            Nodes = new List<Node>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="nodes">The nodes.</param>
        public Node(object data, List<Node> nodes)
        {
            Data = data;
            Nodes = nodes;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        [Serialize("Data")]
        public object Data { get; set; }

        /// <summary>
        /// Gets or sets the nodes.
        /// </summary>
        [Serialize("Nodes")]
        public List<Node> Nodes { get; set; }

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

            if (Data == null) return ((Node)obj).Data == null;

            return Data.Equals(((Node)obj).Data);
        }

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            if (Data == null) return Nodes.Count;

            return Data.GetHashCode() ^ Nodes.Count;
        }

        #endregion
    }
}