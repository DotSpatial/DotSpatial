using System;

namespace DotSpatial.Positioning
{
    /// <summary>
    /// Serves as a notification to dispose of static objects.
    /// </summary>
    /// <remarks>It is not uncommon for static variables to contain unmanaged resources.  Yet,
    /// the .NET Garbage Collector does not allow for finalizers on static objects.  The StaticFinalizer 
    /// class serves to work around this problem.  To use this class, declare an instance as a
    /// private, static variable.  Then, hook into its Disposed event.  The event will be raised
    /// during the StaticFinalizer's own finalizer, allowing you to safely dispose of static resources.</remarks>
    /// <example lang="cs">
    /// private static StaticFinalizer MyStaticFinalizer = new StaticFinalizer();
    /// private static Brush UnmanagedResource = new SolidBrush(Color.Blue);
    /// 
    /// void Constructor()
    /// {
    ///     MyStaticFinalizer.Disposed += new EventHandler(StaticFinalize);
    /// }
    /// 
    /// void StaticFinalize(object sender, EventArgs e)
    /// {
    ///     UnmanagedResource.Dispose();
    /// }
    /// </example>
    public class StaticFinalizer
    {
        public event EventHandler Disposed;

        public static readonly StaticFinalizer Current = new StaticFinalizer();

        public StaticFinalizer()
        { }

        ~StaticFinalizer()
        {
            try
            {
                if (Disposed != null)
                    Disposed(this, EventArgs.Empty);
            }
            catch 
            { 
                // Finalizers can't throw exceptions
            }
        }
    }
}
