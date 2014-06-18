// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/9/2009 3:42:59 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;
using DotSpatial.Data;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    [Serializable,
    XmlRoot("CartographicStroke")]
    public class CartographicStroke : SimpleStroke, ICartographicStroke
    {
        #region Private Variables

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
        /// Creates a new instance of CartographicStroke
        /// </summary>
        public CartographicStroke()
        {
            Color = SymbologyGlobal.RandomDarkColor(1);
            Configure();
        }

        /// <summary>
        /// Getnerates a cartographic stroke of the specified color
        /// </summary>
        /// <param name="color"></param>
        public CartographicStroke(Color color)
        {
            Color = color;
            Configure();
        }

        private void Configure()
        {
            _joinType = LineJoinType.Round;
            _startCap = LineCap.Round;
            _endCap = LineCap.Round;
            _decorations = new CopyList<ILineDecoration>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a pen for drawing the non-decorative portion of the line.
        /// </summary>
        ///<param name="scaleWidth">The base width in pixels that is equivalent to a width of 1</param>
        /// <returns>A new Pen</returns>
        public override Pen ToPen(double scaleWidth)
        {
            Pen myPen = base.ToPen(scaleWidth);
            myPen.EndCap = _endCap;
            myPen.StartCap = _startCap;
            if (_compondArray != null) myPen.CompoundArray = _compondArray;
            if (_offset != 0F)
            {
                float[] pattern = new float[] { 0, 1 };
                float w = (float)Width;
                if (w == 0) w = 1;
                w = (float)(scaleWidth * w);
                float w2 = (Math.Abs(_offset) + w / 2) * 2;
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
                        pattern[i] = 1 - (w / w2) + (w / w2) * pattern[i];
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
                case LineJoinType.Bevel: myPen.LineJoin = LineJoin.Bevel; break;
                case LineJoinType.Mitre: myPen.LineJoin = LineJoin.Miter; break;
                case LineJoinType.Round: myPen.LineJoin = LineJoin.Round; break;
            }
            return myPen;
        }

        /// <summary>
        /// Draws the actual path, overriding the base behavior to include markers.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="path"></param>
        /// <param name="scaleWidth"></param>
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

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets an array of floating point values ranging from 0 to 1 that
        /// indicate the start and end point for where the line should draw.
        /// </summary>
        [XmlIgnore]
        public float[] CompoundArray
        {
            get { return _compondArray; }
            set { _compondArray = value; }
        }

        /// <summary>
        /// gets or sets the DashCap for both the start and end caps of the dashes
        /// </summary>
        [Serialize("DashCap")]
        public DashCap DashCap
        {
            get { return _dashCap; }
            set { _dashCap = value; }
        }

        /// <summary>
        /// Gets or sets the DashPattern as an array of floating point values from 0 to 1
        /// </summary>
        [XmlIgnore]
        public float[] DashPattern
        {
            get { return _dashPattern; }
            set { _dashPattern = value; }
        }

        /// <summary>
        /// Gets or sets the line decoration that describes symbols that should
        /// be drawn along the line as decoration.
        /// </summary>
        [Serialize("Decorations")]
        public IList<ILineDecoration> Decorations
        {
            get { return _decorations; }
            set { _decorations = value; }
        }

        /// <summary>
        /// Gets or sets the line cap for both the start and end of the line
        /// </summary>
        [Serialize("EndCap")]
        public LineCap EndCap
        {
            get { return _endCap; }
            set { _endCap = value; }
        }

        /// <summary>
        /// Gets or sets the OGC line characteristic that controls how connected segments
        /// are drawn where they come together.
        /// </summary>
        [Serialize("JoinType")]
        public LineJoinType JoinType
        {
            get { return _joinType; }
            set { _joinType = value; }
        }

        /// <summary>
        /// Gets or sets the line cap for both the start and end of the line
        /// </summary>
        [Serialize("LineCap")]
        public LineCap StartCap
        {
            get { return _startCap; }
            set { _startCap = value; }
        }

        /// <summary>
        /// This is a cached version of the horizontal pattern that should appear in the custom dash control.
        /// This is only used if DashStyle is set to custom, and only controls the pattern control,
        /// and does not directly affect the drawing pen.
        /// </summary>
        public bool[] DashButtons
        {
            get { return _dashButtons; }
            set { _dashButtons = value; }
        }

        /// <summary>
        /// This is a cached version of the vertical pattern that should appear in the custom dash control.
        /// This is only used if DashStyle is set to custom, and only controls the pattern control,
        /// and does not directly affect the drawing pen.
        /// </summary>
        public bool[] CompoundButtons
        {
            get { return _compoundButtons; }
            set { _compoundButtons = value; }
        }

        /// <summary>
        /// Gets or sets the floating poing offset (in pixels) for the line to be drawn to the left of
        /// the original line.  (Internally, this will modify the width and compound array for the
        /// actual pen being used, as Pens do not support an offset property).
        /// </summary>
        [Serialize("Offset")]
        public float Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        #endregion

        #region Protected Methods

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

        #endregion
    }
}