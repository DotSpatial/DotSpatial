﻿// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Serialization
{
    /// <summary>
    /// A class for parsing the components of the assembly qualified type name.
    /// </summary>
    public class QualifiedTypeName
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QualifiedTypeName"/> class.
        /// Reads in the full string, parsing out the separate elements. These are not always in the specified order,
        /// and many times there are several optional elements. This class helps find the optional elements that are
        /// necessary. This does not support mulitple enclosed types yet, like dictionaries or something. We needed
        /// the single enclosed type for supporting the layer collection however.
        /// </summary>
        /// <param name="qualifiedName">The string qualified name.</param>
        public QualifiedTypeName(string qualifiedName)
        {
            if (qualifiedName.Contains("[["))
            {
                // extract inner type:
                int start = qualifiedName.IndexOf("[[", StringComparison.Ordinal);
                int end = qualifiedName.IndexOf("]]", StringComparison.Ordinal);
                string innerTypeText = qualifiedName.Substring(start + 2, end - (start + 2));
                EnclosedName = new QualifiedTypeName(innerTypeText);
                qualifiedName = qualifiedName.Substring(0, start + 2) + qualifiedName.Substring(end, qualifiedName.Length - end);
            }

            string[] parts = qualifiedName.Split(',');
            TypeName = parts[0];
            Assembly = parts[1].Trim();
            for (int i = 2; i < parts.Length; i++)
            {
                string text = parts[i].Trim();
                if (text.Substring(0, 8) == "Version=")
                {
                    string version = text.Substring(8, text.Length - 8);
                    Version = new Version(version);
                }

                if (text.Substring(0, 8) == "Culture=")
                {
                    Culture = text.Substring(8, text.Length - 8);
                }

                if (text.Substring(0, 15) == "PublicKeyToken=")
                {
                    string publicKeyToken = text.Substring(15, text.Length - 15);
                    PublicKeyToken = publicKeyToken;
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the assembly
        /// </summary>
        public string Assembly { get; set; }

        /// <summary>
        /// Gets or sets the CultureInfo of the Culture
        /// </summary>
        public string Culture { get; set; }

        /// <summary>
        /// Gets or sets the name for the enclosed type.
        /// </summary>
        public QualifiedTypeName EnclosedName { get; set; }

        /// <summary>
        /// Gets or sets the public key token for the strong name of the assembly.
        /// </summary>
        public string PublicKeyToken { get; set; }

        /// <summary>
        /// Gets or sets the TypeName
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Gets or sets a System.Version for getting or setting the version.
        /// </summary>
        public Version Version { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the full qualified type name as "TypeName, Assembly, Version=x, Culture=x, PublicKeyToken=x".
        /// </summary>
        /// <returns>The string expression with the full type name.</returns>
        public new string ToString()
        {
            string fullname = TypeName;
            if (fullname.Contains("[["))
            {
                int insert = TypeName.IndexOf("]]", StringComparison.Ordinal);
                fullname = TypeName.Insert(insert, EnclosedName.ToString());
            }

            // some assemblies don't have PublicKeyToken specified - in this
            // case write 'PublicKeyToken=null'
            string keyToken = PublicKeyToken;
            if (string.IsNullOrEmpty(keyToken))
            {
                keyToken = "null";
            }

            return string.Format("{0}, {1}, Version={2}, Culture={3}, PublicKeyToken={4}", fullname, Assembly, Version, Culture, keyToken);
        }

        #endregion
    }
}