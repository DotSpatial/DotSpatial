// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Diagnostics;

namespace DotSpatial.Extensions
{
    /// <summary>
    /// Extension methods for IExtension.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class IExtensionExtensionMethods
    {
        /// <summary>
        /// Tries to activate the extension.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns>True, if the extension was activated without throwing an exception.</returns>
        public static bool TryActivate(this IExtension extension)
        {
            Trace.WriteLine("Activating: " + extension.AssemblyQualifiedName);
            try
            {
                extension.Activate();
                return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error: {extension.AssemblyQualifiedName} {ex.Message} {ex.StackTrace}");
                return false;
            }
        }
    }
}