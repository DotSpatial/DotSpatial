// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;

namespace DotSpatial.Data
{
    /// <summary>
    /// A named list preserves a 1:1 mapping between names and items. It can be used to
    /// reference information in either direction. It essentially provides a string
    /// handle for working with generic typed ILists. This cannot instantiate new
    /// items. (Creating a default T would not work, for instance, for an interface).
    /// </summary>
    /// <typeparam name="T">Type of the contained items.</typeparam>
    public class NamedList<T> : INamedList
    {
        #region Fields

        private readonly Dictionary<string, T> _items; // search by name
        private readonly Dictionary<T, string> _names; // search by item

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedList{T}"/> class.
        /// </summary>
        public NamedList()
        {
            Items = new List<T>(); // default setting
            _items = new Dictionary<string, T>();
            _names = new Dictionary<T, string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedList{T}"/> class.
        /// </summary>
        /// <param name="values">The values to use for the content.</param>
        public NamedList(IList<T> values)
        {
            Items = values;
            _items = new Dictionary<string, T>();
            _names = new Dictionary<T, string>();
            RefreshNames();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedList{T}"/> class.
        /// </summary>
        /// <param name="values">The values to use for the content.</param>
        /// <param name="baseName">The string that should precede the numbering to describe the individual items.</param>
        public NamedList(IList<T> values, string baseName)
        {
            Items = values;
            _items = new Dictionary<string, T>();
            _names = new Dictionary<T, string>();
            BaseName = baseName;
            RefreshNames();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the base name to use for naming items.
        /// </summary>
        public string BaseName { get; set; }

        /// <summary>
        /// Gets the count of the items in the list.
        /// </summary>
        public int Count => _items.Count;

        /// <summary>
        /// Gets or sets the list of actual items. This is basically a reference copy of
        /// the actual collection of items to be contained in this named list.
        /// </summary>
        public IList<T> Items { get; set; }

        #endregion

        #region Indexers

        /// <summary>
        /// Gets or sets the item corresponding to the specified name. Setting this
        /// will re-use the same name and position in the list, but set a new object.
        /// </summary>
        /// <param name="name">The string name of the item to obtain.</param>
        /// <returns>The item of type T corresponding to the specified name.</returns>
        public T this[string name]
        {
            get
            {
                return _items.ContainsKey(name) ? _items[name] : default(T);
            }

            set
            {
                T oldItem = _items[name];
                _names.Remove(oldItem);
                int index = Items.IndexOf(oldItem);
                Items.RemoveAt(index);
                Items.Insert(index, value);
                _items[name] = value;
                _names.Add(value, name);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Re-orders the list so that the index of the specifeid item is lower,
        /// and threfore will be drawn earlier, and therefore should appear
        /// in a lower position on the list.
        /// </summary>
        /// <param name="name">name of the item that gets demoted.</param>
        public void Demote(string name)
        {
            Demote(_items[name]);
        }

        /// <summary>
        /// Re-orders the list so that this item appears closer to the 0 index.
        /// </summary>
        /// <param name="item">Item that gets demoted.</param>
        public void Demote(T item)
        {
            int index = Items.IndexOf(item);
            if (index == -1 || index == 0) return;

            Items.RemoveAt(index);
            Items.Insert(index - 1, item);
        }

        /// <summary>
        /// Gets the item with the specified name as an object.
        /// This enables the INamedList to work with items even
        /// if it doesn't know the strong type.
        /// </summary>
        /// <param name="name">The string name of the item to retrieve.</param>
        /// <returns>The actual item cast as an object.</returns>
        public object GetItem(string name)
        {
            return this[name];
        }

        /// <summary>
        /// Gets the string name for the specified item.
        /// </summary>
        /// <param name="item">The item of type T to find the name for.</param>
        /// <returns>The string name corresponding to the specified item.</returns>
        public string GetName(T item)
        {
            return _names.ContainsKey(item) ? _names[item] : null;
        }

        /// <summary>
        /// Gets the name of the item corresponding.
        /// </summary>
        /// <param name="value">The item cast as an object.</param>
        /// <returns>The string name of the specified object, or null if the cast fails.</returns>
        public string GetNameOfObject(object value)
        {
            T item = Global.SafeCastTo<T>(value);
            return item == null ? null : GetName(item);
        }

        /// <summary>
        /// Gets the list of names for the items currently stored in the list,
        /// in the sequence defined by the list of items.
        /// </summary>
        /// <returns>The list of names.</returns>
        public string[] GetNames()
        {
            List<string> result = new List<string>();
            foreach (T item in Items)
            {
                if (!_names.ContainsKey(item))
                {
                    RefreshNames();
                    break;
                }
            }

            foreach (T item in Items)
            {
                result.Add(_names[item]);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Re-orders the list so that the index of the specified item is higher,
        /// and therefore will be drawn later, and therefore should appear
        /// in a higher position on the list.
        /// </summary>
        /// <param name="name">Name of the item that gets promoted.</param>
        public void Promote(string name)
        {
            Promote(_items[name]);
        }

        /// <summary>
        /// Re-orders the list so that the index of the specified item is higher,
        /// and therefore will be drawn later, and therefore should appear
        /// in a higher position on the list.
        /// </summary>
        /// <param name="item">Item that gets promoted.</param>
        public void Promote(T item)
        {
            int index = Items.IndexOf(item);
            if (index == -1) return;
            if (index == Items.Count - 1) return;

            Items.RemoveAt(index);
            Items.Insert(index + 1, item);
        }

        /// <summary>
        /// Updates the names to match the current set of actual items.
        /// </summary>
        public void RefreshNames()
        {
            // When re-ordering, we want to keep the name like category 0 the same,
            // so we can't just clear the values. Instead, to see the item move,
            // the name has to stay with the item.
            List<T> deleteItems = new List<T>();
            foreach (T item in _names.Keys)
            {
                if (Items.Contains(item) == false)
                {
                    deleteItems.Add(item);
                }
            }

            foreach (T item in deleteItems)
            {
                _names.Remove(item);
            }

            List<string> deleteNames = new List<string>();
            foreach (string name in _items.Keys)
            {
                if (_names.ContainsValue(name) == false)
                {
                    deleteNames.Add(name);
                }
            }

            foreach (string name in deleteNames)
            {
                _items.Remove(name);
            }

            foreach (T item in Items)
            {
                if (!_names.ContainsKey(item))
                {
                    string name = BaseName + 0;
                    int i = 1;
                    while (_items.ContainsKey(name))
                    {
                        name = BaseName + i;
                        i++;
                    }

                    _names.Add(item, name);
                    _items.Add(name, item);
                }
            }
        }

        /// <summary>
        /// Removes the item with the specified name from the list.
        /// </summary>
        /// <param name="name">The string name of the item to remove.</param>
        public void Remove(string name)
        {
            Remove(_items[name]);
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        public void Remove(T item)
        {
            Items.Remove(item);
            RefreshNames();
        }

        #endregion
    }
}