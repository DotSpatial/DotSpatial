using System;
using System.Diagnostics;

namespace DotSpatial.Extensions
{
    /// <summary>
    /// Extension methods for IExtension
    /// </summary>
    public static class IExtensionExtensionMethods
    {
        /// <summary>
        /// Tries to activate the extension.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns></returns>
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
                Trace.WriteLine(String.Format("Error: {0} {1} {2}", extension.AssemblyQualifiedName, ex.Message, ex.StackTrace));
                return false;
            }
        }
    }
}