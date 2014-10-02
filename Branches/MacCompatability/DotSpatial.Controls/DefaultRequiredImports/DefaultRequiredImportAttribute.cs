using System;

namespace DotSpatial.Controls.DefaultRequiredImports
{
    /// <summary>
    /// Special marker for default required imports
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class DefaultRequiredImportAttribute : Attribute
    {

    }
}