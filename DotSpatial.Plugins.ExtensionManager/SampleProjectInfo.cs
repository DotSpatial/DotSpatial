using DotSpatial.Extensions;
using System;
namespace DotSpatial.Plugins.ExtensionManager
{
    public class SampleProjectInfo : ISampleProject
    {
        public string Name {
            get;
            set;
        }
        public string AbsolutePathToProjectFile {
            get;
            set;
        }
        public string Description {
            get;
            set;
        }
        public string Version {
            get;
            set;
        }
    }
}
