using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotSpatial.Controls;
using DotSpatial.Extensions;

namespace DotSpatial.Plugins.ExtensionManager
{
    public class ExtensionCategory : IExtensionCategory
    {
        public string Name { get { return "Extensions"; } }

        public AppManager App { get; set; }

        public IEnumerable<Tuple<object, bool>> GetItems()
        {
            foreach (var extension in App.Extensions.Where(t => t.DeactivationAllowed))
            {
                yield return new Tuple<object, bool>(extension, extension.IsActive);
            }
        }
    }
}