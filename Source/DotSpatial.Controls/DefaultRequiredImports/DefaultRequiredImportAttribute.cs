// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

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