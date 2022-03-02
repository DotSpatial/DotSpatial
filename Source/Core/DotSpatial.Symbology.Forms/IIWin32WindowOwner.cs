// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// Contains IWin32Window Owner.
    /// </summary>
    public interface IIWin32WindowOwner
    {
        /// <summary>
        /// Gets or sets owner for any dialogs that need to be launched.
        /// </summary>
        IWin32Window Owner { get; set; }
    }
}