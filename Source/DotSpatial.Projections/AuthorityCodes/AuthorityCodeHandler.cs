// *******************************************************************************************************
// Product:   DotSpatial.Projections.AuthorityCodes.AuthorityCodeHandler
// Description:  Reads and holds projections per authority codes.
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------------
// *******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.Remoting;
using System.Text;

namespace DotSpatial.Projections.AuthorityCodes
{
    /// <summary>
    /// Reads and holds projections per authority codes.
    /// </summary>
    public sealed class AuthorityCodeHandler
    {
        #region Fields

        private static readonly Lazy<AuthorityCodeHandler> LazyInstance = new Lazy<AuthorityCodeHandler>(() => new AuthorityCodeHandler(), true);
        private readonly IDictionary<string, ProjectionInfo> _authorityCodeToProjectionInfo = new Dictionary<string, ProjectionInfo>();
        private readonly IDictionary<string, ProjectionInfo> _authorityNameToProjectionInfo = new Dictionary<string, ProjectionInfo>();

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of this class
        /// </summary>
        private AuthorityCodeHandler()
        {
            ReadDefault();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The one and only <see cref="AuthorityCodeHandler"/>
        /// </summary>
        public static AuthorityCodeHandler Instance
        {
            get { return LazyInstance.Value; }
        }

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

        #endregion

        #region Public methods

        /// <summary>
        /// Adds the specified authority.
        /// </summary>
        /// <param name="authority">The authority.</param>
        /// <param name="code">The code.</param>
        /// <param name="proj4String">The proj4 string.</param>
        /// <param name="replace">if set to <c>true</c> [replace].</param>
        public void Add(string authority, int code, string proj4String, bool replace = false)
        {
            Add(authority, code, string.Empty, proj4String, replace);
        }


        /// <summary>
        /// Adds a new projection info to the store, replaces the old one
        /// </summary>
        /// <param name="authority">The authority, e.g. EPSG</param>
        /// <param name="code">The code assigned by the authority</param>
        /// <param name="name">A declarative name</param>
        /// <param name="proj4String">the proj4 definition string</param>
        /// <param name="replace">a value indicating if a previously defined projection should be replaced or not.</param>
        public void Add(string authority, int code, string name, string proj4String, bool replace = false)
        {
            var authorityCode = $"{authority}:{code}";
            Add(authorityCode, name, proj4String, replace);
        }

        #endregion

        #region Private methods

        private void ReadDefault()
        {
            using (var str = DeflateStreamReader.DecodeEmbeddedResource("DotSpatial.Projections.AuthorityCodes.epsg.ds"))
            {
                ReadFromStream(str, "EPSG");
            }
            using (var str = DeflateStreamReader.DecodeEmbeddedResource("DotSpatial.Projections.AuthorityCodes.esri.ds"))
            {
                ReadFromStream(str, "ESRI");
            }
            using (var str = DeflateStreamReader.DecodeEmbeddedResource("DotSpatial.Projections.AuthorityCodes.missing.ds"))
            {
                ReadMissingFromStream(str);
            }
        }

        private void ReadFromStream(Stream s, string authority)
        {
            using (var sr = new StreamReader(s))
            {
                string lastLine = "";

                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();

                    if (!string.IsNullOrWhiteSpace(line) && line.StartsWith("<"))
                    {
                        string name = string.Empty;
                        if (!string.IsNullOrWhiteSpace(lastLine) && lastLine.StartsWith("#")) name = lastLine.Trim('#').Trim();

                        var endAuthorityCode = line.IndexOf('>', 1);
                        var authorityCode = line.Substring(1, endAuthorityCode - 1);
                        var projString = line.Substring(endAuthorityCode + 1, line.Length - 2 - (endAuthorityCode + 1)).Trim();

                        Add($"{authority}:{authorityCode}", string.Empty, projString, false);
                    }
                    lastLine = line;
                }
            }
        }

        /// <summary>
        /// Reads the projection
        /// </summary>
        /// <param name="s"></param>
        private void ReadMissingFromStream(Stream s)
        {
            using (var sr = new StreamReader(s))
            {
                string lastLine = "";

                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line) && line.StartsWith("<"))
                    {
                        string name = string.Empty;
                        if (!string.IsNullOrWhiteSpace(lastLine) && lastLine.StartsWith("#")) name = lastLine.Trim('#').Trim();

                        var endAuthorityType = line.IndexOf('>', 1);
                        var authorityType = line.Substring(1, endAuthorityType - 1);

                        var endAuthorityCode = line.IndexOf('>', endAuthorityType + 1);
                        var startAuthorityCode = endAuthorityType + 2;
                        var authorityCode = line.Substring(startAuthorityCode, endAuthorityCode - startAuthorityCode);

                        var projString = line.Substring(endAuthorityCode + 1).Trim();

                        AddMissing(authorityType, authorityCode, name, projString);
                    }
                    lastLine = line;
                }
            }

            //StringBuilder sb = new StringBuilder();

            //foreach (var kvp in errors)
            //{
            //    sb.AppendLine($"{kvp.Key}\t{kvp.Value}");
            //}
            //File.WriteAllText(@"D:\nonexisting.txt", sb.ToString());

        }

        private SortedDictionary<string, string> errors = new SortedDictionary<string, string>();

        private void AddMissing(string authorityType, string authorityCode, string name, string projString)
        {
            var authority = $"{authorityType}:{authorityCode}";

            if (_authorityCodeToProjectionInfo.ContainsKey(authority))
            {
                throw new ArgumentOutOfRangeException(nameof(authority), string.Format(ProjectionMessages.ProjectionWithAuthorityCode0WasAlreadyAdded, authority));
            }
            ProjectionInfo pi;

            try
            {
                pi = ProjectionInfo.FromEsriString(projString);
            }
            catch (Exception e)
            {
                errors[authority] = e.Message;
                return;
            }

            pi.Authority = authorityType;
            pi.AuthorityCode = int.Parse(authorityCode, CultureInfo.InvariantCulture);
            pi.Name = string.IsNullOrEmpty(name) ? authorityCode : name;

            _authorityCodeToProjectionInfo[authority] = pi;

            if (string.IsNullOrEmpty(name))
                return;

            if (_authorityNameToProjectionInfo.ContainsKey(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name), string.Format(ProjectionMessages.ProjectionWithName0WasAlreadyAdded, name));
            }
            _authorityNameToProjectionInfo[name] = pi;
        }


        private void Add(string authorityCode, string name, string proj4String, bool replace)
        {
            var pos = authorityCode.IndexOf(':');
            if (pos == -1)
                throw new ArgumentOutOfRangeException(nameof(authorityCode), "Invalid authorityCode");

            if (!replace && _authorityCodeToProjectionInfo.ContainsKey(authorityCode))
            {
                throw new ArgumentOutOfRangeException(nameof(authorityCode), string.Format(ProjectionMessages.ProjectionWithAuthorityCode0WasAlreadyAdded, authorityCode));
            }
            ProjectionInfo pi;
            try
            {
                pi = ProjectionInfo.FromProj4String(proj4String);
            }
            catch (ProjectionException)
            {
                // todo: geocent not supported yet by DS
                if (proj4String.Contains("+proj=geocent")) return;
                throw;
            }

            pi.Authority = authorityCode.Substring(0, pos);
            pi.AuthorityCode = int.Parse(authorityCode.Substring(pos + 1), CultureInfo.InvariantCulture);
            pi.Name = string.IsNullOrEmpty(name) ? authorityCode : name;

            _authorityCodeToProjectionInfo[authorityCode] = pi;

            if (string.IsNullOrEmpty(name))
                return;

            if (!replace && _authorityNameToProjectionInfo.ContainsKey(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name), string.Format(ProjectionMessages.ProjectionWithName0WasAlreadyAdded, name));
            }
            _authorityNameToProjectionInfo[name] = pi;
        }

        #endregion
    }
}