using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotSpatial.Controls;
using DotSpatial.Modeling.Forms;

namespace DotSpatial.Plugins.ExtensionManager
{
    public class ToolsCategory : IExtensionCategory
    {
        public string Name { get { return "Tools"; } }

        public AppManager App { get; set; }

        public IEnumerable<Tuple<object, bool>> GetItems()
        {
            foreach (ITool tool in App.CompositionContainer.GetExportedValues<ITool>())
            {
                yield return new Tuple<object, bool>(tool, true);
            }
        }
    }
}