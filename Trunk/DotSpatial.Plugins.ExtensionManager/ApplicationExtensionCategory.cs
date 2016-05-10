using System;
using System.Collections.Generic;
using System.Linq;
using DotSpatial.Controls;

namespace DotSpatial.Plugins.ExtensionManager
{
    public class ApplicationExtensionCategory : IExtensionCategory
    {
        public string Name { get { return "App Extensions"; } }

        public AppManager App { get; set; }

        public IEnumerable<Tuple<object, bool>> GetItems()
        {
            foreach (var extension in App.Extensions.Where(t => !t.DeactivationAllowed))
            {
                yield return new Tuple<object, bool>(extension, extension.IsActive);
            }
        }
    }
}