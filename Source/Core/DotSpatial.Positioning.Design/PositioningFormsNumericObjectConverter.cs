// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Positioning.Design
{
    /// <summary>
    /// The PositioningFormsNumericObjectConverter handles the object conversion but is tailored to work with the DotSpatial.Positioning.Forms class.
    /// </summary>
    // The Original Code is from http://gps3.codeplex.com/ version 3.0
    public abstract class PositioningFormsNumericObjectConverter : PositioningNumericObjectConverter
    {
        /// <inheritdoc />
        protected override Version HandledAssemblyVersion => new Version("1.0.0.*");

        /// <inheritdoc />
        protected override string HandledAssemblyName => "DotSpatial.Positioning.Forms, Culture=neutral, Version=" + HandledAssemblyVersion.ToString(4) + ", PublicKeyToken=3a45fedac1c4cdab";
    }
}