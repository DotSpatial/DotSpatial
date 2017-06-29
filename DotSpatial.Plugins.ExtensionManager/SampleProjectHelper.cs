using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using DotSpatial.Controls;

namespace DotSpatial.Plugins.ExtensionManager
{
    public class SampleProjectHelper
    {
        public IEnumerable<string> FindSampleProjectFiles() {
            return Directory.EnumerateFiles(AppManager.AbsolutePathToExtensions, "*.dspx");
        }
        public static string FindDefaultProjectFile(Assembly asm, string projectName) {
            string directoryName = Path.GetDirectoryName(asm.Location);
            string text = Path.Combine(directoryName, projectName);
            if (!File.Exists(text))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directoryName);
                string fullName = directoryInfo.Parent.Parent.FullName;
                string path = Path.Combine(fullName, "content");
                text = Path.Combine(path, projectName);
            }
            return text;
        }
    }
}
