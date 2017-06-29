// ********************************************************************************************************
// Product Name: DotSpatial.Positioning.dll
// Description:  A library for managing GPS connections.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from http://geoframework.codeplex.com/ version 2.0
//
// The Initial Developer of this original code is Jon Pearson. Submitted Oct. 21, 2010 by Ben Tombs (tidyup)
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// -------------------------------------------------------------------------------------------------------
// |    Developer             |    Date    |                             Comments
// |--------------------------|------------|--------------------------------------------------------------
// | Tidyup  (Ben Tombs)      | 10/21/2010 | Original copy submitted from modified GeoFrameworks 2.0
// | Shade1974 (Ted Dunsford) | 10/21/2010 | Added file headers reviewed formatting with resharper.
// ********************************************************************************************************
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
        public static readonly StaticFinalizer Current = new StaticFinalizer();

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the <see cref="StaticFinalizer"/> is reclaimed by garbage collection.
        /// </summary>
        ~StaticFinalizer()
        {
            try
            {
                if (Disposed != null)
                    Disposed(this, EventArgs.Empty);
            }
            catch
            {
                Debug.WriteLine("Finalizer can't throw exception.");
            }
        }
    }
}