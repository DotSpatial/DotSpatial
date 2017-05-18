// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using DotSpatial.NTSExtension;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    ///  ShadedRelief
    /// </summary>
    public class ShadedRelief : Descriptor, IShadedRelief
    {
        #region Fields

        private float _ambientIntensity;
        private float _elevationFactor;
        private float _extrusion;
        private bool _hasChanged; // set to true when a property changes, and false again when the raster symbolizer calculates the HillShadeMap
        private bool _isUsed;
        private double _lightDirection;
        private float _lightIntensity;
        private double _zenithAngle;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShadedRelief"/> class preset for elevation in feet and coordinates in decimal degrees.
        /// </summary>
        public ShadedRelief()
            : this(ElevationScenario.ElevationFeetProjectionDegrees)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShadedRelief"/> class based on some more common
        /// elevation to geographic coordinate system scenarios.
        /// </summary>
        /// <param name="scenario">Scenario to use.</param>
        public ShadedRelief(ElevationScenario scenario)
        {
            // These scenarios just give a quick approximate calc for the elevation factor
            switch (scenario)
            {
                case ElevationScenario.ElevationCentimetersProjectionDegrees:
                    _elevationFactor = 1F / (160934.4F * 69F);
                    break;
                case ElevationScenario.ElevationCentimetersProjectionFeet:
                    _elevationFactor = 0.0328084F;
                    break;
                case ElevationScenario.ElevationCentimetersProjectionMeters:
                    _elevationFactor = 1F / 100F;
                    break;
                case ElevationScenario.ElevationFeetProjectionDegrees:
                    _elevationFactor = 1F / (5280F * 69F);
                    break;
                case ElevationScenario.ElevationFeetProjectionFeet:
                    _elevationFactor = 1F;
                    break;
                case ElevationScenario.ElevationFeetProjectionMeters:
                    _elevationFactor = 1F / 3.28F;
                    break;
                case ElevationScenario.ElevationMetersProjectionDegrees:
                    _elevationFactor = 1F / (1609F * 69F);
                    break;
                case ElevationScenario.ElevationMetersProjectionFeet:
                    _elevationFactor = 1F * 3.28F;
                    break;
                case ElevationScenario.ElevationMetersProjectionMeters:
                    _elevationFactor = 1F;
                    break;
            }

            // Light direction is SouthEast at about 45 degrees up
            _zenithAngle = 45;
            _lightDirection = 45;

            _lightIntensity = .7F;
            _ambientIntensity = .8F;
            _extrusion = 5;

            // _elevationFactor = 0.0000027F;
            _isUsed = false;
            _hasChanged = false;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the shading for this object has been altered.
        /// </summary>
        public event EventHandler ShadingChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a float specifying how strong the ambient directional light is. This should probably be about 1.
        /// </summary>
        [Category("Shaded Relief")]
        [Description("Gets or sets a float specifying how strong the ambient directional light is. This should probably be about 1.")]
        [Serialize("AmbientIntensity")]
        public float AmbientIntensity
        {
            get
            {
                return _ambientIntensity;
            }

            set
            {
                _ambientIntensity = value;
                _hasChanged = true;
                OnShadingChanged();
            }
        }

        /// <summary>
        /// Gets or sets the elevation factor. This is kept separate from extrusion to reduce confusion. This is a conversion factor that will
        /// convert the units of elevation into the same units that the latitude and longitude are stored in.
        /// To convert feet to decimal degrees is around a factor of .00000274
        /// </summary>
        [Category("Shaded Relief")]
        [Description("This is kept separate from extrusion to reduce confusion. This is a conversion factor that will convert the units of elevation into the same units that the latitude and longitude are stored in. To convert feet to decimal degrees is around a factor of .00000274")]
        [Serialize("ElevationFactor")]
        public float ElevationFactor
        {
            get
            {
                return _elevationFactor;
            }

            set
            {
                _elevationFactor = value;
                _hasChanged = true;
                OnShadingChanged();
            }
        }

        /// <summary>
        /// Gets or sets a float value expression that modifies the "height" of the apparent shaded relief. A value
        /// of 1 should show the mountains at their true elevations, presuming the ElevationFactor is
        /// correct. A value of 0 would be totally flat, while 2 would be twice the value.
        /// </summary>
        [Category("Shaded Relief")]
        [Serialize("Extrusion")]
        [Description("A float value expression that modifies the height of the apparent shaded relief. A value of 1 should show the mountains at their true elevations, presuming the ElevationFactor is correct. A value of 0 would be totally flat, while 2 would be twice the value.")]
        public float Extrusion
        {
            get
            {
                return _extrusion;
            }

            set
            {
                _extrusion = value;
                _hasChanged = true;
                OnShadingChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the values have been changed on this ShadedRelief more recently than
        /// a HillShade map has been calculated from it.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool HasChanged
        {
            get
            {
                return _hasChanged;
            }

            set
            {
                _hasChanged = value;
                OnShadingChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the ShadedRelief should be used or not.
        /// </summary>
        [Category("Shaded Relief")]
        [Serialize("IsUsed")]
        [Description("Gets or sets a boolean value indicating whether the ShadedRelief should be used or not.")]
        public bool IsUsed
        {
            get
            {
                return _isUsed;
            }

            set
            {
                _isUsed = value;
                OnShadingChanged();
            }
        }

        /// <summary>
        /// Gets or sets a double that represents the light direction in degrees clockwise from North.
        /// </summary>
        [Category("Shaded Relief")]
        [Description("The azimuth angle in degrees for the light direction. The angle is measured clockwise from North.")]
        public double LightDirection
        {
            get
            {
                return _lightDirection;
            }

            set
            {
                _lightDirection = value;
                _hasChanged = true;
                OnShadingChanged();
            }
        }

        /// <summary>
        /// Gets or sets a float that should probably be around 1, which controls the light intensity.
        /// </summary>
        [Category("Shaded Relief")]
        [Serialize("LightIntensity")]
        [Description("This specifies a float that should probably be around 1, which controls the light intensity.")]
        public float LightIntensity
        {
            get
            {
                return _lightIntensity;
            }

            set
            {
                _lightIntensity = value;
                _hasChanged = true;
                OnShadingChanged();
            }
        }

        /// <summary>
        /// Gets or sets the zenith angle for the light direction in degrees from 0 (at the horizon) to 90 (straight up).
        /// </summary>
        [Category("Shaded Relief")]
        [Serialize("ZenithAngle")]
        [Description("Gets or sets the zenith angle for the light direction in degrees from 0 (at the horizon) to 90 (straight up).")]
        public double ZenithAngle
        {
            get
            {
                return _zenithAngle;
            }

            set
            {
                _zenithAngle = value;
                _hasChanged = true;
                OnShadingChanged();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a normalized vector in 3 dimensions representing the angle
        /// of the light source.
        /// </summary>
        /// <returns>The light direction.</returns>
        public FloatVector3 GetLightDirection()
        {
            double angle = LightDirection * Math.PI / 180;
            double zAngle = ZenithAngle * Math.PI / 180;
            double x = Math.Sin(angle) * Math.Cos(zAngle);
            double y = Math.Cos(angle) * Math.Cos(zAngle);
            double z = Math.Sin(zAngle);
            return new FloatVector3((float)x, (float)y, (float)z);
        }

        /// <summary>
        /// Fires the ShadingChanged event.
        /// </summary>
        protected virtual void OnShadingChanged()
        {
            ShadingChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}