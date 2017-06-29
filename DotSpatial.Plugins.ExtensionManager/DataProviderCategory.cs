using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;

namespace DotSpatial.Plugins.ExtensionManager
{
    public class DataProviderCategory : IExtensionCategory
    {
        public string Name { get { return "Data Providers"; } }

        public AppManager App { get; set; }

        public IEnumerable<Tuple<object, bool>> GetItems()
        {
            foreach (IDataProvider provider in App.CompositionContainer.GetExportedValues<IDataProvider>())
            {
                yield return new Tuple<object, bool>(provider, true);
            }
        }
    }
}