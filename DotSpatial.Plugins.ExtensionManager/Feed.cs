using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotSpatial.Plugins.ExtensionManager
{
    public class Feed
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public bool IsValid()
        { return true; }
    }
}