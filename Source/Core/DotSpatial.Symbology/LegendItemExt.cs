// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Linq;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Contains extension methods for ILegendItems.
    /// </summary>
    public static class LegendItemExt
    {
        #region Methods

        /// <summary>
        /// Searches through the LegendItems recursively, looking for the 0 index
        /// member of the deepest part of the tree.
        /// </summary>
        /// <param name="self">this.</param>
        /// <returns>The found member.</returns>
        public static ILegendItem BottomMember(this ILegendItem self)
        {
            if (self?.LegendItems != null && self.LegendItems.Any())
            {
                var items = self.LegendItems.ToList();

                for (int i = items.Count - 1; i >= 0; i--)
                {
                    if (items[i].LegendItemVisible)
                    {
                        if (items[i].IsExpanded)
                        {
                            return BottomMember(items[i]);
                        }

                        return items[i];
                    }
                }
            }

            return self;
        }

        /// <summary>
        /// Checks whether the item is a child item of the given parent.
        /// </summary>
        /// <param name="self">This legend item.</param>
        /// <param name="parent">The parent item.</param>
        /// <returns>True, if this is a child of the given parent.</returns>
        public static bool IsChildOf(this ILegendItem self, ILegendItem parent)
        {
            if (self == null || parent == null) return false;

            var item = self;
            while ((item = item.GetParentItem()) != null)
            {
                if (item == parent) return true;
            }

            return false;
        }

        /// <summary>
        /// This method starts with this legend item and tests to see if it can contain
        /// the specified target. As it moves up the.
        /// </summary>
        /// <param name="startItem">This legend item.</param>
        /// <param name="dropItem">The target legend item to test.</param>
        /// <returns>An ILegendItem that is one of the parent items of this item, but that can receive the target.</returns>
        public static ILegendItem GetValidContainerFor(this ILegendItem startItem, ILegendItem dropItem)
        {
            if (startItem == null || dropItem == null) return null;
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
        /// Gets the index at which the positionBase is situated in the parent. This can be used to determine the index the item that gets moved should be inserted.
        /// </summary>
        /// <param name="parent">The legend item the moving item should be added to.</param>
        /// <param name="positionBase">The item that is used to determine the new position of the moving item.</param>
        /// <returns>The new position of the moving item.</returns>
        public static int InsertIndex(this ILegendItem parent, ILegendItem positionBase)
        {
            if (parent == null || positionBase == null) return -1;

            ILegendItem item = positionBase;

            if (item == parent)
            {
                return item.LegendItems.Count();
            }

            while (item != null)
            {
                var p = item.GetParentItem();

                if (p == parent)
                {
                    var items = p.LegendItems.ToList();
                    return items.IndexOf(item);
                }

                item = p;
            }

            return -1;
        }

        /// <summary>
        /// This method starts with this legend item and searches through the
        /// parent groups until it finds a valid mapframe.
        /// </summary>
        /// <param name="self">The ILegendItem to start from.</param>
        /// <returns>The IMapFrame that contains this item.</returns>
        public static IFrame ParentMapFrame(this ILegendItem self)
        {
            ILegendItem current = self;
            while (current != null)
            {
                if (current is IFrame frame) return frame;

                current = current.GetParentItem();
                frame = current as IFrame;
            }

            return null;
        }

        #endregion
    }
}