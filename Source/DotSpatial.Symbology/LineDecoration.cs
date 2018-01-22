// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// LineDecoration
    /// </summary>
    [Serializable]
    public class LineDecoration : Descriptor, ILineDecoration
    {
        #region Fields

        private bool _flipAll;
        private bool _flipFirst;
        private bool _flip1on2;
        private bool _flipped;
        private bool _useSpacing;
        private int _numSymbols;
        private int _spacing;
        private string _spacingUnit;
        private double _offset;

        private int _percentualPosition;
        private bool _rotateWithLine;
        private IPointSymbolizer _symbol;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LineDecoration"/> class.
        /// </summary>
        public LineDecoration()
        {
            _symbol = new PointSymbolizer(SymbologyGlobal.RandomColor(), PointShape.Triangle, 10);
            _flipAll = false;
            _flipFirst = true;
            _rotateWithLine = true;
            _numSymbols = 2;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether all of the symbols should be flipped.
        /// </summary>
        [Serialize("FlipAll")]
        public bool FlipAll
        {
            get
            {
                return _flipAll;
            }

            set
            {
                _flipAll = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the first symbol in relation to the direction of the line should be flipped.
        /// </summary>
        [Serialize("FlipFirst")]
        public bool FlipFirst
        {
            get
            {
                return _flipFirst;
            }

            set
            {
                _flipFirst = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean that, if true, reverse one symbols on 2
        /// </summary>
        [Serialize("Flip1on2")]
        public bool Flip1on2
        {
            get { return _flip1on2; }
            set { _flip1on2 = value; }
        }

        /// <summary>
        /// Gets or sets the number of symbols that should be drawn on each line. (not each segment).
        /// </summary>
        [Serialize("NumSymbols")]
        public int NumSymbols
        {
            get
            {
                return _numSymbols;
            }

            set
            {
                _numSymbols = value;
            }
        }

        /// <summary>
        /// Gets or sets the offset distance measured to the left of the line in pixels.
        /// </summary>
        [Serialize("Offset")]
        public double Offset
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
        /// Gets or sets the percentual position between line start and end at which the single decoration gets drawn.
        /// </summary>
        [Serialize("PercentualPosition")]
        public int PercentualPosition
        {
            get
            {
                return _percentualPosition;
            }

            set
            {
                _percentualPosition = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the symbol should be rotated according to the direction of the line. Arrows
        /// at the ends, for instance, will point along the direction of the line, regardless of the direction of the line.
        /// </summary>
        [Serialize("RotateWithLine")]
        public bool RotateWithLine
        {
            get
            {
                return _rotateWithLine;
            }

            set
            {
                _rotateWithLine = value;
            }
        }

        /// <summary>
        /// Gets or sets the spacing between each line decoration.
        /// </summary>
        [Serialize("Spacing")]
        public int Spacing
        {
            get { return _spacing; }
            set { _spacing = value; }
        }

        /// <summary>
        /// Gets or sets the unit used by the spacing (mm or inch).
        /// </summary>
        [Serialize("SpacingUnit")]
        public string SpacingUnit
        {
            get { return _spacingUnit; }
            set { _spacingUnit = value; }
        }

        /// <summary>
        /// Gets or sets the decorative symbol.
        /// </summary>
        [Serialize("Symbol")]
        public IPointSymbolizer Symbol
        {
            get
            {
                return _symbol;
            }

            set
            {
                _symbol = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean that, if true, will cause the symbol to 
        /// be spaced according to the spacing value.
        /// </summary>
        [Serialize("UseSpacing")]
        public bool UseSpacing
        {
            get { return _useSpacing; }
            set { _useSpacing = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Given the points on this line decoration, this will cycle through and handle
        /// the drawing as dictated by this decoration.
        /// </summary>
        /// <param name="g">The graphics object used for drawing.</param>
        /// <param name="path">The path of the line.</param>
        /// <param name="scaleWidth">The double scale width for controling markers</param>
        public void Draw(Graphics g, GraphicsPath path, double scaleWidth)
        {
            // CGX TRY CATCH
            try
            {
                if (NumSymbols == 0) return;

                GraphicsPathIterator myIterator = new GraphicsPathIterator(path);
                myIterator.Rewind();
                bool isClosed;
                Size2D symbolSize = _symbol.GetSize();

                // CGX
                symbolSize.Height = symbolSize.Height * scaleWidth;
                symbolSize.Width = symbolSize.Width * scaleWidth; // fin CGX

                Bitmap symbol = new Bitmap((int)symbolSize.Width, (int)symbolSize.Height);
                Graphics sg = Graphics.FromImage(symbol);
                _symbol.Draw(sg, new Rectangle(0, 0, (int)symbolSize.Width, (int)symbolSize.Height));
                sg.Dispose();

                Matrix oldMat = g.Transform;

                GraphicsPath gp = new GraphicsPath();
                GraphicsPath pastGP = new GraphicsPath();
                while (myIterator.NextSubpath(gp, out isClosed) > 0)
                {
                    PointF[] points = gp.PathPoints;

                    int start = 0, end = points.Length - 1;

                    if (NumSymbols == 1)
                    {
                        // single decoration spot
                        if (_percentualPosition == 0)
                        {
                            // at start of the line
                            DrawImage(g, points[start], points[start + 1], points[start], FlipFirst ^ FlipAll, symbol, oldMat, scaleWidth);
                        }
                        else if (_percentualPosition == 100)
                        {
                            // at end of the line
                            DrawImage(g, points[end - 1], points[end], points[end], FlipFirst ^ FlipAll, symbol, oldMat, scaleWidth);
                        }
                        else
                        {
                            // somewhere in between start and end
                            double totalLength = GetLength(points, start, end);
                            double span = totalLength * _percentualPosition / 100;
                            List<DecorationSpot> spot = GetPosition(points, span, start, end);
                            if (spot.Count > 1) DrawImage(g, spot[1].Before, spot[1].After, spot[1].Position, FlipFirst ^ FlipAll, symbol, oldMat, scaleWidth);
                        }
                    }
                    else
                    {
                        // more than one decoration spot
                        double totalLength = GetLength(points, start, end);
                        List<DecorationSpot> spots = new List<DecorationSpot>();
                        double span = 0.0;
                        if (_useSpacing)
                        {
                            var dpi = g.DpiX;
                            var mm = GetSpacingValue_mm();
                            span = ((mm * dpi) / 25.4) * scaleWidth;
                        }
                        else
                        {
                            span = Math.Round(totalLength / (NumSymbols - 1), 4);
                        }
                        spots = GetPosition(points, span, start, end);

                        for (int i = 0; i < spots.Count; i++)
                        {
                            using (var pen = new Pen(Color.Black, 2))
                            {
                                if (!pastGP.IsOutlineVisible(spots[i].Position, pen))
                                {
                                    DrawImage(g, spots[i].Before, spots[i].After, spots[i].Position, i == 0 ? (FlipFirst ^ FlipAll) : FlipAll, symbol, oldMat, scaleWidth);
                                }
                            }
                        }
                    }

                    pastGP.AddPath(gp, false);
                }
            }
            catch (Exception)
            { }
        }

        private double GetSpacingValue_mm()
        {
            double dSpan = 0.0;

            if (SpacingUnit == "mm") { dSpan = Spacing; };
            if (SpacingUnit == "in") { dSpan = Spacing * 25.4; };

            return dSpan;
        }

        /// <summary>
        /// Gets the size that is needed to draw this decoration with max. 2 symbols.
        /// </summary>
        /// <returns>The legend symbol size.</returns>
        public Size GetLegendSymbolSize()
        {
            Size size = _symbol.GetLegendSymbolSize();
            if (NumSymbols >= 1) size.Width *= 2; // add space for the line between the decorations
            return size;
        }

        /// <summary>
        /// Handles the creation of random content for the LineDecoration.
        /// </summary>
        /// <param name="generator">The Random class that generates random numbers</param>
        protected override void OnRandomize(Random generator)
        {
            base.OnRandomize(generator);

            // _symbol is randomizable so the base method already will have randomized this class
            _flipAll = generator.NextBool();
            _flipFirst = generator.NextBool();
            _rotateWithLine = generator.NextBool();
            _numSymbols = generator.Next(0, 10);
            _offset = generator.NextDouble() * 10;
        }

        /// <summary>
        /// Flips the given angle by 180 degree.
        /// </summary>
        /// <param name="angle">Angle, that should be flipped.</param>
        private static void FlipAngle(ref float angle)
        {
            angle = angle + 180;
            if (angle > 360) angle -= 360;
        }

        /// <summary>
        /// Gets the angle of the line between StartPoint and EndPoint taking into account the direction of the line.
        /// </summary>
        /// <param name="startPoint">StartPoint of the line.</param>
        /// <param name="endPoint">EndPoint of the line.</param>
        /// <returns>Angle of the given line.</returns>
        private static float GetAngle(PointF startPoint, PointF endPoint)
        {
            double deltaX = endPoint.X - startPoint.X;
            double deltaY = endPoint.Y - startPoint.Y;
            double angle = Math.Atan(deltaY / deltaX);

            if (deltaX < 0)
            {
                if (deltaY <= 0) angle += Math.PI;
                if (deltaY > 0) angle -= Math.PI;
            }

            return (float)(angle * 180.0 / Math.PI);

            // CGX
            /*double angle_deg = (angle * 180 / Math.PI);
            if (deltaX < 0.0)
            {
                double transform = angle_deg + 180.0;
                double quotient = Math.Floor(transform / 360.0);
                double remainder = transform - 360.0 * quotient;
                angle_deg = remainder;

            }
            return (float)angle_deg;*/
            // Fin CGX
        }

        /// <summary>
        /// Gets the length of the line between startpoint and endpoint.
        /// </summary>
        /// <param name="startPoint">Startpoint of the line.</param>
        /// <param name="endPoint">Endpoint of the line.</param>
        /// <returns>Length of the line.</returns>
        private static double GetLength(PointF startPoint, PointF endPoint)
        {
            double dx = endPoint.X - startPoint.X;
            double dy = endPoint.Y - startPoint.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Gets the length of all the lines between startpoint and endpoint.
        /// </summary>
        /// <param name="points">Points of the lines we want to measure.</param>
        /// <param name="start">Startpoint of measuring.</param>
        /// <param name="end">Endpoint of measuring.</param>
        /// <returns>Combined length of all lines between startpoint and endpoint.</returns>
        private static double GetLength(PointF[] points, int start, int end)
        {
            double result = 0;
            for (int i = start; i < end; i++)
            {
                result += GetLength(points[i], points[i + 1]);
            }

            return result;
        }

        /// <summary>
        /// Get the decoration spots that result from the given line and segLength. The decoration spot needed for the endpoint is not included.
        /// </summary>
        /// <param name="points">Point-Array that contains the points of the line.</param>
        /// <param name="segLength">Distance between two decoration spots.</param>
        /// <param name="start">Index of the first point that belongs to the line.</param>
        /// <param name="end">Index of the last point that belongs to the line.</param>
        /// <returns>List of decoration spots that result from the given points and segLength.</returns>
        private static List<DecorationSpot> GetPosition(PointF[] points, double segLength, int start, int end)
        {
            double coveredDistance = 0; // distance between the last decoration spot and the line end; needed to get the correct position of the next decoration spot on the next line
            List<DecorationSpot> liste = new List<DecorationSpot>();

            for (int i = start; i < end; i++)
            {
                if (coveredDistance == 0)
                {
                    // startpoint of the first line or last segment ended on startpoint of next line
                    DecorationSpot result = new DecorationSpot(points[i], points[i + 1], points[i]);
                    liste.Add(result);
                    coveredDistance = 0;
                    if (double.IsInfinity(segLength)) return liste; // when segLength is infinit we're looking only for the first decoration spot
                }

                double dx = points[i + 1].X - points[i].X;
                double dy = points[i + 1].Y - points[i].Y;
                double lineLength = Math.Sqrt(dx * dx + dy * dy);
                if (coveredDistance + lineLength <= segLength)
                {
                    coveredDistance += lineLength;
                    continue; // line was shorter than segment -> does not contain any decorations
                }

                double offset = segLength - coveredDistance; // offset of the first decoration spot

                if (dx == 0)
                {
                    // line is parallel to y-axis
                    while (Math.Round(offset, 3) < Math.Round(lineLength, 3))
                    {
                        float x = points[i].X;
                        float y;
                        if (points[i].Y > points[i + 1].Y)
                        {
                            // line goes bottom up
                            y = (float)(points[i].Y - offset);
                        }
                        else
                        {
                            // line goes top down
                            y = (float)(points[i].Y + offset);
                        }

                        liste.Add(new DecorationSpot(points[i], points[i + 1], new PointF(x, y)));
                        offset += segLength;
                    }

                    if (Math.Round(offset, 3) > Math.Round(lineLength, 3))
                    {
                        var lastpnt = liste[liste.Count - 1];
                        coveredDistance = GetLength(lastpnt.Position, lastpnt.After);
                    }
                    else
                    {
                        coveredDistance = 0; // segment ends on endpoint of the current line -> reset coveredDistance to add a decoration spot at the beginning of the next line
                    }
                }
                else
                {
                    // line is not parallel to y-axis
                    double slope = dy / dx;
                    double alpha = Math.Atan(slope);

                    while (Math.Round(offset, 3) < Math.Round(lineLength, 3))
                    {
                        float x, y;
                        if (points[i].X < points[i + 1].X)
                        {
                            // line goes right to left
                            x = (float)(points[i].X + Math.Cos(alpha) * offset);
                            y = (float)(points[i].Y + Math.Sin(alpha) * offset);
                        }
                        else
                        {
                            // line goes left to right
                            x = (float)(points[i].X - Math.Cos(alpha) * offset);
                            y = (float)(points[i].Y - Math.Sin(alpha) * offset);
                        }

                        var newPoint = new PointF(x, y);
                        liste.Add(new DecorationSpot(points[i], points[i + 1], newPoint));
                        offset += segLength;
                    }

                    if (Math.Round(offset, 3) > Math.Round(lineLength, 3))
                    {
                        var lastpnt = liste[liste.Count - 1];
                        coveredDistance = GetLength(lastpnt.Position, lastpnt.After);
                    }
                    else
                    {
                        coveredDistance = 0; // segment ends on endpoint of the current line -> reset coveredDistance to add a decoration spot at the beginning of the next line
                    }
                }
            }

            return liste;
        }

        /// <summary>
        /// Draws the given symbol at the position calculated from locationPoint and _offset.
        /// </summary>
        /// <param name="g">Graphics-object needed for drawing.</param>
        /// <param name="startPoint">StartPoint of the line locationPoint belongs to. Needed for caluclating the angle and the offset of the symbol.</param>
        /// <param name="stopPoint">StopPoint of the line locationPoint belongs to. Needed for caluclating the angle and the offset of the symbol.</param>
        /// <param name="locationPoint">Position where the center of the image should be drawn.</param>
        /// <param name="flip">Indicates whether the symbol should be flipped.</param>
        /// <param name="symbol">Image that gets drawn.</param>
        /// <param name="oldMat">Matrix used for rotation.</param>
        private void DrawImage(Graphics g, PointF startPoint, PointF stopPoint, PointF locationPoint, bool flip, Bitmap symbol, Matrix oldMat, double scaleWidth)
        {
            // Move the point to the position including the offset
            PointF offset = stopPoint == locationPoint ? GetOffset(startPoint, locationPoint, scaleWidth) : GetOffset(locationPoint, stopPoint, scaleWidth);

            var point = new PointF(locationPoint.X + offset.X, locationPoint.Y + offset.Y);

            // rotate it by the given angle
            float angle = 0F;
            if (_rotateWithLine) angle = GetAngle(startPoint, stopPoint);
            if (flip) FlipAngle(ref angle);
            Matrix rotated = g.Transform;
            rotated.RotateAt(angle, point);
            g.Transform = rotated;

            // correct the position so that the symbol is drawn centered
            point.X -= (float)symbol.Width / 2;
            point.Y -= (float)symbol.Height / 2;
            g.DrawImage(symbol, point);
            g.Transform = oldMat;
        }

        /// <summary>
        /// Calculates the offset needed to show the decoration spot of each line at the same position as it is shown for the horizontal line in the linesymbol editor window.
        /// </summary>
        /// <param name="point">Startpoint of the line the decoration spot belongs to.</param>
        /// <param name="nextPoint">Endpoint of the line the decoration spot belongs to.</param>
        /// <param name="dScaleWidth">Reference scale.</param>
        /// <returns>Offset that must be added to the decoration spots locationPoint for it to be drawn with the given _offset.</returns>
        private PointF GetOffset(PointF point, PointF nextPoint, double dScaleWidth)
        {
            var dX = nextPoint.X - point.X;
            var dY = nextPoint.Y - point.Y;
            var alpha = Math.Atan(-dX / dY);
            double x, y;

            double dOffset = _offset * dScaleWidth;
            if (Flip1on2)
            {
                if (_flipped) { dOffset = -(dOffset + 2); }
                _flipped = !_flipped;
            }

            if (dX == 0 && point.Y > nextPoint.Y)
            {
                // line is parallel to y-axis and goes bottom up
                x = Math.Cos(alpha) * -dOffset;
                y = Math.Sin(alpha) * dOffset;
            }
            else if (dY != 0 && point.Y > nextPoint.Y)
            {
                // line goes bottom up
                x = Math.Cos(alpha) * -dOffset;
                y = Math.Sin(alpha) * -dOffset;
            }
            else
            {
                // line is parallel to x-axis or goes top down
                x = Math.Cos(alpha) * dOffset;
                y = Math.Sin(alpha) * dOffset;
            }

            return new PointF((float)x, (float)y);
        }

        #endregion

        #region Classes

        private struct DecorationSpot
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DecorationSpot"/> struct.
            /// </summary>
            /// <param name="before">The start point of the line the decoration belongs to.</param>
            /// <param name="after">The end point of the line the decoration belongs to.</param>
            /// <param name="position">The position of this decoration spot.</param>
            public DecorationSpot(PointF before, PointF after, PointF position)
            {
                After = after;
                Before = before;
                Position = position;
            }

            /// <summary>
            /// Gets the end point of the line the decoration belongs to.
            /// </summary>
            public PointF After { get; }

            /// <summary>
            /// Gets the start point of the line the decoration belongs to.
            /// </summary>
            public PointF Before { get; }

            /// <summary>
            /// Gets the position of this decoration spot.
            /// </summary>
            public PointF Position { get; }
        }

        #endregion
    }
}