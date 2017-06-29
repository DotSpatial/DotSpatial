// ********************************************************************************************************
// Product Name: DotSpatial.Serialization.dll
// Description:  A module that supports common functions like serialization.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/26/2010 6:56:48 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |-----------------|---------|---------------------------------------------------------------------
// |      Name       |  Date   |                        Comments
// |-----------------|---------|---------------------------------------------------------------------
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DotSpatial.Serialization
{
    /// <summary>
    /// In many cases, the explicit type name references a version or public key token that is expired,
    /// even though the reference is still perfectly valid in the new instance.  This type allows testing
    /// for that eventuality, as well as working directly with the components of a fully qualified name.
    /// </summary>
    public class TypeNameManager
    {
        private readonly Dictionary<string, Assembly> _loadedAssemblies;

        /// <summary>
        /// Initializes a new instance of the TypeManager class.
        /// </summary>
        public TypeNameManager()
        {
            _loadedAssemblies = new Dictionary<string, Assembly>();
        }

        /// <summary>
        /// Since the version, or even possibly the strong name may be dynamic, an older
        /// reference may need to be updated with local assembly information.
        /// </summary>
        /// <param name="invalidTypeName">The invalidated type name, usually because the version is out of date.</param>
        /// <returns>A string represnting the same type, but with a modern assmebly.</returns>
        public string UpdateTypename(string invalidTypeName)
        {
            QualifiedTypeName myType = new QualifiedTypeName(invalidTypeName);
            QualifiedTypeName type = myType;
            while (type != null)
            {
                UpdateVersion(type);
                type = type.EnclosedName;
            }
            return myType.ToString();
        }

        private void UpdateVersion(QualifiedTypeName myType)
        {
            if (!_loadedAssemblies.Keys.Contains(myType.Assembly))
            {
                UpdateAssembly(myType.Assembly);
            }
            if (!_loadedAssemblies.Keys.Contains(myType.Assembly))
            {
                // Don't throw an exception here, because we are just trying to update versions.
                // Since GAC and System libraries aren't discovered this way, we must
                // simply choose not to update those libraries and hope the reference
                // is not invalid because of a GAC or System library update.
                return;
            }

            AssemblyName myAssembly = _loadedAssemblies[myType.Assembly].GetName();
            myType.Version = myAssembly.Version;
            string publicKeyToken = BitConverter.ToString(myAssembly.GetPublicKeyToken());
            publicKeyToken = publicKeyToken.Replace("-", string.Empty).ToLower();
            myType.PublicKeyToken = publicKeyToken;
        }

        /// <summary>
        /// This method searches the executable path, as well as sub-folders looking for an instance of
        /// the specified assembly.  Since this class is only needed if the fully qualified assembly name
        /// is invalid, we have to assume that we are looking for something else.
        /// </summary>
        /// <param name="assembly">The string assembly name.</param>
        private void UpdateAssembly(string assembly)
        {
            string file = assembly + ".dll";
            if (!File.Exists(file))
            {
                string path =
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (path != null)
                {
                    // Try subfolders
                    string[] files = Directory.GetFiles(path, file, SearchOption.AllDirectories);
                    if (files.Length > 0)
                    {
                        file = files[0];
                    }
                    else
                    {
                        // try parent folders
                        string dir = Path.GetDirectoryName(path);
                        if (dir != null)
                        {
                            DirectoryInfo di = Directory.GetParent(dir);
                            while (di != null)
                            {
                                string[] parentfiles = Directory.GetFiles(path, file, SearchOption.TopDirectoryOnly);
                                if (parentfiles.Length == 0)
                                {
                                    di = di.Parent;
                                    continue;
                                }
                                file = parentfiles[0];
                                break;
                            }
                        }
                    }
                }
            }
            if (File.Exists(file))
            {
                Assembly tryAssembly = Assembly.LoadFrom(file);
                if (tryAssembly != null)
                {
                    _loadedAssemblies.Add(assembly, tryAssembly);
                }
            }
        }
    }
}