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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/17/2008 11:20:29 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using DotSpatial.Serialization;
using DotSpatial.Topology;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// ShadedRelief
    /// </summary>
    public class ShadedRelief : Descriptor, IShadedRelief
    {
        #region Events

        /// <summary>
        /// Occurs when the shading for this object has been altered.
        /// </summary>
        public event EventHandler ShadingChanged;

        #endregion

        #region Private Variables

        float _ambientIntensity;
        float _elevationFactor;
        float _extrusion;
        bool _hasChanged; // set to true when a property changes, and false again when the raster symbolizer calculates the HillShadeMap
        bool _isUsed;
        private double _lightDirection;
        float _lightIntensity;
        private double _zenithAngle;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the ShadedRelief preset for elevation in feet and coordinates in decimal degrees
        /// </summary>
        public ShadedRelief()
            : this(ElevationScenario.ElevationFeet_ProjectionDegrees)
        {
        }

        /// <summary>
        /// Creates a new instance of ShadedRelief based on some more common
        /// elevation to goegraphic coordinate sysetem scenarios
        /// </summary>
        public ShadedRelief(ElevationScenario scenario)
        {
            // These scenarios just give a quick approximate calc for the elevation factor
            switch (scenario)
            {
                case ElevationScenario.ElevationCentiMeters_ProjectionDegrees:
                    _elevationFactor = 1F / (160934.4F * 69F);
                    break;
                case ElevationScenario.ElevationCentiMeters_ProjectionFeet:
                    _elevationFactor = 0.0328084F;
                    break;
                case ElevationScenario.ElevationCentiMeters_ProjectionMeters:
                    _elevationFactor = 1F / 100F;
                    break;
                case ElevationScenario.ElevationFeet_ProjectionDegrees:
                    _elevationFactor = 1F / (5280F * 69F);
                    break;
                case ElevationScenario.ElevationFeet_ProjectionFeet:
                    _elevationFactor = 1F;
                    break;
                case ElevationScenario.ElevationFeet_ProjectionMeters:
                    _elevationFactor = 1F / 3.28F;
                    break;
                case ElevationScenario.ElevationMeters_ProjectionDegrees:
                    _elevationFactor = 1F / (1609F * 69F);
                    break;
                case ElevationScenario.ElevationMeters_ProjectionFeet:
                    _elevationFactor = 1F * 3.28F;
                    break;
                case ElevationScenario.ElevationMeters_ProjectionMeters:
                    _elevationFactor = 1F;
                    break;
            }

            // Light direction is SouthEast at about 45 degrees up
            _zenithAngle = 45;
            _lightDirection = 45;

            _lightIntensity = .7F;
            _ambientIntensity = .8F;
            _extrusion = 5;
            //_elevationFactor = 0.0000027F;
            _isUsed = false;
            _hasChanged = false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a normalized vector in 3 dimensions representing the angle
        /// of the light source.
        /// </summary>
        /// <returns></returns>
        public FloatVector3 GetLightDirection()
        {
            double angle = LightDirection * Math.PI / 180;
            double zAngle = ZenithAngle * Math.PI / 180;
            double x = Math.Sin(angle) * Math.Cos(zAngle);
            double y = Math.Cos(angle) * Math.Cos(zAngle);
            double z = Math.Sin(zAngle);
            return new FloatVector3((float)x, (float)y, (float)z);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a float specifying how strong the ambient directional light is.  This should probably be about 1.
        /// </summary>
        [Category("Shaded Relief"), Description("Gets or sets a float specifying how strong the ambient directional light is.  This should probably be about 1."),
         Serialize("AmbientIntensity")]
        public float AmbientIntensity
        {
            get { return _ambientIntensity; }
            set
            {
                _ambientIntensity = value;
                _hasChanged = true;
                OnShadingChanged();
            }
        }

        /// <summary>
        /// This is kept separate from extrusion to reduce confusion.  This is a conversion factor that will
        /// convert the units of elevation into the same units that the latitude and longitude are stored in.
        /// To convert feet to decimal degrees is around a factor of .00000274
        /// </summary>
        [Category("Shaded Relief"), Description("This is kept separate from extrusion to reduce confusion.  This is a conversion factor that will convert the units of elevation into the same units that the latitude and longitude are stored in.  To convert feet to decimal degrees is around a factor of .00000274"),
         Serialize("ElevationFactor")]
        public float ElevationFactor
        {
            get { return _elevationFactor; }
            set
            {
                _elevationFactor = value;
                _hasChanged = true;
                OnShadingChanged();
            }
        }

        /// <summary>
        /// A float value expression that modifies the "height" of the apparent shaded relief.  A value
        /// of 1 should show the mountains at their true elevations, presuming the ElevationFactor is
        /// correct.  A value of 0 would be totally flat, while 2 would be twice the value.
        /// </summary>
        [Category("Shaded Relief"), Serialize("Extrusion"),
         Description("A float value expression that modifies the height of the apparent shaded relief.  A value of 1 should show the mountains at their true elevations, presuming the ElevationFactor is correct.  A value of 0 would be totally flat, while 2 would be twice the value.")]
        public float Extrusion
        {
            get { return _extrusion; }
            set
            {
                _extrusion = value;
                _hasChanged = true;
                OnShadingChanged();
            }
        }

        /// <summary>
        /// Gets or sets a boolean value indicating whether the ShadedRelief should be used or not.
        /// </summary>
        [Category("Shaded Relief"), Serialize("IsUsed"),
         Description("Gets or sets a boolean value indicating whether the ShadedRelief should be used or not.")]
        public bool IsUsed
        {
            get { return _isUsed; }
            set
            {
                _isUsed = value;
                OnShadingChanged();
            }
        }

        /// <summary>
        /// Gets or sets the zenith angle for the light direction in degrees from 0 (at the horizon) to 90 (straight up).
        /// </summary>
        [Category("Shaded Relief"), Serialize("ZenithAngle"),
            //Editor(typeof(ZenithEditor), typeof(UITypeEditor)),
         Description("Gets or sets the zenith angle for the light direction in degrees from 0 (at the horizon) to 90 (straight up).")]
        public double ZenithAngle
        {
            get { return _zenithAngle; }
            set
            {
                _zenithAngle = value;
                _hasChanged = true;
                OnShadingChanged();
            }
        }

        /// <summary>
        /// Gets or sets a double that represents the light direction in degrees clockwise from North
        /// </summary>
        [Category("Shaded Relief"),
            //Editor(typeof(AzimuthAngleEditor), typeof(UITypeEditor)), Serialize("LightDirection"),
         Description("The azimuth angle in degrees for the light direction.  The angle is measured clockwise from North.")]
        public double LightDirection
        {
            get { return _lightDirection; }
            set
            {
                _lightDirection = value;
                _hasChanged = true;
                OnShadingChanged();
            }
        }

        /// <summary>
        /// This specifies a float that should probably be around 1, which controls the light intensity.
        /// </summary>
        [Category("Shaded Relief"), Serialize("LightIntensity"),
         Description("This specifies a float that should probably be around 1, which controls the light intensity.")]
        public float LightIntensity
        {
            get { return _lightIntensity; }
            set
            {
                _lightIntensity = value;
                _hasChanged = true;
                OnShadingChanged();
            }
        }

        /// <summary>
        /// Gets whether or not the values have been changed on this ShadedRelief more recently than
        /// a HillShade map has been calculated from it.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool HasChanged
        {
            get { return _hasChanged; }
            set
            {
                _hasChanged = value;
                OnShadingChanged();
            }
        }

        #endregion

        /// <summary>
        /// Fires the ShadingChanged event
        /// </summary>
        protected virtual void OnShadingChanged()
        {
            if (ShadingChanged != null) ShadingChanged(this, EventArgs.Empty);
        }
    }
}