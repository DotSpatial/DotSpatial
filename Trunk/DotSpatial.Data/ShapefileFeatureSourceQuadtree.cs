// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial
//
// The Initial Developer of this Original Code is Kyle Ellison. Created 04/08/2011.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |-----------------|----------|---------------------------------------------------------------------
// |      Name       |  Date    |                        Comments
// |-----------------|----------|----------------------------------------------------------------------
// |-----------------|----------|----------------------------------------------------------------------
// ********************************************************************************************************

using System.Xml.Serialization;
using DotSpatial.Topology.Geometries;
using DotSpatial.Topology.Index.QuadTree;

namespace DotSpatial.Data
{
    /// <summary>
    /// Spatial index customized for ShapefileFeatureSources.
    /// </summary>
    public class ShapefileFeatureSourceQuadtree : Quadtree<int>
    {
       /// <summary>
        /// Remove the row
        /// </summary>
        /// <param name="itemEnv"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(IEnvelope itemEnv, int item)
        {
            bool retValue = base.Remove(itemEnv, item); //TODO Do we need to adjust? I didn't know how to change that when I upgraded the NTS quellcode because I couldn't figure out how to call this code. (2015-08-24 jany_)
            //if (retValue)
            //    AdjustNodesForDeletedItem(Root, item);
            return retValue;
        }

        ///// <summary>
        ///// When a row is deleted, all other row numbers must be adjusted to compensate for the fact that the shx file gets compressed.
        ///// </summary>
        ///// <param name="node"></param>
        ///// <param name="deletedItem"></param>
        //private static void AdjustNodesForDeletedItem(Node<object> node, int deletedItem)
        //{
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
        //}
    }
}