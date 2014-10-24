using System;
using System.Collections.Generic;
using MonoMac.AppKit;
using MonoMac.Foundation;
using DotSpatial.Controls.MonoMac;

namespace MacDemoMap
{
    public class ToolBarDelegate : NSToolbarDelegate
    {
        private Map map;
        private List<NSToolbarItem> toolBarItemList = new List<NSToolbarItem>();

        public ToolBarDelegate (Map map)
        {
            this.map = map;
        }

        public override NSToolbarItem WillInsertItem(NSToolbar toolbar, string itemIdentifier, bool willBeInserted)
        {
            var toolBarItem = new NSToolbarItem (itemIdentifier) {Label = itemIdentifier, ToolTip = itemIdentifier, 
                PaletteLabel = itemIdentifier, Image = NSImage.ImageNamed ("hand_32x32.png")};
            toolBarItemList.Add (toolBarItem);
            toolBarItem.Activated += Item_Activated;
            return toolBarItem;
        }

        private void Item_Activated(Object sender, EventArgs e)
        {
            map.FunctionMode = DotSpatial.Controls.FunctionMode.Pan;
        }

        public override string[] DefaultItemIdentifiers(NSToolbar toolbar)
        {
            string[] identifiers = {"Pan"};
            return identifiers;
        }

        public override string[] AllowedItemIdentifiers(NSToolbar toolbar)
        {
            string[] identifiers = {"Pan"};
            return identifiers;
        }

        public override string[] SelectableItemIdentifiers(NSToolbar toolbar)
        {
            string[] identifiers = {};
            return identifiers;
        }

        public override void WillAddItem(NSNotification notification)
        {
        }

        public override void DidRemoveItem(NSNotification notification)
        {
        }
    }
}

