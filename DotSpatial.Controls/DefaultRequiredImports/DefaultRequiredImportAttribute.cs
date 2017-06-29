using System;

namespace DotSpatial.Controls.DefaultRequiredImports
{
    /// <summary>
    /// Special marker for default required imports
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    internal class DefaultRequiredImportAttribute : Attribute
    {

    }
}