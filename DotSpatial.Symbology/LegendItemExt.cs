// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/4/2009 8:35:43 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using System.Linq;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Contains extension methods for ILegendItems
    /// </summary>
    public static class LegendItemExt
    {
        #region Methods

        /// <summary>
        /// This method starts with this legend item and tests to see if it can contain
        /// the specified target.  As it moves up the
        /// </summary>
        /// <param name="startItem">This legend item</param>
        /// <param name="dropItem">The target legend item to test</param>
        /// <returns>An ILegendItem that is one of the parent items of this item, but that can receive the target.</returns>
        public static ILegendItem GetValidContainerFor(this ILegendItem startItem, ILegendItem dropItem)
        {
            if (startItem == null) return null;
            if (dropItem == null) return null;
            if (startItem.CanReceiveItem(dropItem)) return startItem;
            ILegendItem item = startItem;
            while ((item = item.GetParentItem()) != null)
            {
                if (item.CanReceiveItem(dropItem))
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// Given the starting position, which might not be able to contain the drop item,
        /// determine the index in the valid container where this item should end up.
        /// </summary>
        /// <param name="startItem">The legend item that may not be a sibling or be able to contain the drop item</param>
        /// <param name="dropItem">The item being added to the legend</param>
        /// <returns>The integer index of the valid container where insertion should occur when dropping onto this item.</returns>
        public static int InsertIndex(this ILegendItem startItem, ILegendItem dropItem)
        {
            ILegendItem container = GetValidContainerFor(startItem, dropItem);
            if (container == null) return -1;
            ILegendItem insertTarget = GetInsertTarget(startItem, dropItem);
            if (insertTarget == null) return container.LegendItems.Count();
            if (insertTarget == container)
            {
                return container.LegendItems.Count();
            }
            List<ILegendItem> items = container.LegendItems.ToList();
            if (items.Contains(insertTarget))
            {
                return items.IndexOf(insertTarget);
            }
            return container.LegendItems.Count();
        }

        private static ILegendItem GetInsertTarget(ILegendItem startItem, ILegendItem dropItem)
        {
            if (startItem == null) return null;
            if (dropItem == null) return null;
            if (startItem.CanReceiveItem(dropItem)) return startItem;
            ILegendItem item = startItem;
            while (item.GetParentItem() != null)
            {
                if (item.GetParentItem().CanReceiveItem(dropItem))
                {
                    return item;
                }
                item = item.GetParentItem();
            }
            return null;
        }

        /// <summary>
        /// Searches through the LegendItems recursively, looking for the 0 index
        /// member of the deepest part of the tree.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static ILegendItem BottomMember(this ILegendItem self)
        {
            if (self.LegendItems != null && self.LegendItems.Count() > 0)
            {
                return BottomMember(self.LegendItems.First());
            }
            return self;
        }

        /// <summary>
        /// This method starts with this legend item and searches through the
        /// parent groups until it finds a valid mapframe.
        /// </summary>
        /// <param name="self">The ILegendItem to start from</param>
        /// <returns>The IMapFrame that contains this item.</returns>
        public static IFrame ParentMapFrame(this ILegendItem self)
        {
            ILegendItem current = self;
            IFrame frame = current as IFrame;
            while (current != null)
            {
                if (frame != null) return frame;
                current = current.GetParentItem();
                frame = current as IFrame;
            }
            return null;
        }

        #endregion
    }
}