// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Drawing;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// EditorSettings
    /// </summary>
    [Serializable]
    public class EditorSettings : Descriptor
    {
        #region Fields

        private Color _endColor;
        private string _excludeExpression;
        private bool _hsl;
        private int _hueShift;
        private IntervalMethod _intervalMethod;
        private int _intervalRoundingDigits;
        private IntervalSnapMethod _intervalSnapMethod;
        private int _maxSampleCount;
        private int _numBreaks;
        private bool _rampColors;
        private Color _startColor;
        private bool _useColor;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorSettings"/> class.
        /// </summary>
        public EditorSettings()
        {
            _hsl = true;
            _useColor = true;
            _startColor = SymbologyGlobal.ColorFromHsl(5, .7, .7);
            _endColor = SymbologyGlobal.ColorFromHsl(345, .8, .8);
            _maxSampleCount = 10000;
            _intervalMethod = IntervalMethod.EqualInterval;
            _rampColors = true;
            _intervalSnapMethod = IntervalSnapMethod.DataValue;
            _intervalRoundingDigits = 0;
            _numBreaks = 5;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the characteristics of the "right" color.
        /// </summary>
        [Serialize("EndColor")]
        public Color EndColor
        {
            get
            {
                return _endColor;
            }

            set
            {
                _endColor = value;
            }
        }

        /// <summary>
        /// Gets or sets a string that allows the user to use any of the data fields to eliminate
        /// values from being considered as part of the histogram for statistical interval calculations.
        /// </summary>
        [Serialize("ExcludeExpression")]
        public string ExcludeExpression
        {
            get
            {
                return _excludeExpression;
            }

            set
            {
                _excludeExpression = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to display the hue, saturation and
        /// lightness as bounds instead of start-color, end-color.
        /// </summary>
        [Serialize("HueSatLight")]
        public bool HueSatLight
        {
            get
            {
                return _hsl;
            }

            set
            {
                _hsl = value;
            }
        }

        /// <summary>
        /// Gets or sets the hue shift.
        /// </summary>
        [Serialize("HueShift")]
        public int HueShift
        {
            get
            {
                return _hueShift;
            }

            set
            {
                _hueShift = value;
            }
        }

        /// <summary>
        /// Gets or sets the interval method.
        /// </summary>
        [Serialize("IntervalMethod")]
        public IntervalMethod IntervalMethod
        {
            get
            {
                return _intervalMethod;
            }

            set
            {
                _intervalMethod = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of digits to preserve when IntervalSnapMethod is set to Rounding.
        /// </summary>
        [Serialize("IntervalRoundingDigits")]
        public int IntervalRoundingDigits
        {
            get
            {
                return _intervalRoundingDigits;
            }

            set
            {
                _intervalRoundingDigits = value;
            }
        }

        /// <summary>
        /// Gets or sets how intervals like equal breaks choose the
        /// actual values, and whether they are rounded or snapped.
        /// </summary>
        [Serialize("IntervalSnapMethod")]
        public IntervalSnapMethod IntervalSnapMethod
        {
            get
            {
                return _intervalSnapMethod;
            }

            set
            {
                _intervalSnapMethod = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum sample count.
        /// </summary>
        [Serialize("MaxSampleCount")]
        public int MaxSampleCount
        {
            get
            {
                return _maxSampleCount;
            }

            set
            {
                _maxSampleCount = value;
            }
        }

        /// <summary>
        /// Gets or sets the integer count if equal breaks are used.
        /// </summary>
        [Serialize("NumBreaks")]
        public int NumBreaks
        {
            get
            {
                return _numBreaks;
            }

            set
            {
                _numBreaks = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this editor should ramp the colors,
        /// or use randomly generated colors. The default is random.
        /// </summary>
        [Serialize("RampColors")]
        public bool RampColors
        {
            get
            {
                return _rampColors;
            }

            set
            {
                _rampColors = value;
            }
        }

        /// <summary>
        /// Gets or sets the characteristics of the "left" color.
        /// </summary>
        [Serialize("StartColor")]
        public Color StartColor
        {
            get
            {
                return _startColor;
            }

            set
            {
                _startColor = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use the color specifications.
        /// </summary>
        [Serialize("UseColorRange")]
        public bool UseColorRange
        {
            get
            {
                return _useColor;
            }

            set
            {
                _useColor = value;
            }
        }

        #endregion
    }
}