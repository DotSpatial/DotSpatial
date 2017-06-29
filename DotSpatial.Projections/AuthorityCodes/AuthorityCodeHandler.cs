// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is also protected by the GNU Lesser Public License and the sourcecode
// for the Net Topology Suite can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |         Name         |    Date    |                              Comment
// |----------------------|------------|------------------------------------------------------------
// |                      |            |
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;

namespace DotSpatial.Projections.AuthorityCodes
{
    /// <summary>
    /// AuthorityCodeHandler
    /// </summary>
    public sealed class AuthorityCodeHandler
    {
        /// <summary>
        /// The one and only <see cref="AuthorityCodeHandler"/>
        /// </summary>
        public static readonly AuthorityCodeHandler Instance;

        static AuthorityCodeHandler()
        {
            Instance = new AuthorityCodeHandler();
        }

        /// <summary>
        /// Creates an instance of this class
        /// </summary>
        private AuthorityCodeHandler()
        {
            ReadDefault();
            ReadCustom();
        }

        #region Fields

        private readonly IDictionary<string, ProjectionInfo> _authorityCodeToProjectionInfo = new Dictionary<string, ProjectionInfo>();
        private readonly IDictionary<string, ProjectionInfo> _authorityNameToProjectionInfo = new Dictionary<string, ProjectionInfo>();

        #endregion

        /// <summary>
        /// Gets the
        /// </summary>
        /// <param name="authorityCodeOrName"></param>
        /// <returns></returns>
        public ProjectionInfo this[string authorityCodeOrName]
        {
            get
            {
                ProjectionInfo pi;
                if (_authorityCodeToProjectionInfo.TryGetValue(authorityCodeOrName, out pi))
                    return pi;
                if (_authorityNameToProjectionInfo.TryGetValue(authorityCodeOrName, out pi))
                    return pi;
                return null;
            }
        }

        private void ReadDefault()
        {
            using (Stream s =
                Assembly.GetCallingAssembly().GetManifestResourceStream(
                    "DotSpatial.Projections.AuthorityCodes.AuthorityCodeToProj4.ds"))
            {
                using (MemoryStream msUncompressed = new MemoryStream())
                {
                    using (DeflateStream ds = new DeflateStream(s, CompressionMode.Decompress, true))
                    {
                        //replaced by jirikadlec2 to compile for .NET Framework 3.5
                        //ds.CopyTo(msUncompressed);
                        byte[] buffer = new byte[4096];
                        int numRead;
                        while ((numRead = ds.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            msUncompressed.Write(buffer, 0, numRead);
                        }
                    }
                    msUncompressed.Seek(0, SeekOrigin.Begin);
                    ReadFromStream(msUncompressed, false);
                }
            }
        }

        private void ReadCustom()
        {
            string fileName = Assembly.GetCallingAssembly().Location + "\\AdditionalProjections.proj4";
            if (File.Exists(fileName))
            {
                ReadFromStream(File.OpenRead(fileName), true);
            }
        }

        private void ReadFromStream(Stream s, bool replace)
        {
            using (StreamReader sr = new StreamReader(s))
            {
                char[] seperator = new[] { '\t' };
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                        continue;

                    string[] parts = line.Split(seperator, 3);
                    if (parts.Length > 1)
                    {
                        if (parts.Length == 2)
                            Add(parts[0], string.Empty, parts[1], replace);
                        else
                            Add(parts[0], parts[1], parts[2], replace);
                    }
                }
            }
        }

        /// <summary>
        /// Adds the specified authority.
        /// </summary>
        /// <param name="authority">The authority.</param>
        /// <param name="code">The code.</param>
        /// <param name="proj4String">The proj4 string.</param>
        public void Add(string authority, int code, string proj4String)
        {
            Add(authority, code, proj4String, false);
        }

        /// <summary>
        /// Adds the specified authority.
        /// </summary>
        /// <param name="authority">The authority.</param>
        /// <param name="code">The code.</param>
        /// <param name="proj4String">The proj4 string.</param>
        /// <param name="replace">if set to <c>true</c> [replace].</param>
        public void Add(string authority, int code, string proj4String, bool replace)
        {
            Add(authority, code, string.Empty, proj4String, replace);
        }

        /// <summary>
        /// Adds the specified authority.
        /// </summary>
        /// <param name="authority">The authority.</param>
        /// <param name="code">The code.</param>
        /// <param name="name">The name.</param>
        /// <param name="proj4String">The proj4 string.</param>
        public void Add(string authority, int code, string name, string proj4String)
        {
            Add(authority, code, name, proj4String, false);
        }

        /// <summary>
        /// Adds a new projection info to the store, replaces the old one
        /// </summary>
        /// <param name="authority">The authority, e.g. EPSG</param>
        /// <param name="code">The code assigned by the authority</param>
        /// <param name="name">A declarative name</param>
        /// <param name="proj4String">the proj4 definition string</param>
        /// <param name="replace">a value indicating if a previously defined projection should be replaced or not.</param>
        public void Add(string authority, int code, string name, string proj4String, bool replace)
        {
            string authorityCode = string.Format("{0}:{1}", authority, code);
            Add(authorityCode, name, proj4String, replace);
            AddToAdditionalProjections(authorityCode, name, proj4String);
        }

        private static void AddToAdditionalProjections(string authorityCode, string name, string proj4String)
        {
            string fileName = Assembly.GetCallingAssembly().Location + "\\AdditionalProjections.proj4";

            FileMode fm = File.Exists(fileName) ? FileMode.Append : FileMode.CreateNew;
            using (FileStream fileStream = File.Open(fileName, fm, FileAccess.Write, FileShare.None))
            using (StreamWriter sw = new StreamWriter(fileStream, Encoding.ASCII))
                sw.WriteLine(String.Format("{0}\t{1}\t{2}", authorityCode, name, proj4String));
        }

        private void Add(string authorityCode, string name, string proj4String, bool replace)
        {
            if (replace && _authorityCodeToProjectionInfo.ContainsKey(authorityCode))
                _authorityCodeToProjectionInfo.Remove(authorityCode);
            _authorityCodeToProjectionInfo.Add(authorityCode, ProjectionInfo.FromProj4String(proj4String));

            if (string.IsNullOrEmpty(name))
                return;

            if (replace && _authorityNameToProjectionInfo.ContainsKey(name))
                _authorityNameToProjectionInfo.Remove(name);
            _authorityNameToProjectionInfo.Add(name, ProjectionInfo.FromProj4String(proj4String));
        }
    }
}