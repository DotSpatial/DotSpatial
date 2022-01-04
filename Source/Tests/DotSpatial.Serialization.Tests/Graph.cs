// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Serialization.Tests
{
    /// <summary>
    /// The graph.
    /// </summary>
    public class Graph
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Graph"/> class.
        /// </summary>
        public Graph()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Graph"/> class.
        /// </summary>
        /// <param name="root">The root.</param>
        public Graph(Node root)
        {
            Root = root;
        }

        /// <summary>
        /// Gets or sets the root.
        /// </summary>
        [Serialize("Root")]
        public Node Root { get; set; }
    }
}