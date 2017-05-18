// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using DotSpatial.NTSExtension;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for ShadedRelief.
    /// </summary>
    public interface IShadedRelief : IDescriptor
    {
        #region Events

        /// <summary>
        /// Occurs when the shading for this object has been altered.
        /// </summary>
        event EventHandler ShadingChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a float specifying how strong the ambient directional light is. This should probably be about 1.
        /// </summary>
        float AmbientIntensity { get; set; }

        /// <summary>
        /// Gets or sets the elevation factor. This is kept separate from extrusion to reduce confusion. This is a conversion factor that will
        /// convert the units of elevation into the same units that the latitude and longitude are stored in.
        /// To convert feet to decimal degrees is around a factor of .00000274
        /// </summary>
        float ElevationFactor { get; set; }

        /// <summary>
        /// Gets or sets a float value expression that modifies the "height" of the apparent shaded relief. A value
        /// of 1 should show the mountains at their true elevations, presuming the ElevationFactor is
        /// correct. A value of 0 would be totally flat, while 2 would be twice the value.
        /// </summary>
        float Extrusion { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the values have been changed on this ShadedRelief more recently than
        /// a HillShade map has been calculated from it.
        /// </summary>
        bool HasChanged { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the ShadedRelief should be used or not.
        /// </summary>
        bool IsUsed { get; set; }

        /// <summary>
        /// Gets or sets the Azimuth light direction in degrees measured clockwise from North.
        /// </summary>
        double LightDirection { get; set; }

        /// <summary>
        /// Gets or sets a float that should probably be around 1, which controls the light intensity.
        /// </summary>
        float LightIntensity { get; set; }

        /// <summary>
        /// Gets or sets the zenith angle in degrees measured with 0 at the horizon and 90 vertically up.
        /// </summary>
        double ZenithAngle { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the normalized light direction in X, Y, Z format.
        /// </summary>
        /// <returns>The normalized light direction.</returns>
        FloatVector3 GetLightDirection();

        #endregion
    }
}