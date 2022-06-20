// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System;
using System.Diagnostics;

namespace DotSpatial.Positioning
{
    /// <summary>
    /// Serves as a notification to dispose of static objects.
    /// </summary>
    /// <example lang="cs">
    /// private static StaticFinalizer MyStaticFinalizer = new StaticFinalizer();
    /// private static Brush UnmanagedResource = new SolidBrush(Color.Blue);
    /// void Constructor()
    /// {
    /// MyStaticFinalizer.Disposed += new EventHandler(StaticFinalize);
    /// }
    /// void StaticFinalize(object sender, EventArgs e)
    /// {
    /// UnmanagedResource.Dispose();
    /// }
    ///   </example>
    /// <remarks>It is not uncommon for static variables to contain unmanaged resources.  Yet,
    /// the .NET Garbage Collector does not allow for finalizers on static objects.  The StaticFinalizer
    /// class serves to work around this problem.  To use this class, declare an instance as a
    /// private, static variable.  Then, hook into its Disposed event.  The event will be raised
    /// during the StaticFinalizer's own finalizer, allowing you to safely dispose of static resources.</remarks>
    public class StaticFinalizer
    {
        /// <summary>
        /// Occurs when [disposed].
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        ///
        /// </summary>
        public static readonly StaticFinalizer Current = new();

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the <see cref="StaticFinalizer"/> is reclaimed by garbage collection.
        /// </summary>
        ~StaticFinalizer()
        {
            try
            {
                Disposed?.Invoke(this, EventArgs.Empty);
            }
            catch
            {
                Debug.WriteLine("Finalizer can't throw exception.");
            }
        }
    }
}