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
// The Initial Developer of this Original Code is Darrel Brown. Created 9/10/2009
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |-----------------|---------|---------------------------------------------------------------------
// |      Name       |  Date   |                        Comments
// |-----------------|---------|----------------------------------------------------------------------
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DotSpatial.Serialization
{
    /// <summary>
    /// A set of utilities to automate some common reflection tasks.
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Searches the entire folder tree beginning at the entry assembly path for types that derive from the specified type.
        /// </summary>
        /// <typeparam name="T">The base class/interface type</typeparam>
        /// <returns>A list of all types found in the search.</returns>
        public static IEnumerable<Type> FindDerivedClasses<T>()
        {
            return FindDerivedClasses(typeof(T));
        }

        /// <summary>
        /// Searches the entire folder tree beginning at the entry assembly path for types that derive from the specified type.
        /// </summary>
        /// <param name="baseType">The base class/interface type.</param>
        /// <returns>A list of all types found in the search.</returns>
        public static IEnumerable<Type> FindDerivedClasses(Type baseType)
        {
            List<Type> result = new List<Type>();

            var dlls = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll", SearchOption.AllDirectories);

            foreach (string assemblyPath in dlls)
            {
                Assembly assembly;

                try
                {
                    assembly = Assembly.LoadFrom(assemblyPath);
                }
                catch (BadImageFormatException)
                {
                    // This is not a problem, we just need to ignore these native assemblies
                    continue;
                }
                catch (FileLoadException)
                {
                    // ignore
                    continue;
                }

                try
                {
                    if (baseType.IsGenericTypeDefinition)
                    {
                        result.AddRange(assembly.GetTypes().
                                            Where(t =>
                                                  t.BaseType != null &&
                                                  t.BaseType.IsGenericType &&
                                                  t.BaseType.GetGenericTypeDefinition().Equals(baseType)));
                    }
                    else
                        result.AddRange(assembly.GetTypes().Where(t => !t.Equals(baseType) && baseType.IsAssignableFrom(t)));
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return result;
        }
    }
}