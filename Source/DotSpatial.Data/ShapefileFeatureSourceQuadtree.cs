// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections;
using GeoAPI.Geometries;
using NetTopologySuite.Index.Quadtree;

namespace DotSpatial.Data
{
    /// <summary>
    /// Spatial index customized for ShapefileFeatureSources.
    /// </summary>
    public class ShapefileFeatureSourceQuadtree : Quadtree<int>
    {
        /// <summary>
        /// Remove the row.
        /// </summary>
        /// <param name="itemEnv">The Envelope of the item to be removed.</param>
        /// <param name="item">The item to remove.</param>
        /// <returns>True, if the item was removed.</returns>
        public bool Remove(Envelope itemEnv, int item)
        {
            bool retValue = base.Remove(itemEnv, item); // TODO Do we need to adjust? I didn't know how to change that when I upgraded the NTS quellcode because I couldn't figure out how to call this code. (2015-08-24 jany_)

            // if (retValue)
            //    AdjustNodesForDeletedItem(Root, item);
            return retValue;
        }

        ///// <summary>
        ///// When a row is deleted, all other row numbers must be adjusted to compensate for the fact that the shx file gets compressed.
        ///// </summary>
        ///// <param name="node"></param>
        ///// <param name="deletedItem"></param>
        // private static void AdjustNodesForDeletedItem(Node<object> node, int deletedItem)
        // {
        //    if (node.HasItems)
        //    {
        //        IList items = new ArrayList(node.Items.Count);
        //        foreach (int item in node.Items)
        //        {
        //            if (item > deletedItem)
        //                items.Add(item - 1);
        //            else
        //                items.Add(item);
        //        }
        //        node.Items = items;
        //    }
        //    foreach (Node<object> childNode in node.Items)
        //    {
        //        if (null != childNode)
        //            AdjustNodesForDeletedItem(childNode, deletedItem);
        //    }
        // }
    }
}