// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Ionic.Zip;

namespace DotSpatial.Plugins.MapWindowProjectFileCompatibility
{
    /// <summary>
    /// Used to Package a project into a zip file.
    /// </summary>
    public class ArchiveSerializer
    {
        #region Methods

        /// <summary>
        /// Saves the specified xml and referenced layers into a zip file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="xml">The Project XML.</param>
        public static void Save(string fileName, string xml)
        {
            using (ZipFile zip = new ZipFile())
            {
                // Assume for now that all paths are appropriate, TODO: remap paths in dspx.
                zip.AddEntry("theProject.dspx", xml);

                XDocument xmlDoc = XDocument.Parse(xml);
                var files = from f in xmlDoc.Descendants("member") where f.Attribute("name").Value == "FilePath" select f.Attribute("value").Value;

                var filesToInclude = GetRelatedFiles(files);
                foreach (string file in filesToInclude)
                {
                    zip.AddFile(file);
                }

                zip.Save(fileName);
            }
        }

        /// <summary>
        /// Gets the related metadata files (e.g., includes shx and dbf when given a shp file).
        /// </summary>
        /// <param name="files">The files.</param>
        /// <returns>The related metadata files (e.g., includes shx and dbf when given a shp file).</returns>
        private static IEnumerable<string> GetRelatedFiles(IEnumerable<string> files)
        {
            List<string> shapeExtentions = new List<string> { "dbf", "shx", "shpxml", "lbl" };
            List<string> metadataExtentions = new List<string> { "prj", "xml", "mwsr", "mwleg", "bmp", "bpw", "jgw", "gfw", "aux", "rrd" };

            foreach (string file in files)
            {
                yield return file;

                var directory = Path.GetDirectoryName(file);
                if (directory != null)
                {
                    string basePath = Path.Combine(directory, Path.GetFileNameWithoutExtension(file));

                    IEnumerable<string> extensions = metadataExtentions;
                    if (Path.GetExtension(file).ToLower().EndsWith(".shp"))
                    {
                        extensions = extensions.Concat(shapeExtentions);
                    }

                    foreach (string ext in extensions)
                    {
                        string relatedFile = $"{basePath}.{ext}";
                        if (File.Exists(relatedFile)) yield return relatedFile;
                    }
                }
            }
        }

        #endregion
    }
}