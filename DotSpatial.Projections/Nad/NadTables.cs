// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/17/2009 4:52:04 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DotSpatial.Projections
{
    /// <summary>
    /// NadTables
    /// </summary>
    public class NadTables
    {
        #region Private Variables

        private readonly Dictionary<string, NadTable> _tables;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NadTables
        /// </summary>
        public NadTables()
        {
            _tables = new Dictionary<string, NadTable>();
            Assembly a = Assembly.GetExecutingAssembly();
            string[] names = a.GetManifestResourceNames();

            foreach (string s in names)
            {
                string[] ss = s.Split('.');
                string coreName = "@" + ss[ss.Length - 2];
                if (_tables.ContainsKey(coreName)) continue;
                string ext = Path.GetExtension(s).ToLower();
                if (ext != ".lla" && ext != ".dat" && ext != ".gsb") continue;
                Stream text = a.GetManifestResourceStream(s);
                if (text == null) continue;
                NadTable nt = NadTable.FromSourceName(s);
                _tables.Add(coreName, nt);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load grids from the default GeogTransformsGrids subfolder
        /// </summary>
        public void InitializeExternalGrids()
        {
            Assembly a = Assembly.GetExecutingAssembly();
            string strGridsFolder = Path.Combine(Path.GetDirectoryName(a.Location), "GeogTransformGrids");
            InitializeExternalGrids(strGridsFolder, true);
        }

        /// <summary>
        /// Load grids from the specified folder
        /// </summary>
        /// <param name="strGridsFolder"></param>
        /// <param name="recursive"></param>
        public void InitializeExternalGrids(string strGridsFolder, bool recursive)
        {
            SearchOption opt = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            string[] gridTypes = new[] { "*.los", "*.gsb", "*.dat", "*.lla" };
            List<string> names = new List<string>();
            foreach (string gridType in gridTypes)
            {
                string[] tmp = Directory.GetFiles(strGridsFolder, gridType, opt);
                names.AddRange(tmp);
            }

            foreach (string s in names)
            {
                string coreName = "@" + Path.GetFileNameWithoutExtension(s);
                if (_tables.ContainsKey(coreName)) continue;
                string ext = Path.GetExtension(s).ToLower();
                if (ext != ".los" && ext != ".gsb" && ext != ".dat") continue;
                if (ext == ".los" && !File.Exists(s.Replace(".los", ".las"))) continue;
                NadTable nt = NadTable.FromSourceName(s, false);
                _tables.Add(coreName, nt);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets an array of the lla tables that have been added as a resource
        /// </summary>
        public Dictionary<string, NadTable> Tables
        {
            get { return _tables; }
        }

        #endregion
    }
}