using System.ComponentModel.Composition;

namespace DotSpatial.Extensions
{
    /// <summary>
    /// Extensions of this type are activiated before other extensions and may be used to help satisfy required imports.
    /// </summary>
    [InheritedExport]
    public interface ISatisfyImportsExtension
    {
        /// <summary>
        /// Specifies the activation priority order
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Activates this extension
        /// </summary>
        void Activate();
    }
}