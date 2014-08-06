using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotSpatial.Controls;
using DotSpatial.Extensions;
using System.IO;
using System.Windows.Forms;

namespace DotSpatial.Plugins.ExtensionManager
{
    public class DotSpatialCategory : IExtensionCategory
    {
        public string Name { get { return "DotSpatial"; } }

        public AppManager App { get; set; }

        public IEnumerable<Tuple<object, bool>> GetItems()
        {
            DirectoryInfo StartupPath = new DirectoryInfo(Application.StartupPath);
            FileInfo[] files = StartupPath.GetFiles("DotSpatial*");

            foreach (FileInfo file in files)
            {
                yield return new Tuple<object, bool>(file.Name, true);
            }
        }
    }
}