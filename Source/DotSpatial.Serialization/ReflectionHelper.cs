// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

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
        #region Methods

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
                        result.AddRange(assembly.GetTypes().Where(t => t.BaseType != null && t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition().Equals(baseType)));
                    }
                    else
                    {
                        result.AddRange(assembly.GetTypes().Where(t => !t.Equals(baseType) && baseType.IsAssignableFrom(t)));
                    }
                }
                catch (Exception)
                {
                }
            }

            return result;
        }

        #endregion
    }
}