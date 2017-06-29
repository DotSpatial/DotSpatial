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
        #region Constructor

        /// <summary>
        /// Creates an instance of this class
        /// </summary>
        private AuthorityCodeHandler()
        {
            ReadDefault();
            ReadCustom();
        }

        #endregion

        #region Fields

        private static readonly Lazy<AuthorityCodeHandler> _lazyInstance = new Lazy<AuthorityCodeHandler>(() => new AuthorityCodeHandler(), true);
        private readonly IDictionary<string, ProjectionInfo> _authorityCodeToProjectionInfo = new Dictionary<string, ProjectionInfo>();
        private readonly IDictionary<string, ProjectionInfo> _authorityNameToProjectionInfo = new Dictionary<string, ProjectionInfo>();

        #endregion


        /// <summary>
        /// The one and only <see cref="AuthorityCodeHandler"/>
        /// </summary>
        public static AuthorityCodeHandler Instance
        {
            get { return _lazyInstance.Value; }
        }

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
            using (var s =
                Assembly.GetCallingAssembly().GetManifestResourceStream(
                    "DotSpatial.Projections.AuthorityCodes.AuthorityCodeToProj4.ds"))
            {
                using (var msUncompressed = new MemoryStream())
                {
                    using (var ds = new DeflateStream(s, CompressionMode.Decompress, true))
                    {
                        //replaced by jirikadlec2 to compile for .NET Framework 3.5
                        //ds.CopyTo(msUncompressed);
                        var buffer = new byte[4096];
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
            var fileName = Assembly.GetCallingAssembly().Location + "\\AdditionalProjections.proj4";
            if (File.Exists(fileName))
            {
                ReadFromStream(File.OpenRead(fileName), true);
            }
        }

        private void ReadFromStream(Stream s, bool replace)
        {
            using (var sr = new StreamReader(s))
            {
                var seperator = new[] { '\t' };
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                        continue;

                    var parts = line.Split(seperator, 3);
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
            var authorityCode = string.Format("{0}:{1}", authority, code);
            Add(authorityCode, name, proj4String, replace);
            AddToAdditionalProjections(authorityCode, name, proj4String);
        }

        private static void AddToAdditionalProjections(string authorityCode, string name, string proj4String)
        {
            var fileName = Assembly.GetCallingAssembly().Location + "\\AdditionalProjections.proj4";

            var fm = File.Exists(fileName) ? FileMode.Append : FileMode.CreateNew;
            using (var fileStream = File.Open(fileName, fm, FileAccess.Write, FileShare.None))
            using (var sw = new StreamWriter(fileStream, Encoding.ASCII))
                sw.WriteLine("{0}\t{1}\t{2}", authorityCode, name, proj4String);
        }

        private void Add(string authorityCode, string name, string proj4String, bool replace)
        {
            var pos = authorityCode.IndexOf(':');
            if (pos == -1)
                throw new ArgumentOutOfRangeException("authorityCode", "Invalid authorityCode");

            if (!replace && _authorityCodeToProjectionInfo.ContainsKey(authorityCode))
            {
                throw new ArgumentOutOfRangeException("authorityCode", "Such projection already added.");
            }
             var pi = ProjectionInfo.FromProj4String(proj4String);
             pi.Authority = authorityCode.Substring(0, pos);
             pi.AuthorityCode = int.Parse(authorityCode.Substring(pos + 1));
             pi.EpsgCode = int.Parse(authorityCode.Substring(pos + 1));
             pi.Name = String.IsNullOrEmpty(name) ? authorityCode : name;

             _authorityCodeToProjectionInfo[authorityCode] =  pi;

            if (string.IsNullOrEmpty(name))
                return;

            if (!replace && _authorityNameToProjectionInfo.ContainsKey(name))
            {
                throw new ArgumentOutOfRangeException("name", "Such projection already added.");
            }
            _authorityNameToProjectionInfo[name] = pi;
        }
    }
}