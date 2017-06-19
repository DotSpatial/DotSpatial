using System;
using DotSpatial.Controls.Header;
using System.ComponentModel.Composition;

namespace DummyHeaderControl
{
    /// <summary>
    /// This is an examplary empty HeaderControl.
    /// </summary>
    [Export(typeof(IHeaderControl))]
    public class DummyHeaderControl : IHeaderControl
    {
        public DropDownActionItem MapServiceDropDown { get; private set; }
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

        public void Remove(string key)
        {
        }

        public void RemoveAll()
        {
        }

        public void SelectRoot(string key)
        {
        }

        public event EventHandler<RootItemEventArgs> RootItemSelected;
    }
}