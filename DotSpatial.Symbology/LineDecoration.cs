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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/9/2009 4:53:01 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using DotSpatial.Serialization;
using System.Collections.Generic;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// LineDecoration
    /// </summary>
    [Serializable]
    public class LineDecoration : Descriptor, ILineDecoration
    {
        #region Private Variables

        private int _percentualPosition;
        private bool _flipAll;
        private bool _flipFirst;
        private int _numSymbols;
        private double _offset;
        private bool _rotateWithLine;
        private IPointSymbolizer _symbol;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of LineDecoration
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

        #region Methods

        /// <summary>
        /// Given the points on this line decoration, this will cycle through and handle
        /// the drawing as dictated by this decoration.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="path"></param>
        /// <param name="scaleWidth">The double scale width for controling markers</param>
        public void Draw(Graphics g, GraphicsPath path, double scaleWidth)
        {
            try // CGX try catch
            {
                if (NumSymbols == 0) return;
                GraphicsPathIterator myIterator = new GraphicsPathIterator(path);
                myIterator.Rewind();
                int start, end;
                bool isClosed;
                Size2D symbolSize = _symbol.GetSize();

                //CGX
                symbolSize.Height = symbolSize.Height * scaleWidth;
                symbolSize.Width = symbolSize.Width * scaleWidth;
                //fin CGX

                Bitmap symbol = new Bitmap((int)symbolSize.Width, (int)symbolSize.Height);
                Graphics sg = Graphics.FromImage(symbol);
                _symbol.Draw(sg, new Rectangle(0, 0, (int)symbolSize.Width, (int)symbolSize.Height));
                sg.Dispose();

                Matrix oldMat = g.Transform;
                PointF[] points;
                if (path.PointCount == 0) return;
                try
                {
                    points = path.PathPoints;
                }
                catch
                {
                    return;
                }


                while (myIterator.NextSubpath(out start, out end, out isClosed) > 0)
                {
                    if (NumSymbols == 1) //single decoration spot
                    {
                        if (_percentualPosition == 0) // at start of the line
                        {
                            DrawImage(g, points[start], points[start + 1], points[start], (FlipFirst ^ FlipAll), symbol, oldMat);
                        }
                        else if (_percentualPosition == 100) // at end of the line
                        {
                            DrawImage(g, points[end - 1], points[end], points[end], (FlipFirst ^ FlipAll), symbol, oldMat);
                        }
                        else  //somewhere in between start and end
                        {
                            double totalLength = GetLength(points, start, end);
                            double span = totalLength * (double)_percentualPosition / 100;
                            List<DecorationSpot> spot = GetPosition(points, span, start, end);
                            if (spot.Count > 1)
                                DrawImage(g, spot[1].Before, spot[1].After, spot[1].Position, (FlipFirst ^ FlipAll), symbol, oldMat);
                        }
                    }
                    else // more than one decoration spot
                    {
                        double totalLength = GetLength(points, start, end);
                        double span = Math.Round(totalLength / (NumSymbols - 1), 4);
                        List<DecorationSpot> spots = GetPosition(points, span, start, end);
                        spots.Add(new DecorationSpot(points[end - 1], points[end], points[end])); //add the missing end point
                        for (int i = 0; i < spots.Count; i++)
                            DrawImage(g, spots[i].Before, spots[i].After, spots[i].Position, i == 0 ? (FlipFirst ^ FlipAll) : FlipAll, symbol, oldMat);
                    }
                }
            }
            catch (Exception)
            { return; }
        }

        /// <summary>
        /// Draws the given symbol at the position calculated from locationPoint and _offset.
        /// </summary>
        /// <param name="g">Graphics-object needed for drawing.</param>
        /// <param name="startPoint">StartPoint of the line locationPoint belongs to. Needed for caluclating the angle and the offset of the symbol.</param>
        /// <param name="stopPoint">StopPoint of the line locationPoint belongs to. Needed for caluclating the angle and the offset of the symbol.</param>
        /// <param name="locationPoint">Position where the center of the image should be drawn.</param>
        /// <param name="Flip">Indicates whether the symbol should be flipped.</param>
        /// <param name="symbol">Image that gets drawn.</param>
        /// <param name="oldMat">Matrix used for rotation.</param>
        private void DrawImage(Graphics g, PointF startPoint, PointF stopPoint, PointF locationPoint, bool Flip, Bitmap symbol, Matrix oldMat)
        {
            // Move the point to the position including the offset
            PointF offset;
            if (stopPoint == locationPoint)
                offset = GetOffset(startPoint, locationPoint);
            else
                offset = GetOffset(locationPoint, stopPoint);

            var point = new PointF((float)(locationPoint.X + offset.X), (float)(locationPoint.Y + offset.Y));

            // rotate it by the given angle
            float angle = 0F;
            if (_rotateWithLine) angle = GetAngle(startPoint, stopPoint);
            if (Flip) FlipAngle(ref angle);
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
        /// <param name="StartPoint">StartPoint of the line.</param>
        /// <param name="EndPoint">EndPoint of the line.</param>
        /// <returns>Angle of the given line.</returns>
        private static float GetAngle(PointF StartPoint, PointF EndPoint)
        {
            double deltaX = EndPoint.X - StartPoint.X;
            double deltaY = EndPoint.Y - StartPoint.Y;
            double angle = Math.Atan(deltaY / deltaX);

            if (deltaX < 0)
            {
                if (deltaY <= 0) angle += Math.PI;
                if (deltaY > 0) angle -= Math.PI;
            }
            return (float)(angle * 180.0 / Math.PI);

            //CGX
            /*double angle_deg = (angle * 180 / Math.PI);
            if (deltaX < 0.0)
            {
                double transform = angle_deg + 180.0;
                double quotient = Math.Floor(transform / 360.0);
                double remainder = transform - 360.0 * quotient;
                angle_deg = remainder;

            }
            return (float)angle_deg;*/
            //Fin CGX
        }

        /// <summary>
        /// Gets the size that is needed to draw this decoration with max. 2 symbols.
        /// </summary>
        public Size GetLegendSymbolSize()
        {
            Size size = _symbol.GetLegendSymbolSize();
            if (NumSymbols >= 1) size.Width *= 2; //add space for the line between the decorations
            return size;
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
        private double GetLength(PointF[] points, int start, int end)
        {
            double result = 0;
            for (int i = start; i < end; i++)
            {
                result += GetLength(points[i], points[i + 1]);
            }
            return result;
        }

        /// <summary>
        /// Calculates the offset needed to show the decoration spot of each line at the same position as it is shown for the horizontal line in the linesymbol editor window.
        /// </summary>
        /// <param name="point">Startpoint of the line the decoration spot belongs to.</param>
        /// <param name="nextPoint">Endpoint of the line the decoration spot belongs to.</param>
        /// <returns>Offset that must be added to the decoration spots locationPoint for it to be drawn with the given _offset.</returns>
        private PointF GetOffset(PointF point, PointF nextPoint)
        {
            var dX = nextPoint.X - point.X;
            var dY = nextPoint.Y - point.Y;
            var alpha = Math.Atan(-dX / dY);
            double x = 0, y = 0;

            if (dX == 0 && point.Y > nextPoint.Y) // line is parallel to y-axis and goes bottom up
            {
                x = Math.Cos(alpha) * -_offset;
                y = Math.Sin(alpha) * _offset;
            }
            else if (dY != 0 && point.Y > nextPoint.Y) // line goes bottom up
            {
                x = Math.Cos(alpha) * -_offset;
                y = Math.Sin(alpha) * -_offset;
            }
            else // line is parallel to x-axis or goes top down
            {
                x = Math.Cos(alpha) * _offset;
                y = Math.Sin(alpha) * _offset;
            }
            return new PointF((float)x, (float)y);
        }

        /// <summary>
        /// Get the decoration spots that result from the given line and segLength. The decoration spot needed for the endpoint is not included.
        /// </summary>
        /// <param name="points">Point-Array that contains the points of the line.</param>
        /// <param name="segLength">Distance between two decoration spots.</param>
        /// <param name="start">Index of the first point that belongs to the line.</param>
        /// <param name="end">Index of the last point that belongs to the line.</param>
        /// <returns>List of decoration spots that result from the given points and segLength.</returns>
        private List<DecorationSpot> GetPosition(PointF[] points, double segLength, int start, int end)
        {
            double coveredDistance = 0; //distance between the last decoration spot and the line end; needed to get the correct position of the next decoration spot on the next line
            List<DecorationSpot> liste = new List<DecorationSpot>();

            for (int i = start; i < end; i++)
            {
                if (coveredDistance == 0) //startpoint of the first line or last segment ended on startpoint of next line
                {
                    DecorationSpot result = new DecorationSpot();
                    result.Position = points[i];
                    result.Before = points[i];
                    result.After = points[i + 1];
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

                if (dx == 0) // line is parallel to y-axis
                {
                    while (Math.Round(offset, 3) < Math.Round(lineLength, 3))
                    {
                        float x = points[i].X;
                        float y;
                        if (points[i].Y > points[i + 1].Y) // line goes bottom up
                        {
                            y = (float)(points[i].Y - offset);
                        }
                        else // line goes top down
                        {
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
                else // line is not parallel to y-axis
                {
                    double slope = dy / dx;
                    double alpha = Math.Atan(slope);

                    while (Math.Round(offset, 3) < Math.Round(lineLength, 3))
                    {
                        float x, y;
                        if (points[i].X < points[i + 1].X) // line goes right to left
                        {
                            x = (float)(points[i].X + Math.Cos(alpha) * offset);
                            y = (float)(points[i].Y + Math.Sin(alpha) * offset);
                        }
                        else // line goes left to right
                        {
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

        private struct DecorationSpot
        {
            public PointF After;
            public PointF Before;
            public PointF Position;

            public DecorationSpot(PointF Before, PointF After, PointF Position)
            {
                this.After = After;
                this.Before = Before;
                this.Position = Position;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the decorative symbol.
        /// </summary>
        [Serialize("Symbol")]
        public IPointSymbolizer Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        /// <summary>
        /// Gets or sets a boolean that, if true, flips the first symbol in relation
        /// to the direction of the line.
        /// </summary>
        [Serialize("FlipFirst")]
        public bool FlipFirst
        {
            get { return _flipFirst; }
            set { _flipFirst = value; }
        }

        /// <summary>
        /// Gets or sets a boolean that, if true, reverses all of the symbols.
        /// </summary>
        [Serialize("FlipAll")]
        public bool FlipAll
        {
            get { return _flipAll; }
            set { _flipAll = value; }
        }

        /// <summary>
        /// Gets or sets a boolean that, if true, will cause the symbol to
        /// be rotated according to the direction of the line.  Arrows
        /// at the ends, for instance, will point along the direction of
        /// the line, regardless of the direction of the line.
        /// </summary>
        [Serialize("RotateWithLine")]
        public bool RotateWithLine
        {
            get { return _rotateWithLine; }
            set { _rotateWithLine = value; }
        }

        /// <summary>
        /// Gets or sets the number of symbols that should be drawn on each
        /// line.  (not each segment).
        /// </summary>
        [Serialize("NumSymbols")]
        public int NumSymbols
        {
            get { return _numSymbols; }
            set { _numSymbols = value; }
        }

        /// <summary>
        /// Gets or sets the percentual position between line start and end at which the single decoration gets drawn.
        /// </summary>
        [Serialize("PercentualPosition")]
        public int PercentualPosition
        {
            get { return _percentualPosition; }
            set { _percentualPosition = value; }
        }

        /// <summary>
        /// Gets or sets the offset distance measured to the left of the line in pixels.
        /// </summary>
        [Serialize("Offset")]
        public double Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        #endregion

        #region Protected Methods

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

        #endregion
    }
}