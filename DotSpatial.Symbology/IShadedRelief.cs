// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/27/2008 3:42:51 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using DotSpatial.Topology;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// IShadedRelief
    /// </summary>
    public interface IShadedRelief : IDescriptor
    {
        #region Events

        /// <summary>
        /// Occurs when the shading for this object has been altered.
        /// </summary>
        event EventHandler ShadingChanged;

        #endregion

        #region Methods

        /// <summary>
        /// Returns the normalized light direction in X, Y, Z format
        /// </summary>
        /// <returns></returns>
        FloatVector3 GetLightDirection();

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a float specifying how strong the ambient directional light is.  This should probably be about 1.
        /// </summary>
        float AmbientIntensity
        {
            get;
            set;
        }

        /// <summary>
        /// This is kept separate from extrusion to reduce confusion.  This is a conversion factor that will
        /// convert the units of elevation into the same units that the latitude and longitude are stored in.
        /// To convert feet to decimal degrees is around a factor of .00000274
        /// </summary>
        float ElevationFactor
        {
            get;
            set;
        }

        /// <summary>
        /// A float value expression that modifies the "height" of the apparent shaded relief.  A value
        /// of 1 should show the mountains at their true elevations, presuming the ElevationFactor is
        /// correct.  A value of 0 would be totally flat, while 2 would be twice the value.
        /// </summary>
        float Extrusion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a boolean value indicating whether the ShadedRelief should be used or not.
        /// </summary>
        bool IsUsed
        {
            get;
            set;
        }

        /// <summary>
        /// This specifies a float that should probably be around 1, which controls the light intensity.
        /// </summary>
        float LightIntensity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the zenith angle in degrees measured with 0 at the horizon and 90 vertically up
        /// </summary>
        double ZenithAngle
        {
            get;
            set;
        }

        /// <summary>
        /// The Azimuth light direction in degrees measured clockwise from North
        /// </summary>
        double LightDirection
        {
            get;
            set;
        }

        /// <summary>
        /// Gets whether or not the values have been changed on this ShadedRelief more recently than
        /// a HillShade map has been calculated from it.
        /// </summary>
        bool HasChanged
        {
            get;
            set;
        }

        #endregion
    }
}