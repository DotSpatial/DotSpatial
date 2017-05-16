﻿// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace DotSpatial.Controls.Header
{
    /// <summary>
    /// HeaderControl which takes care of implementing RemoveItems.
    /// </summary>
    public abstract class HeaderControl : IHeaderControl
    {
        #region Fields

        /// <summary>
        /// The key of the Application Menu item.
        /// </summary>
        public const string ApplicationMenuKey = "kApplicationMenu";

        /// <summary>
        /// Used as the group caption for icons that should appear near the ribbon
        /// maximize/minimize chevron.
        /// </summary>
        public const string HeaderHelpItemKey = "kHeaderHelpItemKey";

        /// <summary>
        /// The key of the home root item.
        /// </summary>
        public const string HomeRootItemKey = "kHome";

        /// <summary>
        /// A key to use for the root container of any extensions that do not provider a root key.
        /// </summary>
        protected const string ExtensionsRootKey = "kExtensions";

        private readonly Dictionary<string, HeaderItemDesc> _items = new Dictionary<string, HeaderItemDesc>();

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a root item is selected
        /// </summary>
        public event EventHandler<RootItemEventArgs> RootItemSelected;

        #endregion

        #region Methods

        /// <summary>
        /// This will add a new item that will appear on the standard toolbar or ribbon control.
        /// </summary>
        /// <param name="item">Item taht gets added.</param>
        /// <remarks>Should only be called by the plugin (from the plugin assembly).</remarks>
        /// <returns>The added item.</returns>
        public virtual object Add(HeaderItem item)
        {
            Contract.Requires(item != null, "item is null.");

            HeaderItemDesc hid;
            if (_items.TryGetValue(item.Key, out hid))
            {
                throw new ArgumentException(string.Format("The key \"{0}\" was already added by {1}. The key may only be used by one HeaderItem.", item.Key, hid.AssemblyName));
            }

            // We don't add the root items to this list. The HeaderControl implementation should remove a root
            // automatically when all item in the root are removed.
            if (item is RootItem == false)
            {
                string assemblyName = Assembly.GetCallingAssembly().FullName;
                RecordItemAdd(item, assemblyName);
            }

            // Bypass static type checking until runtime.
            dynamic test = item;

            // The correct overload of Add will be called below as the specifc type of item is determined at runtime.
            // See http://msdn.microsoft.com/en-us/library/dd264736.aspx
            return Add(test);
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The added item.</returns>
        public abstract object Add(SimpleActionItem item);

        /// <summary>
        /// Adds the menu container item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The added item.</returns>
        public abstract object Add(MenuContainerItem item);

        /// <summary>
        /// Adds the specified root item.
        /// </summary>
        /// <param name="item">The root item.</param>
        /// <remarks>The RootItem should not be visible until it contains other items.</remarks>
        /// <returns>The added item.</returns>
        public abstract object Add(RootItem item);

        /// <summary>
        /// Adds a combo box style item
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The added item.</returns>
        public abstract object Add(DropDownActionItem item);

        /// <summary>
        /// Adds a visible separator.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The added item.</returns>
        public abstract object Add(SeparatorItem item);

        /// <summary>
        /// Adds the specified textbox item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The added item.</returns>
        public abstract object Add(TextEntryActionItem item);

        /// <summary>
        /// Remove item from the standard toolbar or ribbon control. Also removes groups or parents when all
        /// items have been removed from them.
        /// </summary>
        /// <param name="key">The string itemName to remove from the standard toolbar or ribbon control</param>
        /// <remarks>
        /// If passed a root item the behavior is not defined. The root item should never be empty because
        /// it will be removed when all of its child items are removed.
        /// </remarks>
        public virtual void Remove(string key)
        {
            _items.Remove(key);
        }

        /// <summary>
        /// Removes all items the plugin created by calling Remove() individually for each.
        /// </summary>
        /// <remarks>Should only be called by the plugin (from the plugin assembly).</remarks>
        public virtual void RemoveAll()
        {
            string assemblyName = Assembly.GetCallingAssembly().FullName;

            // create a copy of the enumeration so that we can remove items from the original collection.
            var toRemove = _items.Where(i => i.Value.AssemblyName == assemblyName).ToArray();
            foreach (var item in toRemove)
            {
                Remove(item.Key);
            }
        }

        /// <summary>
        /// Selects the root, making it the active root.
        /// </summary>
        /// <param name="key">The key.</param>
        public abstract void SelectRoot(string key);

        /// <summary>
        /// Gets header item by key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Header item or null if not found.</returns>
        protected HeaderItem GetHeaderItemByKey(string key)
        {
            HeaderItemDesc hid;
            if (_items.TryGetValue(key, out hid)) return hid.HeaderItem;
            return null;
        }

        /// <summary>
        /// Occurs when a root item is selected by the user.
        /// This event also occurs after the SelectRoot method is called.
        /// </summary>
        /// <param name="key">The key of the new selected root item</param>
        protected void OnRootItemSelected(string key)
        {
            RootItemSelected?.Invoke(this, new RootItemEventArgs(key));
        }

        /// <summary>
        /// Adds the item to dictionary so that it can be removed later.
        /// </summary>
        /// <param name="item">Item to add.</param>
        /// <param name="assemblyFullName">Full name of the assembly.</param>
        protected void RecordItemAdd(HeaderItem item, string assemblyFullName)
        {
            Contract.Requires(!string.IsNullOrEmpty(assemblyFullName), "assemblyFullName is null or empty.");
            Contract.Requires(!string.IsNullOrEmpty(item.Key), "key is null or empty.");

            _items.Add(item.Key, new HeaderItemDesc(item, assemblyFullName));
        }

        #endregion

        #region Classes

        private class HeaderItemDesc
        {
            #region  Constructors

            public HeaderItemDesc(HeaderItem headerItem, string assemblyName)
            {
                HeaderItem = headerItem;
                AssemblyName = assemblyName;
            }

            #endregion

            #region Properties

            public string AssemblyName { get; }

            public HeaderItem HeaderItem { get; }

            #endregion
        }

        #endregion
    }
}