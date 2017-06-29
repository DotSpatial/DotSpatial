using System;
using System.Collections.Generic;
using System.Linq;
using DotSpatial.Controls;

namespace DotSpatial.Plugins.ExtensionManager
{
    public interface IExtensionCategory
    {
        string Name { get; }

        AppManager App { get; set; }

        IEnumerable<Tuple<object, bool>> GetItems();
    }
}