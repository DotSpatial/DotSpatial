// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// FeatureEditorSettings.
    /// </summary>
    [Serializable]
    public class FeatureEditorSettings : EditorSettings
    {
        #region Fields

        private ClassificationType _classificationType;
        private double _endSize;
        private string _fieldName;
        private int _gradientAngle;
        private string _normField;
        private double _startSize;
        private IFeatureSymbolizer _templateSymbolizer;
        private bool _useGradient;
        private bool _useSizeRange;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureEditorSettings"/> class.
        /// </summary>
        public FeatureEditorSettings()
        {
            _useGradient = true;
            _gradientAngle = -45;
            _startSize = 5;
            _endSize = 20;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique values.
        /// </summary>
        [Serialize("ClassificationType")]
        public ClassificationType ClassificationType
        {
            get
            {
                return _classificationType;
            }

            set
            {
                _classificationType = value;
            }
        }

        /// <summary>
        /// Gets or sets the double size for the last item in the range.
        /// </summary>
        [Serialize("EndSize")]
        public double EndSize
        {
            get
            {
                return _endSize;
            }

            set
            {
                _endSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the field name that categories are based on.
        /// </summary>
        [Serialize("FieldName")]
        public string FieldName
        {
            get
            {
                return _fieldName;
            }

            set
            {
                _fieldName = value;
            }
        }

        /// <summary>
        /// Gets or sets the gradient angle if use gradient is true
        /// and the shape is a polygon shape.
        /// </summary>
        [Serialize("GradientAngle")]
        public int GradientAngle
        {
            get
            {
                return _gradientAngle;
            }

            set
            {
                _gradientAngle = value;
            }
        }

        /// <summary>
        /// Gets or sets the normalization field.
        /// </summary>
        [Serialize("NormField")]
        public string NormField
        {
            get
            {
                return _normField;
            }

            set
            {
                _normField = value;
            }
        }

        /// <summary>
        /// Gets or sets the double start size for point or line size ranges.
        /// </summary>
        [Serialize("StartSize")]
        public double StartSize
        {
            get
            {
                return _startSize;
            }

            set
            {
                _startSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the feature symbolizer that acts as a template for
        /// any characteristics not covered by the size and color ranges.
        /// </summary>
        [Serialize("TemplateSymbolizer")]
        public IFeatureSymbolizer TemplateSymbolizer
        {
            get
            {
                return _templateSymbolizer;
            }

            set
            {
                _templateSymbolizer = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not to use a gradient
        /// when randomly calculating polygon forms.
        /// </summary>
        [Serialize("UseGradient")]
        public bool UseGradient
        {
            get
            {
                return _useGradient;
            }

            set
            {
                _useGradient = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the size range should be used instead of
        /// the size specified by the template.
        /// </summary>
        [Serialize("UseSizeRange")]
        public bool UseSizeRange
        {
            get
            {
                return _useSizeRange;
            }

            set
            {
                _useSizeRange = value;
            }
        }

        #endregion
    }
}