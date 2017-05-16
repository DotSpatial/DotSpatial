// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;
using DotSpatial.Data;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Represents a cartographic stroke with several useful settings.
    /// </summary>
    [Serializable]
    [XmlRoot("CartographicStroke")]
    public class CartographicStroke : SimpleStroke, ICartographicStroke
    {
        #region Fields

        private float[] _compondArray;
        private bool[] _compoundButtons;
        private bool[] _dashButtons;
        private DashCap _dashCap;
        private float[] _dashPattern;
        private IList<ILineDecoration> _decorations;
        private LineCap _endCap;
        private LineJoinType _joinType;
        private float _offset;
        private LineCap _startCap;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CartographicStroke"/> class.
        /// </summary>
        public CartographicStroke()
        {
            Color = SymbologyGlobal.RandomDarkColor(1);
            Configure();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CartographicStroke"/> class of the specified color.
        /// </summary>
        /// <param name="color">The color that should be used.</param>
        public CartographicStroke(Color color)
        {
            Color = color;
            Configure();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets an array of floating point values ranging from 0 to 1 that
        /// indicate the start and end point for where the line should draw.
        /// </summary>
        [XmlIgnore]
        public float[] CompoundArray
        {
            get
            {
                return _compondArray;
            }

            set
            {
                _compondArray = value;
            }
        }

        /// <summary>
        /// Gets or sets the compound buttons. This is a cached version of the vertical pattern that should appear in the custom dash control.
        /// This is only used if DashStyle is set to custom, and only controls the pattern control,
        /// and does not directly affect the drawing pen.
        /// </summary>
        public bool[] CompoundButtons
        {
            get
            {
                return _compoundButtons;
            }

            set
            {
                _compoundButtons = value;
            }
        }

        /// <summary>
        /// Gets or sets the dash buttons. This is a cached version of the horizontal pattern that should appear in the custom dash control.
        /// This is only used if DashStyle is set to custom, and only controls the pattern control,
        /// and does not directly affect the drawing pen.
        /// </summary>
        public bool[] DashButtons
        {
            get
            {
                return _dashButtons;
            }

            set
            {
                _dashButtons = value;
            }
        }

        /// <summary>
        /// gets or sets the DashCap for both the start and end caps of the dashes
        /// </summary>
        [Serialize("DashCap")]
        public DashCap DashCap
        {
            get
            {
                return _dashCap;
            }

            set
            {
                _dashCap = value;
            }
        }

        /// <summary>
        /// Gets or sets the DashPattern as an array of floating point values from 0 to 1
        /// </summary>
        [XmlIgnore]
        public float[] DashPattern
        {
            get
            {
                return _dashPattern;
            }

            set
            {
                _dashPattern = value;
            }
        }

        /// <summary>
        /// Gets or sets the line decoration that describes symbols that should
        /// be drawn along the line as decoration.
        /// </summary>
        [Serialize("Decorations")]
        public IList<ILineDecoration> Decorations
        {
            get
            {
                return _decorations;
            }

            set
            {
                _decorations = value;
            }
        }

        /// <summary>
        /// Gets or sets the line cap for both the start and end of the line
        /// </summary>
        [Serialize("EndCap")]
        public LineCap EndCap
        {
            get
            {
                return _endCap;
            }

            set
            {
                _endCap = value;
            }
        }

        /// <summary>
        /// Gets or sets the OGC line characteristic that controls how connected segments
        /// are drawn where they come together.
        /// </summary>
        [Serialize("JoinType")]
        public LineJoinType JoinType
        {
            get
            {
                return _joinType;
            }

            set
            {
                _joinType = value;
            }
        }

        /// <summary>
        /// Gets or sets the floating poing offset (in pixels) for the line to be drawn to the left of
        /// the original line. (Internally, this will modify the width and compound array for the
        /// actual pen being used, as Pens do not support an offset property).
        /// </summary>
        [Serialize("Offset")]
        public float Offset
        {
            get
            {
                return _offset;
            }

            set
            {
                _offset = value;
            }
        }

        /// <summary>
        /// Gets or sets the line cap for both the start and end of the line
        /// </summary>
        [Serialize("LineCap")]
        public LineCap StartCap
        {
            get
            {
                return _startCap;
            }

            set
            {
                _startCap = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Draws the line with max. 2 decorations. Otherwise the legend line might show only decorations.
        /// </summary>
        /// <param name="g">The graphics object used for drawing.</param>
        /// <param name="path">The path that should be drawn.</param>
        /// <param name="scaleWidth">The double scale width for controling markers.</param>
        public void DrawLegendPath(Graphics g, GraphicsPath path, double scaleWidth)
        {
            base.DrawPath(g, path, scaleWidth); // draw the actual line
            if (Decorations != null)
            {
                int temp = -1;
                foreach (ILineDecoration decoration in Decorations)
                {
                    if (decoration.NumSymbols > 2)
                    {
                        temp = decoration.NumSymbols;
                        decoration.NumSymbols = 2;
                    }

                    decoration.Draw(g, path, scaleWidth);
                    if (temp > -1)
                    {
                        decoration.NumSymbols = temp;
                        temp = -1;
                    }
                }
            }
        }

        /// <summary>
        /// Draws the actual path, overriding the base behavior to include markers.
        /// </summary>
        /// <param name="g">The graphics object used for drawing.</param>
        /// <param name="path">The path that should be drawn.</param>
        /// <param name="scaleWidth">The double scale width for controling markers.</param>
        public override void DrawPath(Graphics g, GraphicsPath path, double scaleWidth)
        {
            base.DrawPath(g, path, scaleWidth); // draw the actual line
            if (Decorations != null)
            {
                foreach (ILineDecoration decoration in Decorations)
                {
                    decoration.Draw(g, path, scaleWidth);
                }
            }
        }

        /// <summary>
        /// Gets the width and height that is needed to draw this stroke with max. 2 decorations.
        /// </summary>
        /// <returns>The legend symbol size.</returns>
        public Size GetLegendSymbolSize()
        {
            Size size = new Size(16, 16);
            foreach (ILineDecoration decoration in Decorations)
            {
                Size s = decoration.GetLegendSymbolSize();
                if (s.Height > size.Height) size.Height = s.Height;
                if (s.Width > size.Width) size.Width = s.Width;
            }

            return size;
        }

        /// <summary>
        /// Creates a pen for drawing the non-decorative portion of the line.
        /// </summary>
        /// <param name="scaleWidth">The base width in pixels that is equivalent to a width of 1</param>
        /// <returns>A new Pen</returns>
        public override Pen ToPen(double scaleWidth)
        {
            Pen myPen = base.ToPen(scaleWidth);
            myPen.EndCap = _endCap;
            myPen.StartCap = _startCap;
            if (_compondArray != null) myPen.CompoundArray = _compondArray;
            if (_offset != 0F)
            {
                float[] pattern = { 0, 1 };
                float w = (float)Width;
                if (w == 0) w = 1;
                w = (float)(scaleWidth * w);
                float w2 = (Math.Abs(_offset) + (w / 2)) * 2;
                if (_compondArray != null)
                {
                    pattern = new float[_compondArray.Length];
                    for (int i = 0; i < _compondArray.Length; i++)
                    {
                        pattern[i] = _compondArray[i];
                    }
                }

                for (int i = 0; i < pattern.Length; i++)
                {
                    if (_offset > 0)
                    {
                        pattern[i] = (w / w2) * pattern[i];
                    }
                    else
                    {
                        pattern[i] = 1 - (w / w2) + ((w / w2) * pattern[i]);
                    }
                }

                myPen.CompoundArray = pattern;
                myPen.Width = w2;
            }

            if (_dashPattern != null)
            {
                myPen.DashPattern = _dashPattern;
            }
            else
            {
                if (myPen.DashStyle == DashStyle.Custom)
                {
                    myPen.DashStyle = DashStyle.Solid;
                }
            }

            switch (_joinType)
            {
                case LineJoinType.Bevel:
                    myPen.LineJoin = LineJoin.Bevel;
                    break;
                case LineJoinType.Mitre:
                    myPen.LineJoin = LineJoin.Miter;
                    break;
                case LineJoinType.Round:
                    myPen.LineJoin = LineJoin.Round;
                    break;
            }

            return myPen;
        }

        /// <summary>
        /// Handles the randomization of the cartographic properties of this stroke.
        /// </summary>
        /// <param name="generator">The random class that generates the random numbers</param>
        protected override void OnRandomize(Random generator)
        {
            base.OnRandomize(generator);
            DashStyle = DashStyle.Custom;
            _dashCap = generator.NextEnum<DashCap>();
            _startCap = generator.NextEnum<LineCap>();
            _endCap = generator.NextEnum<LineCap>();
            _dashButtons = generator.NextBoolArray(1, 20);
            _compoundButtons = generator.NextBoolArray(1, 5);
            _offset = generator.NextFloat(10);
            _joinType = generator.NextEnum<LineJoinType>();
            int len = generator.Next(0, 1);
            if (len > 0)
            {
                _decorations.Clear();
                LineDecoration ld = new LineDecoration();
                ld.Randomize(generator);
                _decorations.Add(ld);
            }
        }

        private void Configure()
        {
            _joinType = LineJoinType.Round;
            _startCap = LineCap.Round;
            _endCap = LineCap.Round;
            _decorations = new CopyList<ILineDecoration>();
        }

        #endregion
    }
}