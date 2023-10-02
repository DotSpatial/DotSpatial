// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel.Composition;
using DotSpatial.Controls.Header;

namespace DummyHeaderControl
{
    /// <summary>
    /// This is an examplary empty HeaderControl.
    /// </summary>
    [Export(typeof(IHeaderControl))]
    public class DummyHeaderControl : IHeaderControl
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public DropDownActionItem MapServiceDropDown { get; private set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public object Add(HeaderItem item)
        {
            switch (item.Key)
            {
                case "kServiceDropDown":
                    {
                        MapServiceDropDown = item as DropDownActionItem;
                        return MapServiceDropDown;
                    }
            }

            return null;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Remove(string key)
        {
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void RemoveAll()
        {
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void SelectRoot(string key)
        {
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event EventHandler<RootItemEventArgs> RootItemSelected;
    }
}